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

        public Playlist GetOrCreatePlaylist(string playlistName)
        {
            Playlist? playlist = _PlaylistRepository.GetAllPlaylists().FirstOrDefault(playlist => playlist.Name == playlistName);
            if (playlist == null)
            {
                playlist = new Playlist { Name = playlistName };
                _PlaylistRepository.Add(playlist);
            }
            return playlist;
        }
    }
}
