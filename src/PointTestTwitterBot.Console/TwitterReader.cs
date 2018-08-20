using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using PointTestTwitterBot.Business.Enums;
using PointTestTwitterBot.Business.Interfaces;
using PointTestTwitterBot.Business.Models;
using PointTestTwitterBot.Extensions;
using PointTestTwitterBot.Properties;

namespace PointTestTwitterBot
{
    public class TwitterReader
    {
        private const int PostsCount = 5;
        private IConnection _connection;

        public TwitterReader(IConnection connection)
        {
            _connection = connection;
        }

        public void Start()
        {
            while (true)
            {
                Console.Write($"{Resources.EnterLogin} ({Resources.ForExit}): ");
                var line = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(line)) break;

                line = line.FormatTwitterUserName();

                var posts = _connection.GetUserLastPosts(line, PostsCount);
                if (!posts.Any())
                {
                    Console.WriteLine(Resources.PostsNotFound);
                    continue;
                }

                var statistics = GetMessagesStatistics(line, posts);
                var statisticsJson = JsonConvert.SerializeObject(statistics.GetCharactersStatistics());
                var message = $"@{line}, {Resources.LastTweetsStatistics.Replace("{0}", $"{PostsCount}")}: {statisticsJson}";

                try
                {
                    _connection.Post(message);
                }
                catch (Exception e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(e.Message);
                    Console.ResetColor();
                }

                Console.WriteLine(message);
                Console.WriteLine();
            }
        }

        private static Statistics GetMessagesStatistics(string userName, IEnumerable<string> messages)
        {
            var statistics = new Statistics(userName);

            //add checking lang

            foreach (var letter in Enum.GetNames(typeof(ABCru)).Select(s => s[0]))
                statistics.Characters.Add(letter, 0);

            foreach (var character in messages.SelectMany(m => m.ToLower()))
            {
                if (!statistics.Characters.ContainsKey(character)) continue;

                statistics.Characters[character]++;
            }

            return statistics;
        }
    }
}
