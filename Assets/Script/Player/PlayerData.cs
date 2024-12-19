using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Script.Player
{
    public class PlayerData
    {
        public int Soul { get; set; }
        public int Health { get; set; }
        public int Money { get; set; }
        public PlayerData() { 
            Soul = 0;
            Health = 0;
            Money = 0;
        
        }
    }
}
