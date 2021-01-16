using System;
using System.Collections.Generic;
using WebApiAutomation.Cache.CacheData;

namespace WebApiAutomation.Cache.CacheManager
{
    public class CacheManager
    {
        public static void Clear(string cacheKey)
        {
            CacheRepo.cacheList[cacheKey] = new Dictionary<string, Dictionary<Guid, string>>();
            CacheRepo.cacheList[cacheKey].Add("request", new Dictionary<Guid, string>());
            CacheRepo.cacheList[cacheKey].Add("response", new Dictionary<Guid, string>());
        }
        public static void Insert(string cacheKey, string key, Guid Id, string value)
        {
            CacheRepo.cacheList[cacheKey][key].Add(Id, value);
        }
        public static string ReadList(string cacheListKey, string keyId, Guid Id)
        {
            return CacheRepo.cacheList[keyId][cacheListKey][Id];
        }
    }
}
