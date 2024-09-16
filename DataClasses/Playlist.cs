namespace DataClasses
{
    public class Playlist
    {
        public string Name { get; set; } = string.Empty;

        public GameMode GameMode { get; set; }

        public IList<Level> Levels { get; init; } = [];
    }
}
