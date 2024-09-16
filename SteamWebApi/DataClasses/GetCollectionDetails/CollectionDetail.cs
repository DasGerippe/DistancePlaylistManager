using System.Text.Json.Serialization;

namespace SteamWebApi.DataClasses.GetCollectionDetails
{
    public class CollectionDetail
    {
        [JsonPropertyName("publishedfileid")]
        public string CollectionId { get; set; } = string.Empty;

        [JsonPropertyName("result")]
        public int ResultCode { get; set; }

        [JsonPropertyName("children")]
        public PublishedFile[] Files { get; set; } = [];
    }
}
