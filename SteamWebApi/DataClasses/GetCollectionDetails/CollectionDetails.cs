using System.Text.Json.Serialization;

namespace SteamWebApi.DataClasses.GetCollectionDetails
{
    public class CollectionDetails
    {
        [JsonPropertyName("result")]
        public int ResultCode { get; set; }

        [JsonPropertyName("resultcount")]
        public int ResultCount { get; set; }

        [JsonPropertyName("collectiondetails")]
        public CollectionDetail[] Collections { get; set; } = [];
    }
}
