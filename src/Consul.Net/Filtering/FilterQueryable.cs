namespace Consul.Net.Filtering
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    /// <inheritdoc/>
    public class FilterQueryable<TService> : IQueryable<TService>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="FilterQueryable{TService}"/> class.
        /// </summary>
        /// <param name="provider">The <see cref="IQueryProvider"/> implementaion.</param>
        public FilterQueryable(IQueryProvider provider)
        {
            this.Expression = Expression.Constant(this);
            this.Provider = provider;
        }

        /// <inheritdoc/>
        public Type ElementType => typeof(TService);

        /// <inheritdoc/>
        public Expression Expression { get; set; }

        /// <inheritdoc/>
        public IQueryProvider Provider { get; }

        /// <inheritdoc/>
        public IEnumerator<TService> GetEnumerator()
            => ((IEnumerable<TService>)this.Provider.Execute(this.Expression)).GetEnumerator();

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
            => this.GetEnumerator();
    }
}
