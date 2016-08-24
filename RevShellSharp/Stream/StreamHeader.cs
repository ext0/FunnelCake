using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FunnelCake.Stream
{
    public enum StreamHeader : byte
    {
        EMPTY = 0,
        GZIPPED = 1,
        STRUCTURED = 1 << 1,
        JSON = 1 << 2,
        SERIALIZED = 1 << 3,
        RAW = 1 << 4,
        PRIORITY = 1 << 5,
        CACHEABLE = 1 << 6,
        RESERVED = 1 << 7
    }
}
