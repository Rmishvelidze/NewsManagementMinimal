using Microsoft.Extensions.Caching.Memory;
using NewsManagementMinimal.DTO;
using NewsManagementMinimal.Models;

namespace NewsManagementMinimal.Data
{
    public class NewsDataContext
    {
        private readonly IConfiguration _configuration;
        private readonly IMemoryCache _memoryCache;

        public NewsDataContext(IConfiguration configuration, IMemoryCache memoryCache)
        {
            _configuration = configuration;
            _memoryCache = memoryCache;
        }

        public async Task<List<NewsDto>?> GetData()
        {
            if (_memoryCache.Get("DATA") != null)
                return (List<NewsDto>?)_memoryCache.Get("DATA")!;

            List<NewsDto>? NewsDtos = new();
            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(_configuration["DataUrl"]);

            if (!response.IsSuccessStatusCode) return default;
            var responseContent = await response.Content.ReadAsStringAsync();

            var news = Newtonsoft.Json.JsonConvert.DeserializeObject<News>(responseContent);
            news?.Feed.ForEach(x => NewsDtos.Add(new NewsDto(x)));

            _memoryCache.Set("DATA", NewsDtos, TimeSpan.FromHours(1));
            return NewsDtos;
        }
    }
}
