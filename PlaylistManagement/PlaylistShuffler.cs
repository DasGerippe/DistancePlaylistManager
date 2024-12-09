using DataClasses;

namespace PlaylistManagement
{
    public class PlaylistShuffler
    {
        public void ShufflePlaylist(Playlist playlist)
        {
            Random random = new Random();

            IList<PlaylistLevel> levels = playlist.Levels;

            for (int i = levels.Count - 1; i > 0; i--)
            {
                int swapIndex = random.Next(0, i + 1);
                
                PlaylistLevel temp = levels[swapIndex];
                levels[swapIndex] = levels[i];
                levels[i] = temp;
            }
        }
    }
}
