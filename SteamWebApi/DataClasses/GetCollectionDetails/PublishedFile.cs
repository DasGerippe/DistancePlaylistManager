using System.Text.Json.Serialization;

namespace SteamWebApi.DataClasses.GetCollectionDetails
{
    public class PublishedFile
    {
        [JsonPropertyName("publishedfileid")]
        public string FileId { get; set; } = string.Empty;

        [JsonPropertyName("sortorder")]
        public int SortOrder { get; set; }

        [JsonPropertyName("filetype")]
        public int FileType { get; set; }
    }
}
