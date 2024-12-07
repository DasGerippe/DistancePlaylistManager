using DataClasses;

namespace DataStoring
{
    public interface IPlaylistRepository
    {
        void Add(Playlist playlist);

        void Update(Playlist playlist);

        void Delete(Playlist playlist);

        IEnumerable<Playlist> GetAllPlaylists();
    }
}
