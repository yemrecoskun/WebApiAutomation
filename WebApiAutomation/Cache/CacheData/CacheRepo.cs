using System;
using System.Collections.Generic;
namespace WebApiAutomation.Cache.CacheData
{
    public class CacheRepo
    {
        public static Dictionary<string, Dictionary<string, Dictionary<Guid, string>>> cacheList { get; set; } = new Dictionary<string, Dictionary<string, Dictionary<Guid, string>>>();
    }
}
