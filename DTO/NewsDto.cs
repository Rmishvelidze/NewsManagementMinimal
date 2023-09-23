using NewsManagementMinimal.Models;
using System;

namespace NewsManagementMinimal.DTO
{
    public class NewsDto
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public DateTime? Time_published { get; set; }
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

        public NewsDto(Feed feed)
        {
            Title = feed.Title;
            Url = feed.Url;
            if (DateTime.TryParseExact(feed.Time_published, "yyyyMMddTHHmmss", null,
                    System.Globalization.DateTimeStyles.None, out var time))
                Time_published = time;
            else
                Time_published = null;
            Authors = feed.Authors;
            Summary = feed.Summary;
            Banner_image = feed.Banner_image;
            Source = feed.Source;
            Category_within_source = feed.Category_within_source;
            Source_domain = feed.Source_domain;
            Topics = feed.Topics;
            Overall_sentiment_score = feed.Overall_sentiment_score;
            Overall_sentiment_label = feed.Overall_sentiment_label;
            Ticker_sentiment = feed.Ticker_sentiment;
        }
    }
}
