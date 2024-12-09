using DataStoring;
using DistanceWorkshop;
using System.CommandLine;
using PlaylistManagement;

namespace DistancePlaylistManagerConsoleApp
{
    internal class Program
    {
        private static readonly HttpClient HttpClient = new HttpClient();

        static async Task<int> Main(string[] args)
        {
            IPathsProvider pathsProvider = new DefaultPathsProvider();
            IPlaylistRepository playlistRepository = new PlaylistFileRepository(pathsProvider);
            PlaylistManager playlistManager = new PlaylistManager(playlistRepository);
            DistanceWorkshopClient distanceWorkshopClient = new DistanceWorkshopClient(HttpClient);

            CommandFactory commandFactory = new CommandFactory(playlistRepository, distanceWorkshopClient, playlistManager);
            RootCommand rootCommand = commandFactory.CreateCommands();

            if (args.Length > 0)
            {
                return await rootCommand.InvokeAsync(args).ConfigureAwait(false);
            }

            await rootCommand.InvokeAsync("--help").ConfigureAwait(false);
            Console.WriteLine("Enter exit/quit to leave the interactive command mode.");

            while (true)
            {
                string command = Console.ReadLine()!;
                if (command.Equals("exit", StringComparison.CurrentCultureIgnoreCase) ||
                    command.Equals("quit", StringComparison.CurrentCultureIgnoreCase))
                    break;

                try
                {
                    await rootCommand.InvokeAsync(command).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }

            return 0;
        }
    }
}
