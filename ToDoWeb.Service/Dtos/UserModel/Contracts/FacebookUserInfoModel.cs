using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System.Globalization;

namespace TodoWeb.Application.Dtos.UserModel.Contracts
{
    public class FacebookUserInfoModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("picture")]
        public Picture Picture { get; set; }

        [JsonProperty("link")]
        public Uri LinkOfUser { get; set; }
    }

    public class Picture
    {
        [JsonProperty("data")]
        public PictureData Data { get; set; }
    }

    public class PictureData
    {
        [JsonProperty("height")]
        public long HeightOfFacebookPictureUser { get; set; }

        [JsonProperty("is_silhouette")]
        public bool IsSilhouette { get; set; }

        [JsonProperty("url")]
        public Uri UrlOfFacebookPictureUser { get; set; }

        [JsonProperty("width")]
        public long WidthOfFacebookPictureUser { get; set; }
    }

}
