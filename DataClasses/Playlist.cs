namespace DataClasses
{
    public class Playlist
    {
        public required string Name { get; init; }

        public GameMode? GameMode => Levels.FirstOrDefault()?.GameMode;

        public IList<PlaylistLevel> Levels { get; init; } = [];
    }
}
