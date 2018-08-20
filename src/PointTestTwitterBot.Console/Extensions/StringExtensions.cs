using System;

namespace PointTestTwitterBot.Extensions
{
    public static class StringExtensions
    {
        public static string FormatTwitterUserName(this string name)
        {
            return name.StartsWith("@") ? name.Substring(1) : name;
        }
    }
}
