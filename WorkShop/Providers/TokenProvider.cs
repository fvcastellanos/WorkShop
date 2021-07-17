using Microsoft.Extensions.Caching.Memory;

namespace WorkShop.Providers
{
    public class TokenProvider
    {
        private readonly IMemoryCache _memoryCache;

        public TokenProvider(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public void StoreToken(string user, string value) {

            var options = new MemoryCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = System.TimeSpan.FromDays(1)
            };

            _memoryCache.Set(user + "-token", value, options);
        }

        public string GetToken(string user)
        {
            return _memoryCache.Get<string>(user + "-token");
        }

        public void RemoveToken(string user)
        {
            _memoryCache.Remove(user + "-token");
        }
    }
}