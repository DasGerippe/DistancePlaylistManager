using DataClasses;
using SteamWebApi;
using SteamWebApi.ApiInterfaces;
using SteamWebApi.DataClasses.GetCollectionDetails;
using SteamWebApi.DataClasses.GetPublishedFileDetails;
using System.Text.RegularExpressions;

namespace DistanceWorkshop
{
    public class DistanceWorkshopClient
    {
        private const int SteamWorkshopAppId = 766;
        private const int DistanceAppId = 233610;

        private readonly SteamWebApiClient _SteamWebApiClient;

        public DistanceWorkshopClient(HttpClient httpClient)
        {
            _SteamWebApiClient = new SteamWebApiClient(httpClient);
        }

        public Task<Playlist> CreatePlaylistFromCollection(Uri collectionUrl, GameMode gameMode)
        {
            bool isSteamWorkshopUrl =
                collectionUrl.Host.Equals("steamcommunity.com", StringComparison.OrdinalIgnoreCase) &&
                collectionUrl.AbsolutePath.Equals("/sharedfiles/filedetails/", StringComparison.OrdinalIgnoreCase);

            if (!isSteamWorkshopUrl)
                throw new ArgumentException("The URL is not a Steam workshop URL.", nameof(collectionUrl));

            Match idMatch = Regex.Match(collectionUrl.Query, @"id=(?<Id>\d+)");
            if (!idMatch.Success)
                throw new ArgumentException("The URL does not contain an id parameter!", nameof(collectionUrl));

            string collectionId = idMatch.Groups["Id"].Value;
            return CreatePlaylistFromCollection(collectionId, gameMode);
        }

        public async Task<Playlist> CreatePlaylistFromCollection(string collectionId, GameMode gameMode)
        {
            ArgumentException.ThrowIfNullOrEmpty(collectionId);

            SteamRemoteStorage steamRemoteStorage = _SteamWebApiClient.Get<SteamRemoteStorage>()!;

            PublishedFileDetail collectionFileDetail = await steamRemoteStorage.GetPublishedFileDetail(collectionId);
            VerifyCollectionFileDetail(collectionFileDetail);

            CollectionDetail collectionDetail = await steamRemoteStorage.GetCollectionDetail(collectionId);
            IEnumerable<string> collectionFileIds = collectionDetail.Files.Select(file => file.FileId);
            PublishedFileDetail[] fileDetails = await steamRemoteStorage.GetPublishedFileDetails(collectionFileIds);
            List<Level> levels = GetLevelsFromFileDetails(fileDetails, gameMode);

            Playlist playlist = new Playlist
            {
                Name = $"{collectionFileDetail.Title} ({gameMode})",
                GameMode = gameMode,
                Levels = levels,
            };

            return playlist;
        }

        private void VerifyCollectionFileDetail(PublishedFileDetail collectionFileDetail)
        {
            if (collectionFileDetail.ResultCode != 1)
                throw new Exception("Collection is not publicly visible or does not exist.");

            if (collectionFileDetail.CreatorAppId != SteamWorkshopAppId)
                throw new Exception("Collection wasn't created with the Steam Workshop.");

            if (collectionFileDetail.ConsumerAppId != DistanceAppId)
                throw new Exception("Collection wasn't created for Distance.");
        }

        private List<Level> GetLevelsFromFileDetails(PublishedFileDetail[] publishedFileDetails, GameMode gameMode)
        {
            string serializedGameMode = gameMode.ToString();
            List<Level> levels = publishedFileDetails
                .Where(file =>
                    file.ResultCode == 1 &&
                    file.CreatorAppId == DistanceAppId &&
                    file.ConsumerAppId == DistanceAppId &&
                    file.Tags.Any(tag => tag.Name == serializedGameMode))
                .Select(file => new Level()
                {
                    Id = file.FileId,
                    Name = file.Title,
                    FileName = file.FileName,
                    CreatorId = file.CreatorId,
                    Source = LevelSource.Workshop,
                })
                .ToList();

            return levels;
        }
    }
}
