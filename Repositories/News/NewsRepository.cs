using NewsManagementMinimal.Data;
using NewsManagementMinimal.DTO;
using NewsManagementMinimal.Models;
using Newtonsoft.Json;

namespace NewsManagementMinimal.Repositories.News
{
    public class NewsRepository : INewsRepository
    {
        private readonly NewsDataContext _newsData;

        public NewsRepository(NewsDataContext newsData)
        {
            _newsData = newsData;
        }

        public async Task<List<NewsDto>?> GetAllNews() => await _newsData.GetData();

        public Task<List<NewsDto>?>? GetNewsByDays(int daysQuantity)
        {
            var targetDate = DateTime.Now.AddDays(-daysQuantity);
            var newsDtos = _newsData.GetData().Result;
            if (newsDtos == null) return null;
            var result = newsDtos.Where(x => x.Time_published > targetDate).ToList();
            return Task.FromResult<List<NewsDto>?>(result);
        }

        public Task<List<NewsDto>?> GetNewsByText(string text)
        {
            var newsDtos = _newsData.GetData().Result;
            List<string> newsStrings = new();
            List<NewsDto?> result = new();

            newsDtos?.ForEach(x => newsStrings.Add(JsonConvert.SerializeObject(x)));
            newsStrings.ForEach(x =>
            {
                if (!x.Contains(text)) return;
                var feed = JsonConvert.DeserializeObject<Feed>(x);
                if (feed != null) result.Add(new NewsDto(feed));
            });

            return Task.FromResult<List<NewsDto>?>(result!);
        }

        public Task<List<NewsDto>> GetLatestNews()
        {
            throw new NotImplementedException();
        }

        public Task Subscribe()
        {
            throw new NotImplementedException();
        }
    }
}
