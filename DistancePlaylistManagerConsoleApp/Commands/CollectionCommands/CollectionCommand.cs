﻿using DataClasses;
using DataStoring;
using DistanceWorkshop;
using PlaylistManagement;
using System.CommandLine;
using System.CommandLine.Binding;

namespace DistancePlaylistManagerConsoleApp.Commands.CollectionCommands
{
    internal class CollectionCommand : Command
    {
        private readonly IPlaylistRepository _PlaylistRepository;

        internal CollectionCommand(IPlaylistRepository playlistRepository) : base(
            name: "collection",
            description: "Retrieves the levels from a workshop collection and adds them to a new or existing playlist.")
        {
            _PlaylistRepository = playlistRepository;

            Argument collectionArgument = new Argument<string>(
                name: "collection",
                description: "The url or steam id of the workshop collection.");

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

            AddArgument(collectionArgument);
            AddOption(playlistNameOption);
            AddOption(gameModeOption);
            AddOption(keepDuplicatesOption);

            this.SetHandler(async (collection, playlist, gameMode, keepDuplicates) =>
                {
                    await AddCollectionLevelsToPlaylist(collection, playlist, gameMode, keepDuplicates).ConfigureAwait(false);
                },
                (IValueDescriptor<string>)collectionArgument,
                playlistNameOption,
                gameModeOption,
                keepDuplicatesOption);
        }

        private async Task AddCollectionLevelsToPlaylist(string collectionUrlOrId, string playlistName, GameMode gameMode, bool keepDuplicates)
        {
            try
            {
                WorkshopCollection collection = await GetWorkshopCollection(collectionUrlOrId).ConfigureAwait(false);

                if (string.IsNullOrEmpty(playlistName))
                    playlistName = GeneratePlaylistName(collection.Name, gameMode);

                PlaylistManager playlistManager = new PlaylistManager(_PlaylistRepository);
                Playlist playlist = playlistManager.GetOrCreatePlaylist(playlistName);

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
            DistanceWorkshopClient distanceWorkshopClient = new DistanceWorkshopClient(HttpClientProvider.HttpClient);
            bool isUrl = Uri.TryCreate(collectionUrlOrId, UriKind.Absolute, out Uri? collectionUrl);

            Task<WorkshopCollection> getWorkshopCollectionTask = isUrl
                ? distanceWorkshopClient.GetWorkshopCollection(collectionUrl!)
                : distanceWorkshopClient.GetWorkshopCollection(collectionUrlOrId);
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