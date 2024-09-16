using System.Text.Json.Serialization;

namespace SteamWebApi.DataClasses.GetPublishedFileDetails
{
    public class PublishedFileDetailsResponse
    {
        [JsonPropertyName("response")]
        public PublishedFileDetails? PublishedFileDetails { get; set; }
    }
}
