using Blazored.Toast;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using Radzen;
using UCR.ECCI.PI.ThemePark.Frontend.Application.Authentication.Services;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Components;
using UCR.ECCI.PI.ThemePark.Frontend.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddRadzenComponents();

builder.Services.AddCleanArchitecture(builder.Configuration);

var azureEntraIdConfiguration = builder.Configuration.GetRequiredSection("AzureAd");
var apiScopeUrl = $"https://{azureEntraIdConfiguration["Domain"]}/{azureEntraIdConfiguration["ApiClientId"]}";
var scopes = azureEntraIdConfiguration["Scopes"]
    .Split(',')
    .Select(scope => $"{apiScopeUrl}/{scope}")
    .ToList();
scopes.Add("offline_access");
scopes.Add("openid");

builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(msIdentityOptions =>
    {
        azureEntraIdConfiguration.Bind(msIdentityOptions);

        msIdentityOptions.CallbackPath = "/signin-oidc";
        msIdentityOptions.ResponseType = "code";

        msIdentityOptions.SaveTokens = true;
        msIdentityOptions.Prompt = "login";
        foreach (var scope in scopes)
        {
            msIdentityOptions.Scope.Add(scope);
        }
        
        msIdentityOptions.Events ??= new OpenIdConnectEvents();
        msIdentityOptions.Events.OnTokenValidated = async context =>
        {
            var authService = context.HttpContext.RequestServices.GetRequiredService<ICustomAuthenticationService>();
            await authService.TriggerUserRegistrationAsync(context.Principal);
            var claimTransformer = context.HttpContext.RequestServices.GetRequiredService<IClaimsTransformation>();
            var transformed = await claimTransformer.TransformAsync(context.Principal);
            context.Principal = transformed;
            await BackendValidator.AuthenticateWithBackendAsync(context);

            if (context.Properties != null)
            {
                context.Properties.Items[".AuthScheme"] =
                    context.Scheme?.Name ?? 
                    (context.Properties.Items.TryGetValue(".AuthScheme", out var existing) ? existing : null);
            }
        };
    });

builder.Services.AddAuthorization(
    options =>
    {
        // List the permissions; at the moment only CRUDS
        var permissionList = new[]
        {
            "ListUsers",
            "CreateUsers",
            "AssignRole",
            "ManageRoles",
            "ManageBuildings",
            "ManageInterComponents",
            "ManageLearningSpaces",

            "ListBuildings",
            "ListInterComponents",
            "ListLearningSpaces"
        };

        foreach (var permission in permissionList)
        {
            options.AddPolicy(permission, policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireClaim("extension_Permissions", permission);
            });
        }
    });
builder.Services.AddHttpContextAccessor();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddBlazoredToast();

// Add logging services for Azure App Services
builder.Logging.AddAzureWebAppDiagnostics();
    
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddAdditionalAssemblies(
    [
        typeof(UCR.ECCI.PI.ThemePark.Frontend.Presentation.Blazor._Imports).Assembly
    ]);

app.MapGroup("/authentication").MapLoginAndLogout();
app.Run();
