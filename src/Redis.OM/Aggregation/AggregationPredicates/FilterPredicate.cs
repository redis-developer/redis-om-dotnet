﻿using System.Collections.Generic;
using System.Linq.Expressions;
using Redis.OM.Common;

namespace Redis.OM.Aggregation.AggregationPredicates
{
    /// <summary>
    /// predicate for filtering results from an aggregation.
    /// </summary>
    public class FilterPredicate : Apply, IAggregationPredicate
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FilterPredicate"/> class.
        /// </summary>
        /// <param name="expression">The expression to use for filtering.</param>
        public FilterPredicate(Expression expression)
            : base(expression, string.Empty)
        {
        }

        /// <inheritdoc/>
        public new IEnumerable<string> Serialize()
        {
            var list = new List<string> { "FILTER" };
            switch (Expression)
            {
                case BinaryExpression rootBinExpression:
                    list.Add(ExpressionParserUtilities.ParseBinaryExpression(rootBinExpression, true));
                    break;
                case MethodCallExpression method:
                    list.Add(ExpressionParserUtilities.GetOperandString(method));
                    break;
                default:
                    list.Add(ExpressionParserUtilities.GetOperandString(Expression));
                    break;
            }

            return list.ToArray();
        }
    }
}
