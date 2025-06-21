using Castle.Components.DictionaryAdapter.Xml;
using Microsoft.AspNetCore.Mvc;

namespace TodoWeb.Application.Services.CacheService
{
    public class CacheService : ICacheService
    {
        private readonly Dictionary<string, CacheData> _cache = new();
        public CacheData Get(string key)
        {
            ////tryGetValue: Nếu có sẽ trả về true đồng thời trả về giá trị Value tương ứng qua biến Value.
            ////Ngược lại trả về false. 
            //if (_cache.TryGetValue(key, out var cacheData))
            //{
            //    if (cacheData.Expiration > DateTime.UtcNow)
            //    {
            //        return cacheData;
            //    }
            //    else
            //    {
            //        _cache.Remove(key);
            //    }
            //}
            ////không chứa key sẽ retuen null
            //return null;

            //Cách trên ko clean nên dùng early return để clean cache
            if (!_cache.ContainsKey(key))
            {
                return null;
            }
            var cacheData = _cache[key];
            if (cacheData.Expiration < DateTime.UtcNow)
            {
                _cache.Remove(key);
                return null;
            }
            return cacheData;
        }
        public void Set(string key, object value, int duration)
        {
            var expirationTime = DateTime.UtcNow.AddSeconds(duration);
            var cacheData = new CacheData(value, expirationTime);
            _cache[key] = cacheData;
        }
        public void Remove(string key)
        {
            _cache.Remove(key);
        }
    }
    
}
