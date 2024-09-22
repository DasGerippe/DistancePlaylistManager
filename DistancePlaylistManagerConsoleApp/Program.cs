using DataClasses;
using DistanceFileManagement;
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
            Playlist playlist = await (isUrl
                ? distanceWorkshopClient.CreatePlaylistFromCollection(collectionUrl!, gameMode)
                : distanceWorkshopClient.CreatePlaylistFromCollection(collectionUrlOrId, gameMode));

            if (!string.IsNullOrEmpty(playlistName))
                playlist.Name = playlistName;

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
