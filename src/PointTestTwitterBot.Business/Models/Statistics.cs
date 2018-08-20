using System;
using System.Collections.Generic;
using System.Linq;

namespace PointTestTwitterBot.Business.Models
{
    public class Statistics
    {
        public Statistics(string userName)
        {
            UserName = userName;
            Characters = new Dictionary<char, int>();
        }

        public string UserName { get; protected set; }
        public Dictionary<char, int> Characters { get; protected set; }

        public Dictionary<string, decimal> GetCharactersStatistics()
        {
            var sum = Characters.Sum(c => c.Value);

            return Characters.ToDictionary(character => character.Key.ToString(), character => decimal.Round((decimal) character.Value / sum, 4, MidpointRounding.AwayFromZero));
        }
    }
}
