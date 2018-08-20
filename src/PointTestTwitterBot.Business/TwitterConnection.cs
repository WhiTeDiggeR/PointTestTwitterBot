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

            //
            tweets = GetFakeTweets(postsCount);
            //

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

        private List<TwitterStatus> GetFakeTweets(int count)
        {
            var tweets = new List<TwitterStatus>();

            var random = new Random();
            var news = new List<string>
            {
                "Российский актер Алексей Панин устроил дебош на борту самолета, следовавшего рейсом Симферополь-Москва. Об этом написал в своем аккаунте в инстаграме один из пассажиров рейса. Пользователь соцсети сообщил, что из-за поведения актера самолет пришлось вернуть назад в Симферополь.",
                "Украина продала двигатели для боевых самолетов в Китай, что противоречит интересам США, говорится в материале американской газеты The Washington Times. По данным издания, украинская компания \"Мотор Сич\" поставила 20 двигателей для 12 учебно-боевых самолетов JL-10.",
                "Наставник санкт-петербургского \"Зенита\" Сергей Семак рассказал о том, как слова президента Белоруссии Александра Лукашенко повлияли на футболистов и смотивировали их забить минскому \"Динамо\" восемь мячей, сообщает ТАСС.",
                "Команда задержанного Украиной российского танкера «Механик Погодин» за сутки дважды пресекла попытки проникнуть на судно со стороны неизвестных лиц, отказавшихся предъявить документы. Сообщение об этом размещено на сайте судоходной компании ООО «В.Ф.Танкер».",
                "По предварительным данным, актера экстренно госпитализировали после кровоизлияния в мозг. Он находится в отделении нейрохирургии Елизаветинской больницы Санкт-Петербурга. На счету Евгения Леонова-Гладышева более семидесяти ролей в театре и кино."
            };

            for (var i = 0; i < count; i++)
            {
                tweets.Add(new TwitterStatus
                {
                    Text = news[random.Next(0, news.Count)]
                });
            }

            return tweets;
        }
    }
}
