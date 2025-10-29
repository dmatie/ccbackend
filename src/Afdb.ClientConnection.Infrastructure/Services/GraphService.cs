using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Application.DTOs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using Microsoft.IdentityModel.Tokens;

namespace Afdb.ClientConnection.Infrastructure.Services;

internal sealed class GraphService(GraphServiceClient graphClient, IConfiguration configuration , ILogger<GraphService> logger) : IGraphService
{
    public async Task<bool> UserExistsAsync(string email, CancellationToken cancellationToken = default)
    {
        try
        {
            var users = await graphClient.Users
                .GetAsync(requestConfiguration =>
                {
                    requestConfiguration.QueryParameters.Filter = $"mail eq '{email}' or userPrincipalName eq '{email}'";
                    requestConfiguration.QueryParameters.Select = new[] { "id" };
                    requestConfiguration.QueryParameters.Top = 1;
                }, cancellationToken);

            return users?.Value?.Any() == true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error checking if user exists in Entra ID: {Email}", email);
            throw new InvalidOperationException($"Error checking if user {email} exists in Entra ID: {ex.Message}", ex);
        }
    }

    public async Task<string> CreateGuestUserAsync(string email, string firstName, string lastName,
        string? organizationName, CancellationToken cancellationToken = default)
    {
        try
        {
            var invitation = new Invitation
            {
                InvitedUserEmailAddress = email,
                InvitedUserDisplayName = $"{firstName} {lastName}",
                InviteRedirectUrl = "https://myapp.com/welcome", // Configure selon vos besoins
                SendInvitationMessage = true,
                InvitedUserMessageInfo = new InvitedUserMessageInfo
                {
                    CustomizedMessageBody = $"Welcome {firstName}! You have been invited to access our client connection portal."
                }
            };

            var result = await graphClient.Invitations.PostAsync(invitation);

            if (result?.InvitedUser?.Id == null)
                throw new InvalidOperationException("Failed to create guest user - no user ID returned");

            logger.LogInformation("Guest user created successfully: {Email} with ID: {UserId}",
                email, result.InvitedUser.Id);

            return result.InvitedUser.Id;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating guest user in Entra ID: {Email}", email);
            throw new InvalidOperationException($"Failed to create guest user: {ex.Message}", ex);
        }
    }

    public async Task<bool> DeleteGuestUserAsync(string objectId, CancellationToken cancellationToken = default)
    {
        try
        {
            await graphClient.Users[objectId].DeleteAsync();

            logger.LogInformation("Guest user deleted successfully: {ObjectId}", objectId);
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deleting guest user from Entra ID: {ObjectId}", objectId);
            return false;
        }
    }

    public async Task<List<string>> GetFifcAdmin(CancellationToken cancellationToken = default)
    {
        bool useMock = configuration.GetValue<bool>("Graph:UseMock");
        List<string> adminEmails;

        if (useMock)
        {
            List<string>? mockEmails = configuration.GetSection("Graph:MockAdminUsers").Get<List<string>>();
            if (mockEmails == null || mockEmails.Count == 0)
            {
                logger.LogInformation("FIFC Admin mock emails are not configured.");
                throw new InvalidOperationException("ERR.General.MissingConfiguration");
            }

            adminEmails = [.. mockEmails!];
        }
        else
        {
            var groupName = configuration["Graph:AppGroups:FifcAdmin"];
            if (string.IsNullOrEmpty(groupName))
            {
                logger.LogInformation("FIFC Admin group name is not configured.");
                throw new InvalidOperationException("ERR.General.MissingConfiguration");
            }
            adminEmails = (await GetGroupUserEmailsAsync(groupName, cancellationToken)).ToList();
        }

        return adminEmails;
    }


    public async Task<List<string>> GetFifcDOs(CancellationToken cancellationToken = default)
    {
        bool useMock = configuration.GetValue<bool>("Graph:UseMock");
        List<string> adminEmails;

        if (useMock)
        {
            List<string>? mockEmails = configuration.GetSection("Graph:MockFifc3DOUsers").Get<List<string>>();
            if (mockEmails == null || mockEmails.Count == 0)
            {
                logger.LogInformation("FIFC Admin mock emails are not configured.");
                throw new InvalidOperationException("ERR.General.MissingConfiguration");
            }

            adminEmails = [.. mockEmails!];
        }
        else
        {
            var groupName = configuration["Graph:AppGroups:CcFifc3DO"];
            if (string.IsNullOrEmpty(groupName))
            {
                logger.LogInformation("FIFC Admin group name is not configured.");
                throw new InvalidOperationException("ERR.General.MissingConfiguration");
            }
            adminEmails = (await GetGroupUserEmailsAsync(groupName, cancellationToken)).ToList();
        }

        return adminEmails;
    }

    public async Task<List<string>> GetFifcDAs(CancellationToken cancellationToken = default)
    {
        bool useMock = configuration.GetValue<bool>("Graph:UseMock");
        List<string> adminEmails;

        if (useMock)
        {
            List<string>? mockEmails = configuration.GetSection("Graph:MockFifc3DAUsers").Get<List<string>>();
            if (mockEmails == null || mockEmails.Count == 0)
            {
                logger.LogInformation("FIFC Admin mock emails are not configured.");
                throw new InvalidOperationException("ERR.General.MissingConfiguration");
            }

            adminEmails = [.. mockEmails!];
        }
        else
        {
            var groupName = configuration["Graph:AppGroups:CcFifc3DA"];
            if (string.IsNullOrEmpty(groupName))
            {
                logger.LogInformation("FIFC Admin group name is not configured.");
                throw new InvalidOperationException("ERR.General.MissingConfiguration");
            }
            adminEmails = (await GetGroupUserEmailsAsync(groupName, cancellationToken)).ToList();
        }

        return adminEmails;
    }


    private async Task<IReadOnlyList<string>> GetGroupUserEmailsAsync(string groupName, CancellationToken cancellationToken = default)
    {
        try
        {
            // Recherche du groupe par son displayName
            var groups = await graphClient.Groups
                .GetAsync(requestConfiguration =>
                {
                    requestConfiguration.QueryParameters.Filter = $"displayName eq '{groupName}'";
                    requestConfiguration.QueryParameters.Select = new[] { "id" };
                    requestConfiguration.QueryParameters.Top = 1;
                }, cancellationToken);

            var groupId = groups?.Value?.FirstOrDefault()?.Id;
            if (string.IsNullOrEmpty(groupId))
            {
                logger.LogInformation("Groupe AD non trouvé : {GroupName}", groupName);
                throw new InvalidOperationException($"ERR.General.MissingAdGroup");
            }

            // Récupération des membres du groupe
            var members = await graphClient.Groups[groupId].Members
                .GetAsync(requestConfiguration =>
                {
                    requestConfiguration.QueryParameters.Select = new[] { "mail", "userPrincipalName" };
                    requestConfiguration.QueryParameters.Top = 100;
                }, cancellationToken);

            var emails = (members?.Value?
                .OfType<User>()
                .Select(u => !string.IsNullOrEmpty(u.Mail) ? u.Mail : u.UserPrincipalName)
                .Where(e => !string.IsNullOrEmpty(e))
                .ToList()) ?? [];

            logger.LogInformation("Utilisateurs du groupe {GroupName} récupérés : {Count} emails.", groupName, emails.Count);

            return emails!;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erreur lors de la récupération des utilisateurs du groupe AD : {GroupName}", groupName);

            throw new InvalidOperationException($"ERR.General.AdGroupFectching");
        }
    }

    public async Task<List<AzureAdUserDto>> SearchUsersAsync(string searchQuery, int maxResults = 10, CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(searchQuery))
            {
                logger.LogWarning("Search query is empty or null");
                return [];
            }

            logger.LogInformation("Searching Azure AD users with query: {SearchQuery}, MaxResults: {MaxResults}",
                searchQuery, maxResults);

            var users = await graphClient.Users
                .GetAsync(requestConfiguration =>
                {
                    requestConfiguration.QueryParameters.Search = $"\"displayName:{searchQuery}\" OR \"mail:{searchQuery}\" OR \"userPrincipalName:{searchQuery}\"";
                    requestConfiguration.QueryParameters.Select = new[] { "id", "displayName", "mail", "userPrincipalName", "jobTitle", "department" };
                    requestConfiguration.QueryParameters.Top = maxResults;
                    requestConfiguration.QueryParameters.Orderby = new[] { "displayName" };
                    requestConfiguration.Headers.Add("ConsistencyLevel", "eventual");
                }, cancellationToken);

            if (users?.Value == null || !users.Value.Any())
            {
                logger.LogInformation("No users found for query: {SearchQuery}", searchQuery);
                return [];
            }

            var result = users.Value
                .Select(user => new AzureAdUserDto
                {
                    Id = user.Id ?? string.Empty,
                    DisplayName = user.DisplayName ?? string.Empty,
                    Email = user.Mail ?? user.UserPrincipalName ?? string.Empty,
                    JobTitle = user.JobTitle,
                    Department = user.Department
                })
                .Where(dto => !string.IsNullOrEmpty(dto.Id) && !string.IsNullOrEmpty(dto.Email))
                .ToList();

            logger.LogInformation("Found {Count} users for query: {SearchQuery}", result.Count, searchQuery);

            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error searching users in Azure AD with query: {SearchQuery}", searchQuery);
            throw new InvalidOperationException($"Error searching users in Azure AD: {ex.Message}", ex);
        }
    }
}