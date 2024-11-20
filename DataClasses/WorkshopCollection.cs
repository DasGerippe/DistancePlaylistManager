namespace DataClasses
{
    public class WorkshopCollection
    {
        public required string Name { get; init; }

        public required IReadOnlyCollection<WorkshopLevel> Levels { get; init; }
    }
}
