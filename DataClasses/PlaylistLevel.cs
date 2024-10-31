namespace DataClasses
{
    public sealed class PlaylistLevel : IEquatable<PlaylistLevel>
    {
        public required GameMode GameMode { get; init; }

        public required string LevelName { get; init; }

        public required string LevelPath { get; init; }

        public bool Equals(PlaylistLevel? other)
        {
            if (other is null)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            return GameMode == other.GameMode
                && string.Equals(LevelName, other.LevelName, StringComparison.OrdinalIgnoreCase)
                && string.Equals(LevelPath, other.LevelPath, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object? obj)
        {
            return ReferenceEquals(this, obj) || obj is PlaylistLevel other && Equals(other);
        }

        public override int GetHashCode()
        {
            HashCode hashCode = new HashCode();
            hashCode.Add((int)GameMode);
            hashCode.Add(LevelName, StringComparer.OrdinalIgnoreCase);
            hashCode.Add(LevelPath, StringComparer.OrdinalIgnoreCase);
            return hashCode.ToHashCode();
        }
    }
}
