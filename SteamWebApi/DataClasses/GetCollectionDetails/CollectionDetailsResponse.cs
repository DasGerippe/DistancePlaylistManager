using System.Text.Json.Serialization;

namespace SteamWebApi.DataClasses.GetCollectionDetails
{
    public class CollectionDetailsResponse
    {
        [JsonPropertyName("response")]
        public CollectionDetails? CollectionDetails { get; set; }
    }
}
