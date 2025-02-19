﻿#if !NO_LITE_DB
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Internal.LiteDB.Constants;

namespace Internal.LiteDB.Engine
{
    internal class IndexInfo
    {
        public string Collection { get; set; }
        public string Name { get; set; }
        public string Expression { get; set; }
        public bool Unique { get; set; }
    }
}

#endif