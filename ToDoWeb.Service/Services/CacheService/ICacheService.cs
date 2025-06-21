namespace TodoWeb.Application.Services.CacheService
{
    public interface ICacheService
    {
        public CacheData Get(string key);
        public void Set(string key, object value, int duration);
        public void Remove(string key);
    }
}
