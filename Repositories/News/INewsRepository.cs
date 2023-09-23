using NewsManagementMinimal.DTO;

namespace NewsManagementMinimal.Repositories.News
{
    public interface INewsRepository
    {
        Task<List<NewsDto>?> GetAllNews();
        Task<List<NewsDto>?>? GetNewsByDays(int daysQuantity);
        Task<List<NewsDto>?> GetNewsByText(string text);
        Task<List<NewsDto>?>? GetLatest5News();
        string Subscribe();
    }
}
