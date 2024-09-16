namespace DataClasses
{
    public class Level
    {
        public string Id { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string FileName { get; set; } = string.Empty;

        public string CreatorId { get; set; } = string.Empty;

        public LevelSource Source { get; set; }
    }
}
