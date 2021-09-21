using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Microsoft.Owin.Security.Notifications;

[assembly: OwinStartup(typeof(PDPA.Startup))]

namespace PDPA
{
    public class Startup
    {
        string clientId = System.Configuration.ConfigurationManager.AppSettings["ClientId"];
        string redirectUri = System.Configuration.ConfigurationManager.AppSettings["redirectUri"];
        static string tenant = System.Configuration.ConfigurationManager.AppSettings["Tenant"];
        string authority = String.Format(System.Globalization.CultureInfo.InvariantCulture,
System.Configuration.ConfigurationManager.AppSettings["Authority"], tenant);
        public void Configuration(IAppBuilder app)
        {
            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);
            app.UseCookieAuthentication(new CookieAuthenticationOptions());
            app.UseOpenIdConnectAuthentication(
            new OpenIdConnectAuthenticationOptions
            {
                ClientId = clientId,
                ClientSecret= "c227d14f-55f9-45f2-a25d-a52afdc17188",
                Authority = authority,
                RedirectUri = redirectUri,
                PostLogoutRedirectUri = redirectUri,
                Scope = OpenIdConnectScope.OpenIdProfile,
                ResponseType = OpenIdConnectResponseType.CodeIdToken,
                TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = false // This is a simplification
                },
                Notifications = new OpenIdConnectAuthenticationNotifications
                {
                    AuthenticationFailed = OnAuthenticationFailed
                }
            });
            }
        private Task OnAuthenticationFailed(AuthenticationFailedNotification<OpenIdConnectMessage,
OpenIdConnectAuthenticationOptions> context)
        {
            context.HandleResponse();
            context.Response.Redirect("/?errormessage=" + context.Exception.Message);
            return Task.FromResult(0);
        }



    }
}
