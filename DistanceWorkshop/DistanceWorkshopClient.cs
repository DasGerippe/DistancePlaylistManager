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

        public async Task<WorkshopCollection> GetWorkshopCollection(Uri collectionUrl)
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
            return await GetWorkshopCollection(collectionId).ConfigureAwait(false);
        }

        public async Task<WorkshopCollection> GetWorkshopCollection(string collectionId)
        {
            ArgumentException.ThrowIfNullOrEmpty(collectionId);

            SteamRemoteStorage steamRemoteStorage = _SteamWebApiClient.Get<SteamRemoteStorage>()!;

            PublishedFileDetail collectionFileDetail = await steamRemoteStorage.GetPublishedFileDetail(collectionId).ConfigureAwait(false);
            VerifyCollectionFileDetail(collectionFileDetail);

            CollectionDetail collectionDetail = await steamRemoteStorage.GetCollectionDetail(collectionId).ConfigureAwait(false);
            IEnumerable<string> collectionFileIds = collectionDetail.Files.Select(file => file.FileId);
            PublishedFileDetail[] fileDetails = await steamRemoteStorage.GetPublishedFileDetails(collectionFileIds).ConfigureAwait(false);
            List<WorkshopLevel> workshopLevels = GetWorkshopLevelsFromFileDetails(fileDetails);

            WorkshopCollection collection = new WorkshopCollection()
            {
                Name = collectionFileDetail.Title,
                Levels = workshopLevels.AsReadOnly(),
            };

            return collection;
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

        private List<WorkshopLevel> GetWorkshopLevelsFromFileDetails(PublishedFileDetail[] publishedFileDetails)
        {
            List<WorkshopLevel> levels = publishedFileDetails
                .Where(file => file is
                {
                    ResultCode: 1,
                    CreatorAppId: DistanceAppId,
                    ConsumerAppId: DistanceAppId,
                })
                .Select(file => new WorkshopLevel
                {
                    Name = file.Title,
                    FileName = file.FileName,
                    CreatorId = file.CreatorId,
                    GameModes = GetGameModesFromTags(file.Tags),
                })
                .ToList();

            return levels;
        }

        private IReadOnlyCollection<GameMode> GetGameModesFromTags(IEnumerable<Tag> tags)
        {
            List<GameMode> gameModes = new List<GameMode>();

            IEnumerable<string> distinctTags = tags.Select(tag => tag.Name).Distinct();
            foreach (string tag in distinctTags)
            {
                if (Enum.TryParse(tag, out GameMode gameMode))
                    gameModes.Add(gameMode);
            }

            return gameModes.AsReadOnly();
        }
    }
}
