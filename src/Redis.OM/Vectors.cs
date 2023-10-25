using System;

namespace Redis.OM
{
    /// <summary>
    /// Container class for Vector extensions.
    /// </summary>
    public static class VectorExtensions
    {
        /// <summary>
        /// Placeholder method to allow you to perform vector range operations. Only meant to be run
        /// in context of a query.
        /// </summary>
        /// <param name="obj">The vector field.</param>
        /// <param name="comparisonObject">The comparison object.</param>
        /// <param name="range">The allowable distance from the provided object.</param>
        /// <typeparam name="T">The type to compare.</typeparam>
        /// <returns>Whether the queried vector is within the allowable distance.</returns>
        public static bool VectorRange<T>(this T obj, object comparisonObject, double range) => throw new NotImplementedException("This method is only meant to be run within a query of Redis.");

        /// <summary>
        /// Placeholder method to allow you to perform vector range operations. Only meant to be run
        /// in context of a query.
        /// </summary>
        /// <param name="obj">The vector field.</param>
        /// <param name="comparisonObject">The comparison object.</param>
        /// <param name="range">The allowable distance from the provided object.</param>
        /// <param name="scoreName">The name of the score in the output.</param>
        /// <typeparam name="T">The type to compare.</typeparam>
        /// <returns>Whether the queried vector is within the allowable distance.</returns>
        public static bool VectorRange<T>(this T obj, object comparisonObject, double range, string scoreName) => throw new NotImplementedException("This method is only meant to be run within a query of Redis.");
    }
}