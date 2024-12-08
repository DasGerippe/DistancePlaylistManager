using DataClasses;
using DataStoring;
using DistanceWorkshop;
using PlaylistManagement;
using System.CommandLine;

namespace DistancePlaylistManagerConsoleApp
{
    internal sealed class CommandFactory
    {
        private readonly IPlaylistRepository _PlaylistRepository;
        private readonly DistanceWorkshopClient _DistanceWorkshopClient;
        private readonly PlaylistManager _PlaylistManager;

        internal CommandFactory(
            IPlaylistRepository playlistRepository,
            DistanceWorkshopClient distanceWorkshopClient,
            PlaylistManager playlistManager)
        {
            _PlaylistRepository = playlistRepository;
            _DistanceWorkshopClient = distanceWorkshopClient;
            _PlaylistManager = playlistManager;
        }

        internal RootCommand CreateCommands()
        {
            RootCommand rootCommand = new RootCommand(
                description: "A tool for the racing game Distance that offers advanced playlist management features.");

            Command collectionToPlaylistCommand = new Command(
                name: "collection-to-playlist",
                description: "Retrieves the levels from a workshop collection and adds them to a new or existing playlist.");

            Option<string> collectionOption = new Option<string>(
                name: "--collection",
                description: "The url or steam id of the workshop collection.",
                parseArgument: result =>
                {
                    if (result.Tokens.Count != 0)
                        return result.Tokens[0].Value;

                    result.ErrorMessage = "The '--collection' option must be set.";
                    return string.Empty;
                },
                isDefault: true);

            Option<string> playlistNameOption = new Option<string>(
                name: "--playlist",
                description: "The name of the playlist to which the levels are added. " +
                             "If omitted, the collection name and game mode are used to generate a name.",
                getDefaultValue: () => string.Empty);

            Option<GameMode> gameModeOption = new Option<GameMode>(
                name: "--gamemode",
                description: "Only levels that support the specified game mode will be added to the playlist.",
                getDefaultValue: () => GameMode.Sprint);

            Option<bool> keepDuplicatesOption = new Option<bool>(
                name: "--keep-duplicates",
                description: "Levels will be added to the playlist, even if they are already included.");

            collectionToPlaylistCommand.AddOption(collectionOption);
            collectionToPlaylistCommand.AddOption(playlistNameOption);
            collectionToPlaylistCommand.AddOption(gameModeOption);
            collectionToPlaylistCommand.AddOption(keepDuplicatesOption);

            collectionToPlaylistCommand.SetHandler(async (collection, playlist, gameMode, keepDuplicates) =>
                {
                    await AddCollectionLevelsToPlaylist(collection, playlist, gameMode, keepDuplicates).ConfigureAwait(false);
                },
                collectionOption,
                playlistNameOption,
                gameModeOption,
                keepDuplicatesOption);

            rootCommand.AddCommand(collectionToPlaylistCommand);

            return rootCommand;
        }

        private async Task AddCollectionLevelsToPlaylist(string collectionUrlOrId, string playlistName, GameMode gameMode, bool keepDuplicates)
        {
            try
            {
                WorkshopCollection collection = await GetWorkshopCollection(collectionUrlOrId).ConfigureAwait(false);

                if (string.IsNullOrEmpty(playlistName))
                    playlistName = GeneratePlaylistName(collection.Name, gameMode);

                Playlist playlist = _PlaylistManager.GetOrCreatePlaylist(playlistName);

                PlaylistLevelAdder levelAdder = new PlaylistLevelAdder();
                levelAdder.AddLevelsToPlaylist(playlist, collection.Levels, gameMode, keepDuplicates);

                _PlaylistRepository.Update(playlist);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private async Task<WorkshopCollection> GetWorkshopCollection(string collectionUrlOrId)
        {
            bool isUrl = Uri.TryCreate(collectionUrlOrId, UriKind.Absolute, out Uri? collectionUrl);
            Task<WorkshopCollection> getWorkshopCollectionTask = isUrl
                ? _DistanceWorkshopClient.GetWorkshopCollection(collectionUrl!)
                : _DistanceWorkshopClient.GetWorkshopCollection(collectionUrlOrId);
            return await getWorkshopCollectionTask.ConfigureAwait(false);
        }

        private string GeneratePlaylistName(string collectionName, GameMode? gameMode)
        {
            return gameMode == null
                ? collectionName
                : $"{collectionName} ({gameMode})";
        }
    }
}
