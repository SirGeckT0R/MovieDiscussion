using System.Linq.Expressions;

namespace DiscussionServiceDataAccess.Specifications
{
    public abstract class Specification<T>(Expression<Func<T, bool>>? criteria) where T : class
    {
        public Expression<Func<T, bool>>? Criteria { get; } = criteria;
        public List<Expression<Func<T, object>>> IncludeExpressions { get; } = [];
        public Expression<Func<T, object>>? OrderByExpression { get; private set; }

        protected void AddInclude(Expression<Func<T, object>> includeExpression)
        {
            IncludeExpressions.Add(includeExpression);
        }

        protected void AddOrderBy(Expression<Func<T, object>> orderByExpression)
        {
            OrderByExpression = orderByExpression;
        }
    }
}
