using DataStoring;
using DistancePlaylistManagerConsoleApp.Commands;
using System.CommandLine;

namespace DistancePlaylistManagerConsoleApp
{
    internal class Program
    {
        static async Task<int> Main(string[] args)
        {
            IPathsProvider pathsProvider = new DefaultPathsProvider();
            IPlaylistRepository playlistRepository = new PlaylistFileRepository(pathsProvider);

            Command commandTree = new DistancePlaylistManagerRootCommand(playlistRepository);

            if (args.Length > 0)
            {
                return await commandTree.InvokeAsync(args).ConfigureAwait(false);
            }

            await commandTree.InvokeAsync("--help").ConfigureAwait(false);
            Console.WriteLine("Enter exit/quit to leave the interactive command mode.");

            while (true)
            {
                Console.WriteLine();
                string command = Console.ReadLine()!;
                if (command.Equals("exit", StringComparison.CurrentCultureIgnoreCase) ||
                    command.Equals("quit", StringComparison.CurrentCultureIgnoreCase))
                    break;

                try
                {
                    await commandTree.InvokeAsync(command).ConfigureAwait(false);
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
