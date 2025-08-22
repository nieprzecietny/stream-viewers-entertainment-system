using MiniIntegrations.Components;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Auth: Cookies + OAuth 2.1 (Authorization Code + PKCE)
builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultScheme = "Cookies";
        options.DefaultChallengeScheme = "kick";
    })
    .AddCookie("Cookies", options =>
    {
        options.SlidingExpiration = true;
    })
    .AddOAuth("kick", options =>
    {
        // Wartości dostawcy (uzupełnij)
        options.ClientId = "";
        options.ClientSecret = "";

        // Punkty końcowe OAuth 2.1 (uzupełnij poprawnymi adresami)
        options.AuthorizationEndpoint = "https://id.kick.com/oauth/authorize";
        options.TokenEndpoint = "https://id.kick.com/oauth/token";
        options.UserInformationEndpoint = "https://api.kick.com/public/v1/users";

        // Callback zarejestruj u dostawcy jako Redirect URI
        options.CallbackPath = "/signin-kick";

        // OAuth 2.1 zaleca PKCE
        options.UsePkce = true;

        options.SaveTokens = true;

        // Zakresy – dopasuj do dostawcy
        options.Scope.Clear();
        options.Scope.Add("user:read");
        options.Scope.Add("channel:read");
        options.Scope.Add("channel:write");
        options.Scope.Add("chat:write");
        options.Scope.Add("streamkey:read");
        options.Scope.Add("events:subscribe");
        options.Scope.Add("moderation:ban");

        // // Mapowanie claimów – dopasuj klucze do odpowiedzi userinfo
        options.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "user_id");
        options.ClaimActions.MapJsonKey(ClaimTypes.Name, "name");
        options.ClaimActions.MapJsonKey(ClaimTypes.Email, "email");
        options.ClaimActions.MapJsonKey(ClaimTypes.Uri, "profile_picture");
        
        // Pobieranie profilu użytkownika
        options.Events = new OAuthEvents
        {
            OnCreatingTicket = async context =>
            {

                var request = new HttpRequestMessage(HttpMethod.Get, context.Options.UserInformationEndpoint);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);
                
                var response = await context.Backchannel.SendAsync(request, context.HttpContext.RequestAborted);
                response.EnsureSuccessStatusCode();
                
                using var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync(context.HttpContext.RequestAborted));

                var user = json.RootElement.GetProperty("data")[0];
                
                context.RunClaimActions(user);
            }
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddHttpContextAccessor();
// Razor Components
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.UseAuthentication();
app.UseAuthorization();

// Endpoints logowania/wylogowania
app.MapGet("/login/kick", async (HttpContext ctx, string? returnUrl) =>
{
    var redirect = string.IsNullOrWhiteSpace(returnUrl) ? "/" : returnUrl;
    await ctx.ChallengeAsync("kick", new AuthenticationProperties { RedirectUri = redirect });
});

app.MapGet("/logout", async (HttpContext ctx) =>
{
    await ctx.SignOutAsync("Cookies");
    ctx.Response.Redirect("/");
    // Jeśli dostawca wspiera federated logout, można dodać SignOutAsync("kick", ...)
    await ctx.Response.CompleteAsync();
});

// Komponenty
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();