namespace NewsManagementMinimal.Models
{
    public class Feed
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public string Time_published { get; set; }
        public List<string> Authors { get; set; }
        public string Summary { get; set; }
        public string Banner_image { get; set; }
        public string Source { get; set; }
        public string Category_within_source { get; set; }
        public string Source_domain { get; set; }
        public List<Topic> Topics { get; set; }
        public double Overall_sentiment_score { get; set; }
        public string Overall_sentiment_label { get; set; }
        public List<TickerSentiment> Ticker_sentiment { get; set; }
    }
}
