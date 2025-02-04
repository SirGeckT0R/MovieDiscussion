using Microsoft.EntityFrameworkCore;

namespace DiscussionServiceDataAccess.Specifications
{
    public static class SpecificationEvaluator<T> where T : class
    {
        public static IQueryable<T> GetQuery(IQueryable<T> inputQueryable, Specification<T> specification)
        {
            var query = inputQueryable;
            if (specification.Criteria != null)
            {
                query = query.Where(specification.Criteria);
            }

            query = specification.IncludeExpressions.Aggregate(query, (current, includeExpression) => current.Include(includeExpression));

            if (specification.OrderByExpression is not null)
            {
                query = query.OrderBy(specification.OrderByExpression);
            }

            return query;
        }
    }
}
