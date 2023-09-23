namespace NewsManagementMinimal.Models
{
    public class News
    {
        public string Items { get; set; }
        public string Sentiment_score_definition { get; set; }
        public string Relevance_score_definition { get; set; }
        public List<Feed> Feed { get; set; }
    }
}
