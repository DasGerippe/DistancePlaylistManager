using DataClasses;
using DataStoring;
using PlaylistManagement;
using System.CommandLine;
using System.CommandLine.Binding;

namespace DistancePlaylistManagerConsoleApp.Commands.PlaylistCommands
{
    internal class ShufflePlaylistCommand : Command
    {
        private readonly IPlaylistRepository _PlaylistRepository;

        internal ShufflePlaylistCommand(IPlaylistRepository playlistRepository) : base(
            name: "shuffle",
            description: "Shuffle the levels in a playlist.")
        {
            _PlaylistRepository = playlistRepository;

            Argument playlistNameArgument = new Argument<string>(
                name: "playlist",
                description: "The name of the playlist to be shuffled.");

            AddArgument(playlistNameArgument);

            this.SetHandler((playlistName) =>
                {
                    ShufflePlaylist(playlistName);
                },
                (IValueDescriptor<string>)playlistNameArgument);
        }

        private void ShufflePlaylist(string playlistName)
        {
            PlaylistManager playlistManager = new PlaylistManager(_PlaylistRepository);
            Playlist? playlist = playlistManager.GetPlaylist(playlistName);
            if (playlist == null)
            {
                Console.WriteLine($"Could not find playlist '{playlistName}'.");
                return;
            }

            PlaylistShuffler shuffler = new PlaylistShuffler();
            shuffler.ShufflePlaylist(playlist);

            _PlaylistRepository.Update(playlist);
        }
    }
}
