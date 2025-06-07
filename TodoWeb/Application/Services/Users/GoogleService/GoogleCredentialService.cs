using System.Text;
using Google.Apis.Auth;
using Microsoft.Extensions.Options;
using TodoWeb.Domains.AppsettingsConfigurations;

namespace TodoWeb.Application.Services.Users.GoogleService
{
    public class GoogleCredentialService : IGoogleCredentialService
    {
        private readonly GoogleSettings _googleSettings;
        public GoogleCredentialService(IOptions<GoogleSettings> googleSettings)
        {
            _googleSettings = googleSettings.Value;
        }

        public async Task<GoogleJsonWebSignature.Payload> VerifyCredential(string clientId, string credential)
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = new[] { clientId }
            };
            try
            {
                var payload = await GoogleJsonWebSignature.ValidateAsync(credential, settings);
                return payload;
            }
            catch (Exception ex)
            {
                // Handle other exceptions as needed  
                throw new Exception("An error occurred while verifying the Google credential.", ex);
            }
        }

        public async Task<GoogleJsonWebSignature.Payload> VerifyCredential(string credential)
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = new[] { _googleSettings.ClientId }
            };
            try
            {
                var payload = await GoogleJsonWebSignature.ValidateAsync(credential, settings);
                return payload;
            }
            catch (Exception ex)
            {
                // Handle other exceptions as needed  
                throw new Exception("An error occurred while verifying the Google credential.", ex);
            }
        }
    }
}
