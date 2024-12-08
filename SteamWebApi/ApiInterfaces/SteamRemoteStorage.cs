using SteamWebApi.DataClasses.GetCollectionDetails;
using SteamWebApi.DataClasses.GetPublishedFileDetails;
using System.Text.Json;

namespace SteamWebApi.ApiInterfaces
{
    public class SteamRemoteStorage : SteamWebApiInterfaceBase
    {
        public override string InterfaceName => $"I{nameof(SteamRemoteStorage)}";

        internal SteamRemoteStorage(SteamWebApiClient steamWebApiClient) 
            : base(steamWebApiClient)
        {
        }

        public async Task<CollectionDetail[]> GetCollectionDetails(IEnumerable<string> collectionIds)
        {
            ArgumentNullException.ThrowIfNull(collectionIds);

            string uri = GetInterfaceMethodUri();
            HttpContent content = CreateHttpContentFromItemIds(collectionIds, "collection");
            using HttpResponseMessage httpResponse = await SteamWebApiClient.HttpClient.PostAsync(uri, content).ConfigureAwait(false);

            CollectionDetailsResponse responseObject = DeserializeResponseContent<CollectionDetailsResponse>(httpResponse);
            return responseObject.CollectionDetails?.Collections ?? [];
        }

        public async Task<CollectionDetail> GetCollectionDetail(string collectionId)
        {
            return (await GetCollectionDetails([collectionId]).ConfigureAwait(false)).FirstOrDefault()
                   ?? throw new Exception($"Could not retrieve {nameof(CollectionDetail)} for workshop file '{collectionId}'.");
        }

        public async Task<PublishedFileDetail[]> GetPublishedFileDetails(IEnumerable<string> fileIds)
        {
            ArgumentNullException.ThrowIfNull(fileIds);

            string uri = GetInterfaceMethodUri();
            HttpContent content = CreateHttpContentFromItemIds(fileIds, "item");
            using HttpResponseMessage httpResponse = await SteamWebApiClient.HttpClient.PostAsync(uri, content).ConfigureAwait(false);

            PublishedFileDetailsResponse responseObject = DeserializeResponseContent<PublishedFileDetailsResponse>(httpResponse);
            return responseObject.PublishedFileDetails?.Files ?? [];
        }

        public async Task<PublishedFileDetail> GetPublishedFileDetail(string fileId)
        {
            return (await GetPublishedFileDetails([fileId]).ConfigureAwait(false)).FirstOrDefault()
                   ?? throw new Exception($"Could not retrieve {nameof(PublishedFileDetail)} for workshop file '{fileId}'.");
        }

        private HttpContent CreateHttpContentFromItemIds(IEnumerable<string> itemIds, string itemType)
        {
            int itemIndex = 0;
            Dictionary<string, string> contentValues = itemIds.ToDictionary(_ => $"publishedfileids[{itemIndex++}]");
            contentValues.Add($"{itemType}count", itemIndex.ToString());
            HttpContent content = new FormUrlEncodedContent(contentValues);
            return content;
        }

        private T DeserializeResponseContent<T>(HttpResponseMessage httpResponse)
        {
            if (!httpResponse.IsSuccessStatusCode)
                throw new Exception($"The Steam web API returned a non-success status code: {httpResponse.StatusCode}.");

            Stream responseContentStream = httpResponse.Content.ReadAsStream();
            T? deserializedResponseContent = JsonSerializer.Deserialize<T>(responseContentStream);
            if (deserializedResponseContent == null)
                throw new Exception($"Could not deserialize response content to {typeof(T)}.");

            return deserializedResponseContent;
        }
    }
}
