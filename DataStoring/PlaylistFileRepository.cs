using DataClasses;

namespace DataStoring
{
    public class PlaylistFileRepository : IPlaylistRepository
    {
        private readonly IReadOnlyCollection<char> _InvalidPlaylistNameChars = ['<', '>', '/', '\\'];

        private readonly PlaylistSerializer _Serializer = new PlaylistSerializer();
        private readonly PlaylistDeserializer _Deserializer = new PlaylistDeserializer();

        private readonly IPathsProvider _PathsProvider;

        private readonly Dictionary<Playlist,string> _PlaylistPathDictionary = [];

        public PlaylistFileRepository(IPathsProvider pathsProvider)
        {
            _PathsProvider = pathsProvider;
            LoadPlaylists();
        }

        public void LoadPlaylists()
        {
            _PlaylistPathDictionary.Clear();

            string playlistsFolderPath = _PathsProvider.GetPlaylistsFolderPath();
            IEnumerable<string> playlistFilePaths = Directory.EnumerateFiles(playlistsFolderPath, "*.xml");

            foreach (string playlistFilePath in playlistFilePaths)
            {
                try
                {
                    Playlist playlist = _Deserializer.Deserialize(playlistFilePath);
                    _PlaylistPathDictionary.Add(playlist, playlistFilePath);
                }
                catch
                {
                }
            }
        }

        public void Add(Playlist playlist)
        {
            bool playlistNameInvalid = playlist.Name.Any(playlistNameChar => _InvalidPlaylistNameChars.Contains(playlistNameChar));
            if (playlistNameInvalid)
                throw new ArgumentException("The name of the playlist is invalid.");

            bool isRepositoryPlaylist = _PlaylistPathDictionary.ContainsKey(playlist);
            if (isRepositoryPlaylist)
                throw new ArgumentException("The playlist already belongs to this repository.", nameof(playlist));

            if (_PlaylistPathDictionary.Any(playlistPath => playlistPath.Key.Name.Equals(playlist.Name, StringComparison.OrdinalIgnoreCase)))
                throw new ArgumentException("A playlist with the same name already exists.");

            string playlistFilePath = _PathsProvider.GetPlaylistFilePath(playlist.Name);

            Playlist? samePathPlaylist = _PlaylistPathDictionary
                .Where(playlistPath => playlistPath.Value == playlistFilePath)
                .Select(playlistPath => playlistPath.Key)
                .FirstOrDefault();

            if (samePathPlaylist != null)
                throw new ArgumentException($"The playlist would have the same file path as the playlist '{samePathPlaylist.Name}'.");

            _PlaylistPathDictionary.Add(playlist, playlistFilePath);
        }

        public void Update(Playlist playlist)
        {
            bool isRepositoryPlaylist = _PlaylistPathDictionary.TryGetValue(playlist, out string? playlistFilePath);
            if (!isRepositoryPlaylist)
                throw new ArgumentException("The playlist doesn't belong to this repository.", nameof(playlist));

            _Serializer.Serialize(playlistFilePath!, playlist);
        }

        public void Delete(Playlist playlist)
        {
            bool isRepositoryPlaylist = _PlaylistPathDictionary.TryGetValue(playlist, out string? playlistFilePath);
            if (!isRepositoryPlaylist)
                return;

            File.Delete(playlistFilePath!);
            _PlaylistPathDictionary.Remove(playlist);
        }

        public IEnumerable<Playlist> GetAllPlaylists()
        {
            return _PlaylistPathDictionary.Keys;
        }
    }
}
