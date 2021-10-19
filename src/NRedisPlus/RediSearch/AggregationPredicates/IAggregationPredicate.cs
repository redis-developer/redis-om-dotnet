﻿using System.Collections.Generic;

namespace NRedisPlus.RediSearch.AggregationPredicates
{
    /// <summary>
    /// A predicate in an aggregation pipeline.
    /// </summary>
    public interface IAggregationPredicate
    {
        /// <summary>
        /// Serializes the predicate.
        /// </summary>
        /// <returns>An array of string arguments for an aggregation.</returns>
        IEnumerable<string> Serialize();
    }
}
