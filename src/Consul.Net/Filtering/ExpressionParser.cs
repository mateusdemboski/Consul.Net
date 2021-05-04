namespace Consul.Net.Filtering
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text;
    using static System.Linq.Expressions.ExpressionType;

    /// <summary>
    /// Parse a <see cref="Expression"/> into consul Filter expression
    /// using the <see cref="ExpressionVisitor"/>.
    /// </summary>
    internal sealed class ExpressionParser : ExpressionVisitor
    {
        private readonly StringBuilder builder;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpressionParser"/> class.
        /// </summary>
        public ExpressionParser()
        {
            this.builder = new StringBuilder();
        }

        /// <summary>
        /// Parse a <see cref="Expression"/> into consul Filter expression
        /// using the <see cref="ExpressionParser"/>.
        /// </summary>
        /// <param name="expression">The <see cref="Expression"/> to be parsed.</param>
        /// <returns>A string contanig the consul filter expression.</returns>
        public static string Parse(Expression expression)
        {
            var visitor = Activator.CreateInstance<ExpressionParser>();

            _ = visitor.Visit(expression);

            return visitor.ToString();
        }

        /// <inheritdoc/>
        public override string ToString()
            => this.builder.ToString();

        /// <inheritdoc/>
        protected override Expression VisitBinary(BinaryExpression node)
        {
            var parsedOperator = ParseOperator(node.NodeType, node.Left, node.Right);

            if (node.NodeType == OrElse)
            {
                _ = this.builder.Append('(');
                _ = this.Visit(node);
                _ = this.builder.Append('(');

                return node;
            }

            _ = this.Visit(node.Left);

            _ = this.builder.Append($" {parsedOperator} ");

            _ = this.Visit(node.Right);

            return node;
        }

        /// <inheritdoc/>
        protected override Expression VisitConstant(ConstantExpression node)
        {
            _ = this.builder.Append(node.Value ?? "empty");

            return node;
        }

        /// <inheritdoc/>
        protected override Expression VisitMember(MemberExpression node)
        {
            _ = this.builder.Append(node.Member.Name);

            return node;
        }

        /// <inheritdoc/>
        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (node.Method.Name == "get_Item")
            {
                _ = this.Visit(node.Object);

                _ = this.builder.Append('.');

                _ = this.Visit(node.Arguments.Single());
            }
            else if (node.Method.Name == "Contains" && node.Object is not null)
            {
                _ = this.Visit(node.Object);

                _ = this.builder.Append(" contains ");

                _ = this.builder.Append('"');
                _ = this.Visit(node.Arguments.Single());
                _ = this.builder.Append('"');
            }
            else if (node.Method.Name == "Contains" && node.Object is null && node.Arguments.Count == 2)
            {
                _ = this.builder.Append('"');
                _ = this.Visit(node.Arguments[1]);
                _ = this.builder.Append('"');

                _ = this.builder.Append(" in ");

                _ = this.Visit(node.Arguments[0]);
            }
            else if (node.Method.Name == "IsNullOrEmpty")
            {
                _ = this.Visit(
                    Expression.MakeBinary(
                        Equal,
                        node.Arguments.Single(),
                        Expression.Constant(null)));
            }

            return node;
        }

        /// <inheritdoc/>
        protected override Expression VisitUnary(UnaryExpression node)
        {
            if (node.NodeType == Not)
            {
                _ = this.builder.Append($"{ParseOperator(node.NodeType)} (");
                _ = this.Visit(node.Operand);
                _ = this.builder.Append(')');

                return node;
            }

            _ = this.builder.Append($" {ParseOperator(node.NodeType)} ");

            _ = this.Visit(node.Operand);

            return node;
        }

        private static string ParseOperator(
            ExpressionType expressionType,
            Expression? left = default,
            Expression? right = default) => expressionType switch
            {
                AndAlso => "and",
                Equal when
                    (left is ConstantExpression x && x.Value is null)
                    || (right is ConstantExpression y && y.Value is null) => "is",
                Equal => "==",
                NotEqual when
                    (left is ConstantExpression x && x.Value is null)
                    || (right is ConstantExpression y && y.Value is null) => "is not",
                NotEqual => "!=",
                OrElse => "or",
                Not => "not",
                _ => string.Empty,
            };
    }
}
