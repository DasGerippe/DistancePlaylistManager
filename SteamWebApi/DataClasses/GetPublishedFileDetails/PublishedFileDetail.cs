using System.Text.Json.Serialization;

namespace SteamWebApi.DataClasses.GetPublishedFileDetails
{
    public class PublishedFileDetail
    {
        [JsonPropertyName("publishedfileid")]
        public string FileId { get; set; } = string.Empty;

        [JsonPropertyName("result")]
        public int ResultCode { get; set; }

        [JsonPropertyName("creator")]
        public string CreatorId { get; set; } = string.Empty;

        [JsonPropertyName("creator_app_id")]
        public int CreatorAppId { get; set; }

        [JsonPropertyName("consumer_app_id")]
        public int ConsumerAppId { get; set; }

        [JsonPropertyName("filename")]
        public string FileName { get; set; } = string.Empty;

        [JsonPropertyName("file_size")]
        public string FileSize { get; set; } = string.Empty;

        [JsonPropertyName("file_url")]
        public string FileUrl { get; set; } = string.Empty;

        [JsonPropertyName("preview_url")]
        public string PreviewUrl { get; set; } = string.Empty;

        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("time_created")]
        public long TimeCreated { get; set; }

        [JsonPropertyName("time_updated")]
        public long TimeUpdated { get; set; }

        [JsonPropertyName("visibility")]
        public int Visibility { get; set; }

        [JsonPropertyName("tags")]
        public Tag[] Tags { get; set; } = [];
    }
}
