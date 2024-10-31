namespace DataClasses
{
    public class Playlist
    {
        public string Name { get; set; } = string.Empty;

        public GameMode? GameMode => Levels.FirstOrDefault()?.GameMode;

        public IList<PlaylistLevel> Levels { get; init; } = [];
    }
}
