namespace DataClasses
{
    public abstract class Level
    {
        public required string Name { get; init; }

        public required string FileName { get; init; }

        public required IReadOnlyCollection<GameMode> GameModes { get; init; }

        public abstract string GetLevelPath();
    }
}
