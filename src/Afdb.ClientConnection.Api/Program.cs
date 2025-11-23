using Afdb.ClientConnection.Api.Middleware;
using Afdb.ClientConnection.Application;
using Afdb.ClientConnection.Infrastructure;
using Azure.Identity;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// builder.WebHost.ConfigureKestrel(options =>
// {
//     options.ListenAnyIP(80); // écoute HTTP
// });

//Config Azure KeyVault
var keyVaultUrl = builder.Configuration["KeyVault:VaultUri"];
if (!string.IsNullOrEmpty(keyVaultUrl))
{
    builder.Configuration.AddAzureKeyVault(new Uri(keyVaultUrl), new DefaultAzureCredential());
}

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "ClientConnection API",
        Version = "v1",
        Description = "API for managing client disbursement requests and approvals"
    });


    // Add OAuth2 authentication to Swagger
    c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows
        {
            AuthorizationCode = new OpenApiOAuthFlow
            {
                AuthorizationUrl = new Uri($"{builder.Configuration["AzureAd:Instance"]}{builder.Configuration["AzureAd:TenantId"]}/oauth2/v2.0/authorize"),
                TokenUrl = new Uri($"{builder.Configuration["AzureAd:Instance"]}{builder.Configuration["AzureAd:TenantId"]}/oauth2/v2.0/token"),
                Scopes = new Dictionary<string, string>
                {
                    { $"api://{builder.Configuration["AzureAd:ClientId"]}/access_as_user", "Access API as user" }
                }
            }
        }
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "oauth2"
                }
            },
            new[] { $"api://{builder.Configuration["AzureAd:ClientId"]}/access_as_user" }
        }
    });
});

// Add Microsoft Identity Web
builder.Services.AddMicrosoftIdentityWebApiAuthentication(builder.Configuration, "AzureAd");

// Add authorization policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("DOOrAdmin", policy => policy.RequireRole("Admin", "DO"));
    options.AddPolicy("InternalUsers", policy => policy.RequireRole("Admin", "DO", "DA"));
    options.AddPolicy("ExternalUsers", policy => policy.RequireRole("ExternalUser"));
});

// Add CORS

var allowedOrigins = builder.Configuration
    .GetSection("Cors:AllowedOrigins")
    .Get<string[]>()!
    .Where(origin => !origin.Contains("*")) // exclure les wildcards
    .ToArray();

builder.Services.AddCors(options =>
{
    options.AddPolicy("DefaultPolicy", policy =>
    {
        policy.WithOrigins(allowedOrigins)
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

builder.Services.Configure<SecurityHeadersOptions>(
    builder.Configuration.GetSection(SecurityHeadersOptions.SectionName)
);

// Add HTTP Context Accessor
builder.Services.AddHttpContextAccessor();

// Add Application and Infrastructure layers
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsEnvironment("Test"))
{
    app.UseCors(option => option.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ClientConnection API V1");
        c.OAuthClientId(builder.Configuration["AzureAd:ClientId"]);
        c.OAuthUsePkce();
        c.OAuthScopeSeparator(" ");
    });
}

if (!app.Environment.IsDevelopment())
{
    app.UseHsts(); 
}

//app.UseHttpsRedirection();

app.UseSecurityHeaders();

app.UseCors("DefaultPolicy");

app.UseAuthentication();
app.UseUserContext();
app.UseAuthorization();

// Payload Encryption/Decryption Middlewares
// IMPORTANT: Ces middlewares doivent être AVANT ExceptionHandlingMiddleware
// pour que les erreurs d'encryption soient catchées proprement
app.UseMiddleware<PayloadDecryptionMiddleware>();
app.UseMiddleware<PayloadEncryptionMiddleware>();

// Add custom exception handling middleware
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapControllers();

// Health check endpoint
app.MapGet("/health", () => "Healthy")
   .WithName("Health Check")
   .WithOpenApi();

// Redirect to swagger page
app.MapGet("/", () => Results.Redirect("/swagger"))
   .ExcludeFromDescription(); 

app.Run();

public partial class Program { }