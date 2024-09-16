using System.Text.Json.Serialization;

namespace SteamWebApi.DataClasses.GetPublishedFileDetails
{
    public class PublishedFileDetails
    {
        [JsonPropertyName("result")]
        public int ResultCode { get; set; }

        [JsonPropertyName("resultcount")]
        public int ResultCount { get; set; }

        [JsonPropertyName("publishedfiledetails")]
        public PublishedFileDetail[] Files { get; set; } = [];
    }
}
