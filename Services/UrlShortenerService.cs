using StackExchange.Redis;

namespace UrlShortener.Services
{
    public class UrlShortenerService
    {
        private readonly IDatabase _redisDb;

        public UrlShortenerService(IConnectionMultiplexer redis)
        {
            _redisDb = redis.GetDatabase();
        }

        public async Task<string> ShortenUrl(string originalUrl)
        {
            var shortCode = Guid.NewGuid().ToString("N")[..8]; // Generate a unique 8-character code
            await _redisDb.StringSetAsync(shortCode, originalUrl);
            return shortCode;
        }

        public async Task<string> GetOriginalUrl(string shortCode)
        {
            return await _redisDb.StringGetAsync(shortCode);
        }
    }
}
