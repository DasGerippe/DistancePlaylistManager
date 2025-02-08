using DataClasses;
using DataStoring;

namespace PlaylistManagement
{
    public sealed class PlaylistManager
    {
        private readonly IPlaylistRepository _PlaylistRepository;

        public PlaylistManager(IPlaylistRepository playlistRepository)
        {
            _PlaylistRepository = playlistRepository;
        }

        public Playlist? GetPlaylist(string playlistName)
        {
            return _PlaylistRepository.GetAllPlaylists().FirstOrDefault(playlist => playlist.Name.Equals(playlistName, StringComparison.OrdinalIgnoreCase));
        }

        public Playlist GetOrCreatePlaylist(string playlistName)
        {
            Playlist? playlist = GetPlaylist(playlistName);
            if (playlist == null)
            {
                playlist = new Playlist { Name = playlistName };
                _PlaylistRepository.Add(playlist);
            }
            return playlist;
        }
    }
}
