using System.Text.Json.Serialization;

namespace SteamWebApi.DataClasses.GetPublishedFileDetails
{
    public class Tag
    {
        [JsonPropertyName("tag")]
        public string Name { get; set; } = string.Empty;
    }
}
