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

                try
                {
                    connection.Post($"@{line}, {Resources.LastTweetsStatistics.Replace("{0}", $"{PostsCount}")}: {statisticsJson}");
                }
                catch (Exception e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(e.Message);
                    Console.ResetColor();
                }

                Console.WriteLine(statisticsJson);
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

        //private static void AuthorizeAsUser(IConnection connection)
        //{
        //    string userName;
        //    string password;

        //    while (true)
        //    {
        //        Console.WriteLine($"{Resources.EnterToAccount}:");
        //        Console.Write($"{Resources.UserName}: ");
        //        userName = Console.ReadLine();
        //        Console.Write($"{Resources.Password}: ");
        //        password = ReadPassword();

        //        var account = new Account(userName.FormatTwitterUserName(), password);
        //    }
        //}

        //private static string ReadPassword()
        //{
        //    var password = "";
        //    var info = Console.ReadKey(true);
        //    while (info.Key != ConsoleKey.Enter)
        //    {
        //        if (info.Key != ConsoleKey.Backspace)
        //        {
        //            if (char.IsLetterOrDigit(info.KeyChar) || char.IsPunctuation(info.KeyChar))
        //            {
        //                Console.Write("*");
        //                password += info.KeyChar;
        //            }
        //        }
        //        else if (info.Key == ConsoleKey.Backspace)
        //        {
        //            if (!string.IsNullOrEmpty(password))
        //            {
        //                password = password.Substring(0, password.Length - 1);
        //                var pos = Console.CursorLeft;
        //                Console.SetCursorPosition(pos - 1, Console.CursorTop);
        //                Console.Write(" ");
        //                Console.SetCursorPosition(pos - 1, Console.CursorTop);
        //            }
        //        }
        //        info = Console.ReadKey(true);
        //    }
        //    Console.WriteLine();
        //    return password;
        //}
    }
}
