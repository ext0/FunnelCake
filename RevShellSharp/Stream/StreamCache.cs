using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.IO;

namespace FunnelCake.Stream
{
    public static class StreamCache
    {
        private static ObjectCache _cache = MemoryCache.Default;
        private static TimeSpan _expiration = TimeSpan.FromSeconds(3600);
        
        public static StreamBlock GetCache(byte[] checksum)
        {
            string key = BitConverter.ToString(checksum).Replace("-", "");
            object data = _cache[key];
            if (data != null && data is StreamBlock)
            {
                return data as StreamBlock;
            } else
            {
                return null;
            }
        }

        public static bool Cache(StreamBlock block)
        {
            if (!block.Header.HasFlag(StreamHeader.CACHEABLE))
            {
                throw new InvalidOperationException("Cannot cache block " + block + ", is not marked as CACHEABLE!");
            }
            CacheItem cacheItem = new CacheItem(block.ToString(), block);
            CacheItemPolicy policy = new CacheItemPolicy();
            policy.AbsoluteExpiration = DateTimeOffset.Now.Add(_expiration);
            CacheItem existing = _cache.AddOrGetExisting(cacheItem, policy);
            if (existing != null)
            {
                return false;
            }
            return true;
        }
    }
}
