namespace NewsManagementMinimal.Data
{
    public class NewsDataContext
    {
        private readonly IConfiguration _configuration;

        public NewsDataContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string?> GetData()
        {
            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(_configuration["DataUrl"]);

            if (!response.IsSuccessStatusCode) return default;
            var responseContent = await response.Content.ReadAsStringAsync();
            return responseContent;
        }
    }
}
