using DataStoring;
using DistancePlaylistManagerConsoleApp.Commands.CollectionCommands;
using System.CommandLine;

namespace DistancePlaylistManagerConsoleApp.Commands
{
    internal class DistancePlaylistManagerRootCommand : RootCommand
    {
        internal DistancePlaylistManagerRootCommand(IPlaylistRepository playlistRepository) : base(
            description: "A tool for the racing game Distance that offers advanced playlist management features.")
        {
            AddCommand(new CollectionCommand(playlistRepository));
        }
    }
}
