namespace Consul.Net.Filtering
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Expression = System.Linq.Expressions.Expression;

    /// <inheritdoc/>
    public class FilterQueryProvider<TService> : IQueryProvider
    {
        private readonly Func<string, IEnumerable<TService>> filterExecuter;

        /// <summary>
        /// Initializes a new instance of the <see cref="FilterQueryProvider{TService}"/> class.
        /// </summary>
        /// <param name="filterExecuter">Factory that turns a in string filter into an <see cref="IEnumerable{T}"/> of <typeparamref name="TService"/>.</param>
        public FilterQueryProvider(Func<string, IEnumerable<TService>> filterExecuter)
        {
            this.filterExecuter = filterExecuter;
        }

        /// <inheritdoc/>
        public IQueryable CreateQuery(Expression expression)
            => this.CreateQuery<TService>(expression);

        /// <inheritdoc/>
        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
            => (IQueryable<TElement>)new FilterQueryable<TService>(this)
            {
                Expression = ExpressionNormalizer.Normalize(expression),
            };

        /// <inheritdoc/>
        public object Execute(Expression expression)
        {
            expression = ExpressionNormalizer.Normalize(expression);

            return this.filterExecuter(ExpressionParser.Parse(expression));
        }

        /// <inheritdoc/>
        public TResult Execute<TResult>(Expression expression)
        {
            var enumerationMap = new Dictionary<string, Func<IEnumerable<TResult>, TResult>>
            {
                { "First", x => x.First() },
                { "FirstOrDefault", x => x.FirstOrDefault() },
                { "Single", x => x.Single() },
                { "SingleOrDefault", x => x.SingleOrDefault() },
            };

            if (expression is not MethodCallExpression methodCallExpression ||
                !enumerationMap.TryGetValue(methodCallExpression.Method.Name, out var enumerableResolver))
            {
                throw new NotSupportedException();
            }

            var results = (IEnumerable<TResult>)this.Execute(methodCallExpression.Arguments.Single());

            return enumerableResolver(results);
        }
    }
}
