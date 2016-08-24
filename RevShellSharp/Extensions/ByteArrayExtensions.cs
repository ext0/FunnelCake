using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FunnelCake.Extensions
{
    public static class ByteArrayExtensions
    {
        public static unsafe long ContainsSequence(this byte[] data, byte[] sequence, long index)
        {
            fixed (byte* h = data) fixed (byte* n = sequence)
            {
                for (byte* hNext = h + index, hEnd = h + data.LongLength + 1 - sequence.LongLength, nEnd = n + sequence.LongLength; hNext < hEnd; hNext++)
                    for (byte* hInc = hNext, nInc = n; *nInc == *hInc; hInc++)
                        if (++nInc == nEnd)
                            return hNext - h;
                return -1;
            }
        }
    }
}
