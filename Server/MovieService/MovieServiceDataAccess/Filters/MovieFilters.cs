namespace MovieServiceDataAccess.Filters
{
    public record MovieFilters(string? Name, ICollection<Guid>? Genres, Guid? CrewMember);
}
