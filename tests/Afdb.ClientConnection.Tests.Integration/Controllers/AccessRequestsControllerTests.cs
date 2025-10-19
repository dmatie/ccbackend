using Afdb.ClientConnection.Application.Commands.AccessRequestCmd;
using Afdb.ClientConnection.Application.Common.Exceptions;
using Afdb.ClientConnection.Application.Queries.AccessRequestQrs;
using Afdb.ClientConnection.Domain.Entities;
using Afdb.ClientConnection.Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace Afdb.ClientConnection.Tests.Integration.Controllers;

[Collection("IntegrationTestCollection")]
public class AccessRequestsControllerTests
{
    private readonly HttpClient _client;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly CustomWebApplicationFactory<Program> _factory;

    public AccessRequestsControllerTests(CustomWebApplicationFactoryFixture fixture)
    {
        _factory = fixture.Factory;
        _client = _factory.CreateClient();
        _scopeFactory = _factory.Services.GetRequiredService<IServiceScopeFactory>();
    }

    [Fact]
    public async Task CreateAccessRequest_WithValidData_ReturnsCreatedResult()
    {
        var (function, country, profile, fintype) = await TestDataSeeder.SeedReferenceDataAsync(_scopeFactory);
        if (function == null || country == null || profile == null || fintype == null)
        {
            throw new Exception("Echec du seed des données de référence");
        }

        List<RequestProject> projects = [
            new RequestProject(){ SapCode="P-NG-KF0-004"},
            new RequestProject(){ SapCode="P-KM-K00-014"},
            ];

        var command = new CreateAccessRequestCommand
        {
            Email = "testok@example.com",
            FirstName = "Test",
            LastName = "Ok",
            FunctionId = function.Id,
            CountryId = country.Id,
            BusinessProfileId = profile.Id,
            Projects = projects
        };

        var response = await _client.PostAsJsonAsync("/api/accessRequests", command);
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();

        var result = JsonSerializer.Deserialize<CreateAccessRequestResponse>(responseContent,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        Assert.NotNull(result);
        Assert.NotEqual(Guid.Empty, result.AccessRequest.Id);
        Assert.Equal(command.Email, result.AccessRequest.Email);
    }

    [Fact]
    public async Task CreateAccessRequest_WithValidDataWithoutOptionalIds_ReturnsCreatedResult()
    {
        var command = new CreateAccessRequestCommand
        {
            Email = "testok2@example.com",
            FirstName = "Test",
            LastName = "Ok2"
            // FunctionId, CountryId, BusinessProfileId sont null (optionnels)
        };

        var response = await _client.PostAsJsonAsync("/api/accessRequests", command);
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();

        var result = JsonSerializer.Deserialize<CreateAccessRequestResponse>(responseContent,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        Assert.NotNull(result);
        Assert.NotEqual(Guid.Empty, result.AccessRequest.Id);
        Assert.Equal(command.Email, result.AccessRequest.Email);
        Assert.Null(result.AccessRequest.FunctionId);
        Assert.Null(result.AccessRequest.CountryId);
        Assert.Null(result.AccessRequest.BusinessProfileId);
    }

    [Fact]
    public async Task CreateAccessRequest_WithRequestAlreadyExistsWithSameEmail_ReturnsValidationError()
    {
        var (function, country, profile, fintype) = await TestDataSeeder.SeedReferenceDataAsync(_scopeFactory);
        if (function == null || country == null || profile == null || fintype == null)
        {
            throw new Exception("Echec du seed des données de référence");
        }

        var command = new CreateAccessRequestCommand
        {
            Email = "testsamemail@example.com",
            FirstName = "Same",
            LastName = "Mail",
            FunctionId = function.Id,
            CountryId = country.Id,
            BusinessProfileId = profile.Id
        };

        var response = await _client.PostAsJsonAsync("/api/accessrequests", command);
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<CreateAccessRequestResponse>(responseContent,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        Assert.NotNull(result);

        // Creer une deuxieme demande avec le meme email
        command = new CreateAccessRequestCommand
        {
            Email = result.AccessRequest.Email,
            FirstName = "Alber",
            LastName = "Camu",
            CountryId = Guid.NewGuid()
        };

        var response2 = await _client.PostAsJsonAsync("/api/accessrequests", command);
        Assert.Equal(HttpStatusCode.BadRequest, response2.StatusCode);
        var content = await response2.Content.ReadAsStringAsync();

        var result2 = JsonSerializer.Deserialize<ExceptionContent>(content,
             new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        Assert.NotNull(result2);
        Assert.NotEmpty(result2.Errors);
        Assert.Equal("ERR.AccessRequest.RequestAlreadyExists", result2.Errors[0].Error);
    }

    [Fact]
    public async Task CreateAccessRequest_WithUserAlreadyExists_ReturnsValidationError()
    {
        var (function, country, profile, fintype) = await TestDataSeeder.SeedReferenceDataAsync(_scopeFactory);
        if (function == null || country == null || profile == null || fintype == null)
        {
            throw new Exception("Echec du seed des données de référence");
        }

        string email = "testalreadyexist@example.com";
        string firstName = "Test";
        string lastName = "Exist";

        // Seed user directement dans la DB
        await TestDataSeeder.CreateUser(_scopeFactory, email, firstName, lastName);

        var command = new CreateAccessRequestCommand
        {
            Email = email,
            FirstName = firstName,
            LastName = lastName,
            BusinessProfileId = profile.Id
        };

        var response = await _client.PostAsJsonAsync("/api/accessrequests", command);

        var responseContent = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var result = JsonSerializer.Deserialize<ExceptionContent>(responseContent,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        Assert.NotNull(result);
        Assert.NotEmpty(result.Errors);
        Assert.Equal("ERR.AccessRequest.EmailAlreadyExists", result.Errors[0].Error);
    }

    [Fact]
    public async Task CreateAccessRequest_WithUserAlreadyExistsInEntraID_ReturnsValidationError()
    {
        // Arrange → forcer GraphService à dire que l'utilisateur existe
        _factory.MockGraphService.Setup(g => g.UserExistsAsync("testentraexist@example.com", It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var command = new CreateAccessRequestCommand
        {
            Email = "testentraexist@example.com",
            FirstName = "Entra",
            LastName = "Exist",
            FunctionId = Guid.NewGuid(),
            CountryId = Guid.NewGuid()
        };

        var response = await _client.PostAsJsonAsync("/api/accessrequests", command);

        var responseContent = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var result = JsonSerializer.Deserialize<ExceptionContent>(responseContent,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        Assert.NotNull(result);
        Assert.NotEmpty(result.Errors);
        Assert.Equal("ERR.General.EmailExistsInEntra", result.Errors[0].Error);
    }

    [Fact]
    public async Task GetAccessRequest_WithValidId_ReturnsOkResult()
    {
        // Arrange
        var (function, country, profile, fintype) = await TestDataSeeder.SeedReferenceDataAsync(_scopeFactory);
        if (function == null || country == null || profile == null || fintype == null)
        {
            throw new Exception("Echec du seed des données de référence");
        }

        var createCommand = new CreateAccessRequestCommand
        {
            Email = "getOne-test@example.com",
            FirstName = "Get",
            LastName = "One",
            FunctionId = function.Id,
            CountryId = country.Id,
            BusinessProfileId = profile.Id
        };


        var createResponse = await _client.PostAsJsonAsync("/api/accessrequests", createCommand);
        var createContent = await createResponse.Content.ReadAsStringAsync();
        var createResult = JsonSerializer.Deserialize<CreateAccessRequestResponse>(createContent,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");
        _client.DefaultRequestHeaders.Add("X-Test-Role", "Admin"); // rôle insuffisant

        // Act
        var response = await _client.GetAsync($"/api/accessrequests/{createResult!.AccessRequest.Id}");
        response.EnsureSuccessStatusCode();
        var responseContent = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Contains("Get", responseContent);
        Assert.Contains("One", responseContent);
    }

    [Fact]
    public async Task GetAccessRequests_AsAdmin_ReturnsOkWithResults()
    {
        // Arrange
        var (function, country, profile, fintype) = await TestDataSeeder.SeedReferenceDataAsync(_scopeFactory);
        if (function == null || country == null || profile == null || fintype == null)
        {
            throw new Exception("Echec du seed des données de référence");
        }

        var createCommand = new CreateAccessRequestCommand
        {
            Email = "getok-test@example.com",
            FirstName = "Get",
            LastName = "Ok",
            FunctionId = function.Id,
            CountryId = country.Id,
            BusinessProfileId = profile.Id
        };

        var createResponse = await _client.PostAsJsonAsync("/api/accessrequests", createCommand);
        var createContent = await createResponse.Content.ReadAsStringAsync();
        var createResult = JsonSerializer.Deserialize<CreateAccessRequestResponse>(createContent,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        var request = new HttpRequestMessage(HttpMethod.Get, "/api/accessrequests?status=Pending");

        request.Headers.Authorization = new AuthenticationHeaderValue("Test"); // si tu utilises un handler de test
        request.Headers.Add("X-Test-Role", "Admin"); // si ton middleware de test lit ça

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(createResult);

        var content = await response.Content.ReadFromJsonAsync<GetAccessRequestsResponse>();
        Assert.NotNull(content);
        Assert.NotEmpty(content.AccessRequests);
        Assert.Equal(content.AccessRequests[0].Email, createResult.AccessRequest.Email);
        Assert.Equal(content.AccessRequests[0].FirstName, createResult.AccessRequest.FirstName);
        Assert.Equal(content.AccessRequests[0].LastName, createResult.AccessRequest.LastName);
        Assert.Equal(content.AccessRequests[0].Status, createResult.AccessRequest.Status);
    }

    [Fact]
    public async Task GetAccessRequests_AsUserWithoutRole_ReturnsForbidden()
    {
        // Arrange
        var request = new HttpRequestMessage(HttpMethod.Get, "/api/accessrequests");
        request.Headers.Authorization = new AuthenticationHeaderValue("Test");
        request.Headers.Add("X-Test-Role", "User"); // simulons un rôle non autorisé

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task GetAccessRequests_WithoutAuth_ReturnsUnauthorized()
    {
        // Arrange
        var request = new HttpRequestMessage(HttpMethod.Get, "/api/accessrequests");

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task CreateAccessRequest_WithInvalidGuid_ReturnsValidationError()
    {
        var command = new CreateAccessRequestCommand
        {
            Email = "testguid@example.com",
            FirstName = "Test",
            LastName = "Guid",
            FunctionId = Guid.Empty // GUID vide devrait être rejeté
        };

        var response = await _client.PostAsJsonAsync("/api/accessrequests", command);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var responseContent = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<ExceptionContent>(responseContent,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        Assert.NotNull(result);
        Assert.NotEmpty(result.Errors);
    }

    [Fact]
    public async Task CreateAccessRequest_WithAllOptionalIds_ReturnsCreatedResult()
    {
        var (function, country, profile, fintype) = await TestDataSeeder.SeedReferenceDataAsync(_scopeFactory);
        if (function == null || country == null || profile == null || fintype == null)
        {
            throw new Exception("Echec du seed des données de référence");
        }

        var command = new CreateAccessRequestCommand
        {
            Email = "testallids@example.com",
            FirstName = "Test",
            LastName = "AllIds",
            FunctionId = function.Id,
            CountryId = country.Id,
            BusinessProfileId = profile.Id
        };

        var response = await _client.PostAsJsonAsync("/api/accessrequests", command);
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<CreateAccessRequestResponse>(responseContent,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        Assert.NotNull(result);
        Assert.Equal(function.Id, result.AccessRequest.FunctionId);
        Assert.Equal(country.Id, result.AccessRequest.CountryId);
        Assert.Equal(profile.Id, result.AccessRequest.BusinessProfileId);
    }

    [Fact]
    public async Task GenerateOtp_ReturnsCreated()
    {
        var (function, country, profile, fintype) = await TestDataSeeder.SeedReferenceDataAsync(_scopeFactory);
        if (function == null || country == null || profile == null || fintype == null)
        {
            throw new Exception("Echec du seed des données de référence");
        }

        var command = new CreateAccessRequestCommand
        {
            Email = "generateotp@email.com",
            FirstName = "Generate",
            LastName = "OTP",
            FunctionId = function.Id,
            CountryId = country.Id,
            BusinessProfileId = profile.Id
        };

        var createResponse = await _client.PostAsJsonAsync("/api/accessrequests", command);
        var createContent = await createResponse.Content.ReadAsStringAsync();
        var createResult = JsonSerializer.Deserialize<CreateAccessRequestResponse>(createContent,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        Assert.NotNull(createResult);


        var commandOtp = new { Email = "generateotp@email.com" };
        var response = await _client.PostAsJsonAsync("/api/accessrequests/GenerateOtp", commandOtp);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task IsAccessRequestRejected_ReturnsFalse_WhenNoRejected()
    {
        var email = "integration@email.com";
        var response = await _client.GetAsync($"/api/accessrequests/is-rejected?email={email}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var value = await response.Content.ReadFromJsonAsync<bool>();
        Assert.False(value);
    }

    [Fact]
    public async Task IsAccessRequestRejected_ReturnsTrue_WhenRejected()
    {
        var email = "rejectedok@email.com";

        var (function, country, profile, fintype) = await TestDataSeeder.SeedReferenceDataAsync(_scopeFactory);
        if (function == null || country == null || profile == null || fintype == null)
        {
            throw new Exception("Echec du seed des données de référence");
        }

        var command = new CreateAccessRequestCommand
        {
            Email = "rejectedok@email.com",
            FirstName = "Test",
            LastName = "Rejected",
            FunctionId = function.Id,
            CountryId = country.Id,
            BusinessProfileId = profile.Id
        };

        var createResponse = await _client.PostAsJsonAsync("/api/accessrequests", command);
        var createContent = await createResponse.Content.ReadAsStringAsync();
        var createResult = JsonSerializer.Deserialize<CreateAccessRequestResponse>(createContent,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        Assert.NotNull(createResult);

        await TestDataSeeder.CreateUser(_scopeFactory, _factory.AdminUserEmail, _factory.AdminUserFirstName, _factory.AdminUserLastName);


        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");
        _client.DefaultRequestHeaders.Add("X-Test-Role", "Admin"); // rôle insuffisant

        // 2. Rejeter la demande
        var rejectCommand = new
        {
            RejectionReason = "Test rejection"
        };
        var rejectResponse = await _client.
            PostAsJsonAsync($"/api/accessrequests/{createResult.AccessRequest.Id}/reject", rejectCommand);
        rejectResponse.EnsureSuccessStatusCode();

        // Préparer la base de données pour qu'une demande rejetée existe pour cet email
        // (à adapter selon votre setup de test)

        var checkResponse = await _client.GetAsync($"/api/accessrequests/is-rejected?email={email}");
        Assert.Equal(HttpStatusCode.OK, checkResponse.StatusCode);
        var value = await checkResponse.Content.ReadFromJsonAsync<bool>();
        Assert.True(value);
    }

    [Fact]
    public async Task VerifyOtp_ReturnsTrue_WhenOtpIsValid()
    {
        var email = "otpvalid@email.com";

        var (function, country, profile, fintype) = await TestDataSeeder.SeedReferenceDataAsync(_scopeFactory);
        if (function == null || country == null || profile == null || fintype == null)
        {
            throw new Exception("Echec du seed des données de référence");
        }

        var command = new CreateAccessRequestCommand
        {
            Email = email,
            FirstName = "Test",
            LastName = "Rejected",
            FunctionId = function.Id,
            CountryId = country.Id,
            BusinessProfileId = profile.Id
        };

        var createResponse = await _client.PostAsJsonAsync("/api/accessrequests", command);
        var createContent = await createResponse.Content.ReadAsStringAsync();
        var createResult = JsonSerializer.Deserialize<CreateAccessRequestResponse>(createContent,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        Assert.NotNull(createResult);


        // Générer un OTP
        var generateCommand = new { Email = email, IsEmailExist = true };
        var generateResponse = await _client.PostAsJsonAsync("/api/accessrequests/GenerateOtp", generateCommand);
        generateResponse.EnsureSuccessStatusCode();

        // Récupérer le code OTP généré (à adapter selon votre logique, ici on suppose qu'il est retourné)
        // Si le code n'est pas retourné, il faut le récupérer depuis la DB ou mocker le service
        var otpCode = await TestDataSeeder.GetOtpAsync(_scopeFactory, email);
        Assert.NotNull(otpCode);

        // Vérifier l'OTP
        var verifyCommand = new { Email = email, Code = otpCode };
        var verifyResponse = await _client.PostAsJsonAsync("/api/accessrequests/verify-otp", verifyCommand);
        verifyResponse.EnsureSuccessStatusCode();
        var isValid = await verifyResponse.Content.ReadFromJsonAsync<bool>();
        Assert.True(isValid);
    }

    [Fact]
    public async Task VerifyOtp_ReturnsFalse_WhenOtpIsInvalid()
    {
        var email = "otpinvalid@email.com";
        var verifyCommand = new { Email = email, Code = "000000" };
        var verifyResponse = await _client.PostAsJsonAsync("/api/accessrequests/verify-otp", verifyCommand);
        verifyResponse.EnsureSuccessStatusCode();
        var isValid = await verifyResponse.Content.ReadFromJsonAsync<bool>();
        Assert.False(isValid);
    }


    [Fact]
    public async Task Patch_UpdateRejectedAccessRequest_ShouldReturnCreatedAndUpdatedRequest()
    {
        // Arrange

        var (function, country, profile, fintype) = await TestDataSeeder.SeedReferenceDataAsync(_scopeFactory);
        if (function == null || country == null || profile == null || fintype == null)
        {
            throw new Exception("Echec du seed des données de référence");
        }

        string email = "testpatch@afdb.org";

        var command = new CreateAccessRequestCommand
        {
            Email = email,
            FirstName = "Intial",
            LastName = "Value",
            FunctionId = function.Id,
            CountryId = country.Id,
            BusinessProfileId = profile.Id,
            Projects = [new RequestProject { SapCode = "PRJ001" }]
        };

        var createResponse = await _client.PostAsJsonAsync("/api/accessrequests", command);
        var createContent = await createResponse.Content.ReadAsStringAsync();
        var createResult = JsonSerializer.Deserialize<CreateAccessRequestResponse>(createContent,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });


        Assert.NotNull(createResult);

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");
        _client.DefaultRequestHeaders.Add("X-Test-Role", "Admin"); // rôle insuffisant
        //Ajouter le user admin dans la BD
        await TestDataSeeder.CreateUser(_scopeFactory, _factory.AdminUserEmail, _factory.AdminUserFirstName, _factory.AdminUserLastName);

        // 2. Rejeter la demande
        var rejectCommand = new
        {
            RejectionReason = "Test rejection"
        };
        var rejectResponse = await _client.
            PostAsJsonAsync($"/api/accessrequests/{createResult.AccessRequest.Id}/reject", rejectCommand);
        rejectResponse.EnsureSuccessStatusCode();


        var updteCommand = new UpdateRejectedAccessRequestCommand
        {
            Email = email,
            FirstName = "Pacth",
            LastName = "Commande",
            FunctionId = function.Id,
            CountryId = country.Id,
            BusinessProfileId = profile.Id,
            FinancingTypeId = fintype.Id,
            Projects = [new UpdateRequestProject { SapCode = "PRJ001" }, new UpdateRequestProject { SapCode = "PRJ002" }]
        };

        // Act
        var response = await _client.PatchAsJsonAsync("/api/AccessRequests", updteCommand);

        // Assert
        Assert.True(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var result = await response.Content.ReadFromJsonAsync<UpdateRejectedAccessRequestResponse>();
        Assert.NotNull(result);
        Assert.NotNull(result.AccessRequest);
        Assert.Equal(updteCommand.Email, result.AccessRequest.Email);
        Assert.Equal(updteCommand.FirstName, result.AccessRequest.FirstName);
        Assert.Equal(updteCommand.LastName, result.AccessRequest.LastName);
        Assert.Equal(updteCommand.FunctionId, result.AccessRequest.FunctionId);
        Assert.Equal(updteCommand.CountryId, result.AccessRequest.CountryId);
        Assert.Equal(updteCommand.BusinessProfileId, result.AccessRequest.BusinessProfileId);
        Assert.Equal(updteCommand.FinancingTypeId, result.AccessRequest.FinancingTypeId);
        Assert.Equal(2, result.AccessRequest.SelectedProjectCodes.Count);
        Assert.Contains("MSG.AccessRequestSubmitted", result.Message);
        Assert.True(result.AccessRequest.CanBeProcessed);
    }

    [Fact]
    public async Task Patch_UpdateRejectedAccessRequest_WhenNotRejected_ShouldReturnBadRequest()
    {
        // Arrange

        var (function, country, profile, fintype) = await TestDataSeeder.SeedReferenceDataAsync(_scopeFactory);
        if (function == null || country == null || profile == null || fintype == null)
        {
            throw new Exception("Echec du seed des données de référence");
        }

        string email = "testpatchbaqreq@afdb.org";

        var command = new CreateAccessRequestCommand
        {
            Email = email,
            FirstName = "Intial",
            LastName = "Value",
            FunctionId = function.Id,
            CountryId = country.Id,
            BusinessProfileId = profile.Id,
            Projects = [new RequestProject { SapCode = "PRJ001" }]
        };

        var createResponse = await _client.PostAsJsonAsync("/api/accessrequests", command);
        var createContent = await createResponse.Content.ReadAsStringAsync();
        var createResult = JsonSerializer.Deserialize<CreateAccessRequestResponse>(createContent,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        Assert.NotNull(createResult);

        var updteCommand = new UpdateRejectedAccessRequestCommand
        {
            Email = email,
            FirstName = "Pacth",
            LastName = "Commande",
            FunctionId = function.Id,
            CountryId = country.Id,
            BusinessProfileId = profile.Id,
            FinancingTypeId = fintype.Id,
            Projects = [new UpdateRequestProject { SapCode = "PRJ001" }, new UpdateRequestProject { SapCode = "PRJ002" }]
        };

        // Act
        var response = await _client.PatchAsJsonAsync("/api/AccessRequests", updteCommand);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var responseContent = await response.Content.ReadAsStringAsync();


        var result = JsonSerializer.Deserialize<ExceptionContent>(responseContent,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        Assert.NotNull(result);
        Assert.NotEmpty(result.Errors);
        Assert.Equal("ERR.AccessRequest.RejectedRequestNotExist", result.Errors[0].Error);

    }
}