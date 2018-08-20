using System;
using System.Collections.Generic;
using System.Linq;
using PointTestTwitterBot.Business;
using PointTestTwitterBot.Business.Enums;
using PointTestTwitterBot.Business.Models;
using PointTestTwitterBot.Business.Serialization;
using PointTestTwitterBot.Extensions;
using PointTestTwitterBot.Interfaces;
using PointTestTwitterBot.Properties;

namespace PointTestTwitterBot
{
    class Program
    {
        private const int PostsCount = 5;

        static void Main(string[] args)
        {
            IConnection connection;
            try
            {
                connection = new TwitterConnection();
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e.Message);
                Console.ResetColor();

                Console.ReadLine();
                return;
            }
            
            while (true)
            {
                Console.Write($"{Resources.EnterLogin} ({Resources.ForExit}): ");
                var line = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(line)) break;

                line = line.FormatTwitterUserName();

                var posts = connection.GetUserLastPosts(line, PostsCount);
                if (!posts.Any())
                {
                    Console.WriteLine(Resources.PostsNotFound);
                    continue;
                }

                var statistics = GetMessagesStatistics(line, posts);
                var statisticsJson = JsonSerialization.SerializeObject(statistics.GetCharactersStatistics());
                var message = $"@{line}, {Resources.LastTweetsStatistics.Replace("{0}", $"{PostsCount}")}: {statisticsJson}";

                try
                {
                    connection.Post(message);
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
