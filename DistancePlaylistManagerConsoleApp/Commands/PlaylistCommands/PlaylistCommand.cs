using DataStoring;
using System.CommandLine;

namespace DistancePlaylistManagerConsoleApp.Commands.PlaylistCommands
{
    internal class PlaylistCommand : Command
    {
        internal PlaylistCommand(IPlaylistRepository playlistRepository) : base(
            name: "playlist",
            description: "Provides several sub commands for playlist management.")
        {
            AddCommand(new ShufflePlaylistCommand(playlistRepository));
        }
    }
}
