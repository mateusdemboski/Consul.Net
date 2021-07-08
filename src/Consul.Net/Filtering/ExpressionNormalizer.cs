namespace Consul.Net.Filtering
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    internal sealed class ExpressionNormalizer : ExpressionVisitor
    {
        public static Expression Normalize(Expression expression)
        {
            _ = expression ?? throw new ArgumentNullException(nameof(expression));

            var parser = new ExpressionNormalizer();

            return parser.Visit(expression);
        }

        /// <inheritdoc/>
        protected override Expression VisitLambda<T>(Expression<T> node)
        {
            return node.Body;
        }

        /// <inheritdoc/>
        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (node.Method.DeclaringType == typeof(Queryable))
            {
                return node.Method.Name switch
                {
                    "Where" => this.Visit(node.Arguments[1]),
                    _ when node.Arguments.Count == 1 => this.Visit(node.Arguments.Single()),
                    _ => throw new NotSupportedException(),
                };
            }

            return base.VisitMethodCall(node);
        }

        /// <inheritdoc/>
        protected override Expression VisitUnary(UnaryExpression node) => node.NodeType switch
        {
            ExpressionType.Quote => this.Visit(node.Operand),
            _ => base.VisitUnary(node),
        };
    }
}
