using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using NewsManagementMinimal.DTO;
using NewsManagementMinimal.Models;

namespace NewsManagementMinimal.Data
{
    public class NewsDataContext
    {
        private readonly IConfiguration _configuration;
        //private readonly IMemoryCache _memoryCache;

        //public NewsDataContext(IConfiguration configuration, IMemoryCache memoryCache)
        //{
        //    _configuration = configuration;
        //    _memoryCache = memoryCache;
        //}

        public NewsDataContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<List<NewsDto>?> GetData()
        {
            //var a = _memoryCache.Set();

            List<NewsDto>? NewsDtos = new();
            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(_configuration["DataUrl"]);

            if (!response.IsSuccessStatusCode) return default;
            var responseContent = await response.Content.ReadAsStringAsync();

            var news = Newtonsoft.Json.JsonConvert.DeserializeObject<News>(responseContent);
            news?.Feed.ForEach(x => NewsDtos.Add(new NewsDto(x)));

            return NewsDtos;
        }
    }
}
