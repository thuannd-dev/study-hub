using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using TodoWeb.Application.Dtos.UserModel.Contracts;
using TodoWeb.Domains.AppsettingsConfigurations;

namespace TodoWeb.Application.Services.Users.FacebookService
{
    public class FacebookCredentialService : IFacebookCredentialService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly FacebookSettings _facebookSettings;
        public FacebookCredentialService(IHttpClientFactory httpClientFactory, IOptions<FacebookSettings> facebookSettings)
        {
            _httpClientFactory = httpClientFactory;
            _facebookSettings = facebookSettings.Value;
        }

        public async Task<FacebookTokenValidationResult> ValidateAccessTokenAsync(string accessToken)
        {
            var formattedUrl = string.Format(
                _facebookSettings.TokenValidationUrl, accessToken,
                _facebookSettings.AppId, _facebookSettings.AppSecret
                );
            var result = await _httpClientFactory.CreateClient().GetAsync(formattedUrl);
            result.EnsureSuccessStatusCode();//ko thành công ném ra ngoại lệ 
            var content = await result.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(content))
            {
                throw new Exception("Invalid response from Facebook token validation.");
            }
            return JsonConvert.DeserializeObject<FacebookTokenValidationResult>(content);
            //content là chuỗi json, deserialize thành đối tượng FacebookTokenValidationResult

        }
        public async Task<FacebookUserInfoModel> GetUserInfoAsync(string accessToken)
        {
            var formattedUrl = string.Format(
                _facebookSettings.UserInfoUrl, accessToken
                );
            var result = await _httpClientFactory.CreateClient().GetAsync(formattedUrl);
            result.EnsureSuccessStatusCode();//ko thành công ném ra ngoại lệ 
            var content = await result.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(content))
            {
                throw new Exception("Invalid response from Facebook token validation.");
            }
            return JsonConvert.DeserializeObject<FacebookUserInfoModel>(content);
            //content là chuỗi json, deserialize thành đối tượng FacebookTokenValidationResult
        }

    }
}
