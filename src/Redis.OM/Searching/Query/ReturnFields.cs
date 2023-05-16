﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Redis.OM.Searching.Query
{
    /// <summary>
    /// Predicate denoting the fields that will be returned from redis.
    /// </summary>
    public class ReturnFields : QueryOption
    {
        /// <summary>
        /// The fields to bring back.
        /// </summary>
        private readonly IEnumerable<ReturnField> _fields;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReturnFields"/> class.
        /// </summary>
        /// <param name="fields">the fields to return.</param>
        public ReturnFields(IEnumerable<string> fields)
        {
            _fields = fields.Select(x => new ReturnField(x));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReturnFields"/> class.
        /// </summary>
        /// <param name="fields">the fields to return.</param>
        public ReturnFields(IEnumerable<ReturnField> fields)
        {
            _fields = fields;
        }

        /// <inheritdoc/>
        internal override IEnumerable<string> SerializeArgs
        {
            get
            {
                var ret = new List<string> { "RETURN", (_fields.Count() + (_fields.Count(x => x.Alias != null) * 2)).ToString() };
                foreach (var field in _fields)
                {
                    ret.Add($"{field.Name}");
                    if (field.Alias != null)
                    {
                        ret.Add("AS");
                        ret.Add(field.Alias);
                    }
                }

                return ret.ToArray();
            }
        }
    }
}
