using Afdb.ClientConnection.Application.Common.Exceptions;
using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Domain.Entities;
using Afdb.ClientConnection.Domain.EntitiesParams;
using Afdb.ClientConnection.Domain.Enums;
using Afdb.ClientConnection.Infrastructure.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using Microsoft.Graph.Models;

namespace Afdb.ClientConnection.Infrastructure.Services;

internal sealed class GraphService(
    GraphServiceClient graphClient,
    IConfiguration configuration ,
    IOptions<GraphSettings> graphSettings,
    IOptions<AzureAdSettings> azureAdSettings,
    ILogger<GraphService> logger) : IGraphService
{
    private readonly GraphSettings _graphSettings = graphSettings.Value;
    private readonly AzureAdSettings  _azureAdSettings = azureAdSettings.Value;
    public async Task<AzureAdUserDetails?> GetAzureAdUserDetailsAsync(string email, CancellationToken cancellationToken)
    {
        try
        {
            var searchResults = await SearchUsersAsync(email, 1, cancellationToken);
            
            var user = searchResults.FirstOrDefault(u =>
                u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

            if (user == null)
            {
                logger.LogWarning("Could not find user details in Azure AD for: {Email}", email);
                return null;
            }

            var nameParts = user.DisplayName.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
            var firstName = nameParts.Length > 0 ? nameParts[0] : email.Split('@')[0];
            var lastName = nameParts.Length > 1 ? nameParts[1] : string.Empty;

            return new AzureAdUserDetails(new AzureAdUserDetailsLoadParam
            {
                Id = user.Id,
                DisplayName = user.DisplayName,
                FirstName = firstName,
                LastName = lastName,
                Email = user.Email,
                JobTitle = user.JobTitle,
                Department = user.Department
            });
        }
        catch (Exception ex) when (ex is not NotFoundException)
        {
            logger.LogError(ex, "Error retrieving user details from Azure AD for: {Email}", email);
            return null;
        }
    }

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
                InviteRedirectUrl = _graphSettings.InviteRedirectUrl, // Configure selon vos besoins
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
        bool useMock = _graphSettings.UseMock;
        List<string> adminEmails;

        if (useMock)
        {
            List<string>? mockEmails =  [.. _graphSettings.MockAdminUsers];
            if (mockEmails == null || mockEmails.Count == 0)
            {
                logger.LogInformation("FIFC Admin mock emails are not configured.");
                throw new InvalidOperationException("ERR.General.MissingConfiguration");
            }

            adminEmails = [.. mockEmails!];
        }
        else
        {
            var groupName = _graphSettings.AppGroups.FifcAdmin;
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
        bool useMock = _graphSettings.UseMock;
        List<string> adminEmails;

        if (useMock)
        {
            List<string>? mockEmails =[.. _graphSettings.MockFifc3DOUsers];
            if (mockEmails == null || mockEmails.Count == 0)
            {
                logger.LogInformation("FIFC Admin mock emails are not configured.");
                throw new InvalidOperationException("ERR.General.MissingConfiguration");
            }

            adminEmails = [.. mockEmails!];
        }
        else
        {
            var groupName = _graphSettings.AppGroups.FifcDO;
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
        bool useMock = _graphSettings.UseMock;
        List<string> adminEmails;

        if (useMock)
        {
            List<string>? mockEmails = [.. _graphSettings.MockFifc3DAUsers];
            if (mockEmails == null || mockEmails.Count == 0)
            {
                logger.LogInformation("FIFC Admin mock emails are not configured.");
                throw new InvalidOperationException("ERR.General.MissingConfiguration");
            }

            adminEmails = [.. mockEmails!];
        }
        else
        {
            var groupName = _graphSettings.AppGroups.FifcDA;
            if (string.IsNullOrEmpty(groupName))
            {
                logger.LogInformation("FIFC Admin group name is not configured.");
                throw new InvalidOperationException("ERR.General.MissingConfiguration");
            }
            adminEmails = (await GetGroupUserEmailsAsync(groupName, cancellationToken)).ToList();
        }

        return adminEmails;
    }

    public async Task AddUserToGroupByNameAsync(string userId, UserRole userRole, CancellationToken cancellationToken = default)
    {
        string groupName = GetGroupNameByRole(userRole);
        if (string.IsNullOrEmpty(groupName))
            throw new InvalidOperationException("ERR.General.MissingConfiguration");

        try
        {

            var groups = await graphClient.Groups
                .GetAsync(requestConfiguration =>
                {
                    requestConfiguration.QueryParameters.Filter = $"displayName eq '{groupName}'";
                    requestConfiguration.QueryParameters.Select = new[] { "id" };
                    requestConfiguration.QueryParameters.Top = 1;
                }, cancellationToken);

            var groupId = groups?.Value?.FirstOrDefault()?.Id;
            if (string.IsNullOrEmpty(groupId))
                throw new InvalidOperationException($"ERR.AzureAd.MissingGroup");
            
            string graphUrl = _graphSettings.AddGroupMemberUrl;
            if (string.IsNullOrEmpty(graphUrl))
            {
                throw new InvalidOperationException("ERR.General.MissingConfiguration");
            }

            var requestBody = new ReferenceCreate
            {
                OdataId = $"{graphUrl}/{userId}",
            };

            await graphClient.Groups[groupId].Members.Ref.PostAsync(requestBody, cancellationToken: cancellationToken);

            logger.LogInformation("Utilisateur {UserId} ajouté au groupe {GroupName}", userId, groupName);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erreur lors de l'ajout de l'utilisateur {UserId} au groupe {GroupName}", userId, groupName);
            throw new InvalidOperationException("ERR.AzureAd.AddToGroup");
        }
    }

    public async Task AssignAppRoleToUserByRoleNameAsync(string userId, UserRole userRole, CancellationToken cancellationToken = default)
    {
        string appRoleName = GetAppRoleNameByUserRole(userRole);
        if (appRoleName == null) 
            throw new InvalidOperationException("ERR.General.MissingConfiguration");

        try
        {
            // Récupère l'ID de l'application (App Registration) depuis la configuration
            var appId = _azureAdSettings.ClientId;
            if (string.IsNullOrEmpty(appId))
                throw new InvalidOperationException("ERR.AzureAd.AppRegIdNotFound");

            // Recherche du Service Principal correspondant à l'App Registration
            var servicePrincipals = await graphClient.ServicePrincipals
                .GetAsync(requestConfiguration =>
                {
                    requestConfiguration.QueryParameters.Filter = $"appId eq '{appId}'";
                    requestConfiguration.QueryParameters.Select = new[] { "id", "appRoles" };
                    requestConfiguration.QueryParameters.Top = 1;
                }, cancellationToken);

            var servicePrincipal = servicePrincipals?.Value?.FirstOrDefault();
            if (servicePrincipal == null)
                throw new InvalidOperationException("ERR.AzureAd.AppRegFound");

            if (servicePrincipal.Id == null)
                throw new InvalidOperationException("ERR.AzureAd.AppRegFound");

            // Recherche du rôle par son nom
            var appRole = servicePrincipal.AppRoles?.FirstOrDefault(r => r.Value == appRoleName || r.DisplayName == appRoleName);
            if (appRole == null)
                throw new InvalidOperationException("ERR.AzureAd.AppRoleNotFound");

            var appRoleAssignment = new AppRoleAssignment
            {
                PrincipalId = Guid.Parse(userId),
                ResourceId = Guid.Parse(servicePrincipal.Id),
                AppRoleId = appRole.Id
            };

            await graphClient.Users[userId].AppRoleAssignments.PostAsync(appRoleAssignment, cancellationToken: cancellationToken);

            logger.LogInformation("Rôle {AppRoleName} attribué à l'utilisateur {UserId} pour l'application {AppId}", appRoleName, userId, appId);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erreur lors de l'attribution du rôle {AppRoleName} à l'utilisateur {UserId}", appRoleName, userId);
            throw new InvalidOperationException("ERR.AzureAd.AssignRole");
        }
    }


    public async Task RemoveUserFromGroupByNameAsync(string userId, UserRole userRole, CancellationToken cancellationToken = default)
    {
        string groupName = GetGroupNameByRole(userRole);
        if (string.IsNullOrEmpty(groupName))
            throw new InvalidOperationException("ERR.General.MissingConfiguration");

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
                throw new InvalidOperationException("ERR.AzureAd.MissingGroup");

            // Suppression du membre du groupe
            await graphClient.Groups[groupId].Members[userId].Ref.DeleteAsync(cancellationToken: cancellationToken);

            logger.LogInformation("Utilisateur {UserId} retiré du groupe {GroupName}", userId, groupName);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erreur lors du retrait de l'utilisateur {UserId} du groupe {GroupName}", userId, groupName);
            throw new InvalidOperationException("ERR.AzureAd.RemoveFromGroup");
        }
    }

    public async Task RemoveAppRoleFromUserByRoleNameAsync(string userId, UserRole userRole, CancellationToken cancellationToken = default)
    {
        string appRoleName = GetAppRoleNameByUserRole(userRole);
        if (appRoleName == null)
            throw new InvalidOperationException("ERR.General.MissingConfiguration");

        try
        {
            var appId = _azureAdSettings.ClientId;
            if (string.IsNullOrEmpty(appId))
                throw new InvalidOperationException("ERR.AzureAd.AppRegIdNotFound");

            // Recherche du Service Principal
            var servicePrincipals = await graphClient.ServicePrincipals
                .GetAsync(requestConfiguration =>
                {
                    requestConfiguration.QueryParameters.Filter = $"appId eq '{appId}'";
                    requestConfiguration.QueryParameters.Select = new[] { "id", "appRoles" };
                    requestConfiguration.QueryParameters.Top = 1;
                }, cancellationToken);

            var servicePrincipal = servicePrincipals?.Value?.FirstOrDefault();
            if (servicePrincipal == null || servicePrincipal.Id == null)
                throw new InvalidOperationException("ERR.AzureAd.AppRegFound");

            // Recherche du rôle par son nom
            var appRole = servicePrincipal.AppRoles?.FirstOrDefault(r => r.Value == appRoleName || r.DisplayName == appRoleName);
            if (appRole == null)
                throw new InvalidOperationException("ERR.AzureAd.AppRoleNotFound");

            // Recherche de l'assignation du rôle à l'utilisateur
            var assignments = await graphClient.Users[userId].AppRoleAssignments
                .GetAsync(cancellationToken: cancellationToken);

            var assignment = assignments?.Value?.FirstOrDefault(a =>
                a.AppRoleId == appRole.Id && a.ResourceId == Guid.Parse(servicePrincipal.Id));

            if (assignment == null)
                throw new InvalidOperationException("ERR.AzureAd.AppRoleAssignmentNotFound");

            if (assignment.Id == null)
                throw new InvalidOperationException("ERR.AzureAd.AppRoleAssignmentNotFound");

            // Suppression de l'assignation
            await graphClient.Users[userId].AppRoleAssignments[assignment.Id.ToString()].DeleteAsync(cancellationToken: cancellationToken);

            logger.LogInformation("Rôle {AppRoleName} retiré de l'utilisateur {UserId} pour l'application {AppId}", appRoleName, userId, appId);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erreur lors du retrait du rôle {AppRoleName} à l'utilisateur {UserId}", appRoleName, userId);
            throw new InvalidOperationException("ERR.AzureAd.RemoveRole");
        }
    }


    private string GetGroupNameByRole(UserRole role)
    {
        return role switch
        {
            UserRole.DO => _graphSettings.AppGroups.FifcDO ??
                    throw new InvalidOperationException("ERR.General.MissingConfiguration"),
            UserRole.DA => _graphSettings.AppGroups.FifcDA ??
                     throw new InvalidOperationException("ERR.General.MissingConfiguration"),
            UserRole.Admin => _graphSettings.AppGroups.FifcAdmin ??
                    throw new InvalidOperationException("ERR.General.MissingConfiguration"),
            _ => throw new InvalidOperationException("ERR.General.UnknownRole")
        };
    }

    private string GetAppRoleNameByUserRole(UserRole role)
    {
        return role switch
        {
            UserRole.DO => _graphSettings.AppRoles.DO ??
                    throw new InvalidOperationException("ERR.General.MissingConfiguration"),
            UserRole.DA => _graphSettings.AppRoles.DA ??
                     throw new InvalidOperationException("ERR.General.MissingConfiguration"),
            UserRole.Admin => _graphSettings.AppRoles.Admin ??
                    throw new InvalidOperationException("ERR.General.MissingConfiguration"),
            _ => throw new InvalidOperationException("ERR.General.UnknownRole")
        };
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
                .OfType<Microsoft.Graph.Models.User>()
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

    public async Task<List<AzureAdUserDetails>> SearchUsersAsync(string searchQuery, int maxResults = 10, 
        CancellationToken cancellationToken = default)
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
                    requestConfiguration.QueryParameters.Search = $"\"displayName:{searchQuery}\" OR " +
                    $"\"mail:{searchQuery}\" OR \"userPrincipalName:{searchQuery}\"";
                    requestConfiguration.QueryParameters.Select = new[] { "id", "displayName", "mail", "userPrincipalName",
                        "jobTitle", "department", "userType" };
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
                .Where(user => user.UserType == null || user.UserType.Equals("Member", StringComparison.OrdinalIgnoreCase))
                .Select(user => new AzureAdUserDetails(new AzureAdUserDetailsLoadParam
                {
                    Id = user.Id ?? string.Empty,
                    DisplayName = user.DisplayName ?? string.Empty,
                    Email = user.Mail ?? user.UserPrincipalName ?? string.Empty,
                    JobTitle = user.JobTitle,
                    Department = user.Department,
                    UserType = user.UserType
                }))
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