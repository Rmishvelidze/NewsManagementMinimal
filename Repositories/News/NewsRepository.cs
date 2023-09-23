using NewsManagementMinimal.Data;
using NewsManagementMinimal.DTO;

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

        public Task<List<NewsDto>> GetNewsByDays(int dayQuantity)
        {
            throw new NotImplementedException();
        }

        public Task<List<NewsDto>> GetNewsByText(string text)
        {
            throw new NotImplementedException();
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
