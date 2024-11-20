namespace DataClasses
{
    public sealed class WorkshopLevel : Level
    {
        public required string CreatorId { get; init; }

        public override string GetLevelPath()
        {
            return $"WorkshopLevels/{CreatorId}/{FileName}";
        }
    }
}
