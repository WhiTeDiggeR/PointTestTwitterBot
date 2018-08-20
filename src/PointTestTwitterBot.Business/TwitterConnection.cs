using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using PointTestTwitterBot.Business.Properties;
using PointTestTwitterBot.Interfaces;
using TweetSharp;

namespace PointTestTwitterBot.Business
{
    public class TwitterConnection : IConnection
    {
        private const string ConsumerKey = "";
        private const string ConsumerSecret = "";
        private const string AccessToken = "";
        private const string AccessTokenSecret = "";
        private readonly TwitterService service;
        
        public TwitterConnection()
        {
            if (!HaveInternet()) throw new Exception(Resources.ConnectionFailed);

            service = new TwitterService(ConsumerKey, ConsumerSecret);
            service.AuthenticateWith(AccessToken, AccessTokenSecret);
        }

        public List<string> GetUserLastPosts(string userName, int postsCount = 5)
        {
            var tweets = service.ListTweetsOnUserTimeline(new ListTweetsOnUserTimelineOptions
            {
                ScreenName = userName,
                Count = postsCount
            });

            return tweets?.Select(tweet => tweet.Text).ToList() ?? new List<string>();
        }

        public void Post(string text)
        {
            try
            {
                service.SendTweet(new SendTweetOptions
                {
                    Status = text
                });
            }
            catch (Exception)
            {
                throw new Exception(Resources.TweetNotSent);
            }
        }

        private bool HaveInternet()
        {
            var client = new HttpClient();

            try
            {
                var response = client.GetAsync("https://twitter.com")?.Result?.IsSuccessStatusCode ?? false;

                return response;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
