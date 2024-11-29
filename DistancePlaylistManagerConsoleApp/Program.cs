using DataClasses;
using DataStoring;
using DistanceWorkshop;

namespace DistancePlaylistManagerConsoleApp
{
    internal class Program
    {
        private static readonly HttpClient HttpClient = new HttpClient();

        static async Task Main(string[] args)
        {
            Console.WriteLine("==== Distance Playlist Manager ====");
            Console.WriteLine("Currently, the tool can only be used to create a playlist from a collection. ");
            Console.WriteLine();

            Console.WriteLine("Enter a URL or ID of a Distance workshop collection:");
            string collectionUrlOrId = Console.ReadLine()!;
            bool isUrl = Uri.TryCreate(collectionUrlOrId, UriKind.Absolute, out Uri? collectionUrl);

            GameMode gameMode = ReadGameMode();

            Console.WriteLine("Enter a name for the playlist (leave empty to use collection name):");
            string playlistName = Console.ReadLine()!;

            DistanceWorkshopClient distanceWorkshopClient = new DistanceWorkshopClient(HttpClient);
            WorkshopCollection collection = await (isUrl
                ? distanceWorkshopClient.GetWorkshopCollection(collectionUrl!)
                : distanceWorkshopClient.GetWorkshopCollection(collectionUrlOrId));

            Playlist playlist = new Playlist
            {
                Name = string.IsNullOrEmpty(playlistName) ? $"{collection.Name} ({gameMode})" : playlistName,
                Levels = collection.Levels
                    .Select(workshopLevel => new PlaylistLevel
                    {
                        GameMode = gameMode,
                        LevelName = workshopLevel.Name,
                        LevelPath = workshopLevel.GetLevelPath(),
                    })
                    .ToList(),
            };

            PlaylistSerializer serializer = new PlaylistSerializer();
            DefaultPathsProvider pathsProvider = new DefaultPathsProvider();
            string playlistFilePath = pathsProvider.GetPlaylistFilePath(playlist.Name);
            await using FileStream fileStream = File.Open(playlistFilePath, FileMode.Create, FileAccess.Write, FileShare.Read);
            serializer.Serialize(fileStream, playlist);
        }

        private static GameMode ReadGameMode()
        {
            string possibleGameModes = string.Join(", ", Enum.GetNames<GameMode>());
            Console.WriteLine($"Enter the game mode for the playlist ({possibleGameModes}):");
            while (true)
            {
                string? input = Console.ReadLine();
                bool isGameModeString = Enum.TryParse(input, out GameMode gameMode);

                if (isGameModeString)
                    return gameMode;

                Console.WriteLine("Invalid input! Try again:");
            }
        }
    }
}
