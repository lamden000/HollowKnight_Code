using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Script.BackEnd.MainMenu
{
    public class Leaderboard
    {
        public string PlayerName { get; set; }  // Match server-side property name
        public int Score { get; set; }
        public int Rank { get; set; }
    }
}
