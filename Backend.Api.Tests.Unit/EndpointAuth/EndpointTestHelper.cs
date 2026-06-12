using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;
using UCR.ECCI.PI.ThemePark.Backend.DependencyInjection;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Users.Entities;

namespace UCR.ECCI.PI.ThemePark.Backend.Api.Tests.Unit.ApiAuth;

/// <summary>
/// Helper class for creating test clients with mocked authentication and database.
/// This class encapsulates the complex setup required for integration testing with ASP.NET Core.
/// </summary>
public class EndpointTestHelper
{
    private readonly WebApplicationFactory<Program> _factory;

    /// <summary>
    /// Initializes a new instance of the ApiTestHelper class.
    /// </summary>
    /// <param name="factory">The WebApplicationFactory to use for creating test clients.</param>
    public EndpointTestHelper(WebApplicationFactory<Program> factory)
    {
        _factory = factory ?? throw new ArgumentNullException(nameof(factory));
    }

    /// <summary>
    /// Creates an HTTP client with mocked authentication for testing purposes.
    /// This method was created using the help of Artificial Intelligence.
    /// As the authentication cannot be completely tested (would need real email and password), 
    /// it mocks the authentication state and only tests the API response based on authentication status.
    /// </summary>
    /// <param name="isAuthenticated">Whether the user should be considered authenticated</param>
    /// <param name="isExpired">Whether the token should be considered expired</param>
    /// <param name="additionalClaims">Additional claims to add to the test user</param>
    /// <param name="mockUsers">List of users to return from the mocked database</param>
    /// <returns>HttpClient configured for testing</returns>
    public HttpClient CreateClientWithMockAuth(
        bool isAuthenticated, 
        bool isExpired = false,
        IEnumerable<Claim>? additionalClaims = null,
        List<User>? mockUsers = null)
    {
        return _factory.WithWebHostBuilder(builder =>
            {
                builder.UseEnvironment("Testing");

                // Override configuration for testing
                builder.ConfigureAppConfiguration((context, configBuilder) =>
                    {
                        configBuilder.Sources.Clear();

                        // Add test configuration
                        var testConfig = new Dictionary<string, string?>
                        {
                            {"AzureAd:Instance", "https://test.b2clogin.com/"},
                            {"AzureAd:Domain", "test.onmicrosoft.com"},
                            {"AzureAd:TenantId", "test-tenant-id"},
                            {"AzureAd:ClientId", "test-client-id"},
                            {"AzureAd:SwaggerClientId", "test-swagger-client-id"},
                            {"AzureAd:SignUpSignInPolicyId", "B2C_1_Test"},
                            {"AzureAd:Scopes", "test.scope"},
                            {"Logging:LogLevel:Default", "Warning"} // Reduce noise in tests
                        };

                        configBuilder.AddInMemoryCollection(testConfig);
                    });

                    builder.ConfigureServices(services =>
                        {
                            // Remove existing authentication services
                            RemoveExistingAuthenticationServices(services);

                            // Add mocked authentication
                            AddMockAuthentication(services, isAuthenticated, isExpired, additionalClaims);
                        });

                    // Configure the application pipeline to include mock endpoints
                    builder.Configure(app =>
                        {
                            app.UseRouting();
                            app.UseAuthentication();
                            app.UseAuthorization();
                
                            app.UseEndpoints(endpoints =>
                                {
                                    // Map the original clean architecture endpoints
                                    endpoints.MapCleanArchitectureEndpoints();
                    
                                    // Add our mock authentication test endpoint
                                    endpoints.MapGet("/mock-auth-test", () => Results.Ok("1"))
                                        .WithName("MockAuthTest")
                                        .RequireAuthorization();
                                });
                        });
                }).CreateClient();
        }

    /// <summary>
    /// Creates a test client with default authenticated user and empty database.
    /// </summary>
    /// <returns>HttpClient with authenticated test user</returns>
    public HttpClient CreateAuthenticatedClient(List<User>? mockUsers = null)
    {
        return CreateClientWithMockAuth(isAuthenticated: true, mockUsers: mockUsers);
    }

    /// <summary>
    /// Creates a test client with unauthenticated user and empty database.
    /// </summary>
    /// <returns>HttpClient with unauthenticated test user</returns>
    public HttpClient CreateUnauthenticatedClient(List<User>? mockUsers = null)
    {
        return CreateClientWithMockAuth(isAuthenticated: false, mockUsers: mockUsers);
    }

    /// <summary>
    /// Creates a test client with expired token.
    /// </summary>
    /// <returns>HttpClient with expired token</returns>
    public HttpClient CreateExpiredTokenClient(List<User>? mockUsers = null)
    {
        return CreateClientWithMockAuth(isAuthenticated: true, isExpired: true, mockUsers: mockUsers);
    }

    /// <summary>
    /// Creates a test client with custom claims for advanced testing scenarios.
    /// </summary>
    /// <param name="claims">Custom claims for the test user</param>
    /// <param name="mockUsers">List of users to return from the mocked database</param>
    /// <returns>HttpClient with custom claims</returns>
    public HttpClient CreateClientWithCustomClaims(IEnumerable<Claim> claims, List<User>? mockUsers = null)
    {
        return CreateClientWithMockAuth(isAuthenticated: true, additionalClaims: claims, mockUsers: mockUsers);
    }

    private static void RemoveExistingAuthenticationServices(IServiceCollection services)
    {
        // Remove existing authentication services
        var authDescriptors = services
            .Where(d => d.ServiceType.FullName?.Contains("Authentication") == true ||
                       d.ServiceType.FullName?.Contains("JwtBearer") == true ||
                       d.ServiceType.FullName?.Contains("Identity") == true)
            .ToList();

        foreach (var descriptor in authDescriptors)
        {
            services.Remove(descriptor);
        }
    }

    private static void AddMockAuthentication(
        IServiceCollection services, 
        bool isAuthenticated, 
        bool isExpired,
        IEnumerable<Claim>? additionalClaims)
    {
        // Configure test handler options
        services.AddSingleton(new MockAuthOptions
        {
            IsAuthenticated = isAuthenticated,
            IsExpired = isExpired,
            AdditionalClaims = additionalClaims?.ToList() ?? new List<Claim>()
        });

        // Add authorization services
        services.AddAuthorization();

        // Add test authentication
        services.AddAuthentication("Test")
            .AddScheme<AuthenticationSchemeOptions, MockAuthHandler>
                ("Test", options => { });

        // Set default authentication scheme
        services.Configure<AuthenticationOptions>(options =>
        {
            options.DefaultAuthenticateScheme = "Test";
            options.DefaultChallengeScheme = "Test";
        });
    }
}

/// <summary>
/// Options for the test authentication handler
/// </summary>
public class MockAuthOptions
{
    public bool IsAuthenticated { get; set; }
    public bool IsExpired { get; set; }
    public List<Claim> AdditionalClaims { get; set; } = new();
}

/// <summary>
/// Test authentication handler for mocking authentication in tests
/// </summary>
public class MockAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly MockAuthOptions _options;

    public MockAuthHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> authOptions,
        ILoggerFactory logger,
        UrlEncoder encoder,
        MockAuthOptions options)
        : base(authOptions, logger, encoder)
    {
        _options = options;
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!_options.IsAuthenticated)
        {
            Logger.LogInformation("Test was not authenticated. Failure message: Not authenticated for test");
            return Task.FromResult(AuthenticateResult.Fail("Not authenticated for test"));
        }

        if (_options.IsExpired)
        {
            Logger.LogInformation("Test token was expired. Failure message: Token expired for test");
            return Task.FromResult(AuthenticateResult.Fail("Token expired for test"));
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, "Test User"),
            new(ClaimTypes.NameIdentifier, "test-user-id"),
        };

        claims.AddRange(_options.AdditionalClaims);

        var identity = new ClaimsIdentity(claims, "Test");
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, "Test");

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }

    protected override Task HandleChallengeAsync(AuthenticationProperties properties)
    {
        Logger.LogInformation("AuthenticationScheme: Test was challenged.");
        Response.StatusCode = 401;
        return Task.CompletedTask;
    }
}
