namespace NewsManagementMinimal.Models
{
    public class TickerSentiment
    {
        public string Ticker { get; set; }
        public double Relevance_score { get; set; }
        public double Ticker_sentiment_score { get; set; }
        public string Ticker_sentiment_label { get; set; }
    }
}
