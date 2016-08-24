using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace FunnelCake.Stream
{
    public class StreamBlock
    {
        private static int HEADER_OFFSET = 0;
        private static int HEADER_LENGTH = 1;

        private static int STREAM_ID_OFFSET = 1;
        private static int STREAM_ID_LENGTH = 2;

        private static int CHECKSUM_OFFSET = 3;
        private static int CHECKSUM_LENGTH = 16;

        private static int DATA_OFFSET = 19;

        private short _streamId;
        private StreamHeader _header;

        public StreamHeader Header
        {
            get
            {
                return _header;
            }
        }

        private byte[] _checksum;
        private byte[] _data;

        private StreamBlock(short streamId, StreamHeader header, byte[] checksum, byte[] data)
        {
            _streamId = streamId;
            _header = header;
            _checksum = checksum;
            _data = data;
        }

        protected static StreamBlock BuildBlock(byte[] rawBlock)
        {
            StreamHeader header = (StreamHeader)rawBlock.Skip(HEADER_OFFSET).Take(HEADER_LENGTH).First();
            short streamId = BitConverter.ToInt16(rawBlock.Skip(STREAM_ID_OFFSET).Take(STREAM_ID_LENGTH).ToArray(), 0);
            byte[] checksum = rawBlock.Skip(CHECKSUM_OFFSET).Take(CHECKSUM_LENGTH).ToArray();
            if (header.HasFlag(StreamHeader.CACHEABLE))
            {
                StreamBlock block = StreamCache.GetCache(checksum);
                if (block != null)
                {
                    return block;
                }
            }
            MD5 md5 = MD5.Create();
            byte[] data = rawBlock.Skip(DATA_OFFSET).ToArray();
            byte[] calculatedChecksum = md5.ComputeHash(data);
            if (!calculatedChecksum.SequenceEqual(checksum))
            {
                throw new InvalidDataException("Checksums did not match when checking data in BuildBlock routine! Calculated: " + BitConverter.ToString(calculatedChecksum) + " | Stored: " + BitConverter.ToString(checksum));
            }
            if (header.HasFlag(StreamHeader.GZIPPED))
            {
                using (GZipStream stream = new GZipStream(new MemoryStream(data), CompressionMode.Decompress))
                {
                    byte[] buffer = new byte[256];
                    using (MemoryStream memory = new MemoryStream())
                    {
                        int count = 0;
                        do
                        {
                            count = stream.Read(buffer, 0, buffer.Length);
                            if (count > 0)
                            {
                                memory.Write(buffer, 0, count);
                            }
                        }
                        while (count > 0);
                        data = memory.ToArray();
                    }
                }
            }
            return new StreamBlock(streamId, header, checksum, data);
        }

        public override bool Equals(object obj)
        {
            if (obj is StreamBlock)
            {
                return (obj as StreamBlock)._checksum.SequenceEqual(_checksum);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return _checksum.GetHashCode();
        }

        public override string ToString()
        {
            return BitConverter.ToString(_checksum).Replace("-", "");
        }
    }
}
