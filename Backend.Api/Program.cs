using UCR.ECCI.PI.ThemePark.Backend.DependencyInjection;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Core.Middlewares;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCleanArchitecture(builder.Configuration);

// Register the CORS (Cross-Origin Resource Sharing) service
builder.Services.AddCors(options =>

    {
        options.AddDefaultPolicy(policy =>
            {
                policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
            });
    });

// Add logging services for Azure App Services
builder.Logging.AddAzureWebAppDiagnostics();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    #if SWAGGER
    app.UseSwagger();
    
var azureEntraIdConfiguration = builder.Configuration.GetSection("AzureAd");
    var apiScopeUrl = $"https://{azureEntraIdConfiguration["Domain"]}/{azureEntraIdConfiguration["ClientId"]}";
    var scopes = azureEntraIdConfiguration["Scopes"]?
        .Split(',')
        .Select(scope => $"{apiScopeUrl}/{scope}")
        .ToList() ?? new List<string>();

    app.UseSwaggerUI(x =>
        {
            x.SwaggerEndpoint("/swagger/v1/swagger.json", "Backend API");
            x.OAuthClientId(azureEntraIdConfiguration["SwaggerClientId"]);
            x.OAuthUsePkce();
            var scopesString = string.Join(',', scopes);
            x.OAuthScopes(scopesString);
        });
    #endif
}

app.UseMiddleware<ExceptionHandlerMiddleware>();
app.UseHttpsRedirection();
app.UseCors();               
app.UseAuthentication();     
app.UseAuthorization();      
app.MapCleanArchitectureEndpoints(); 
app.Run();

// Make the implicit Program class public for testing
public partial class Program { }
