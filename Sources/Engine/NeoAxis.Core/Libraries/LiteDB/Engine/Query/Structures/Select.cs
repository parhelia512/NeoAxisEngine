﻿#if !NO_LITE_DB
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Internal.LiteDB.Constants;

namespace Internal.LiteDB.Engine
{
    /// <summary>
    /// Represent a Select expression
    /// </summary>
    internal class Select
    {
        public BsonExpression Expression { get; }

        public bool All { get; }

        public Select(BsonExpression expression, bool all)
        {
            this.Expression = expression;
            this.All = all;
        }
    }
}

#endif