namespace MovieServiceDataAccess.LinqExtensions
{
    public static class LinqExtensions
    {
        public static IQueryable<TResult> TrySkip<TResult>(this IQueryable<TResult> source, int? count)
        {
            return !count.HasValue ? source : source.Skip(count.Value);
        }

        public static IQueryable<TResult> TryTake<TResult>(this IQueryable<TResult> source, int? count)
        {
            return !count.HasValue ? source : source.Take(count.Value);
        }
    }
}
