using System;
using PointTestTwitterBot.Business.Connections;
using PointTestTwitterBot.Business.Interfaces;

namespace PointTestTwitterBot
{
    class Program
    {
        static void Main(string[] args)
        {
            IConnection connection;
            try
            {
                connection = new TestConnection();
            }
            catch (Exception e)
            {
                OnConnectionException(e);
                return;
            }
            
            var twitterReader = new TwitterReader(connection);
            twitterReader.Start();
        }

        private static void OnConnectionException(Exception e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(e.Message);
            Console.ResetColor();

            Console.ReadLine();
        }
    }
}
