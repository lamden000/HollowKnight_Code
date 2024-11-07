using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Script.BackEnd
{
    public class Room
    {
        public int RoomId { get; set; }
        public string HostName { get; set; }
        public bool IsPrivate { get; set; }
        public string Password { get; set; }
        public int CurrentPlayerCount { get; set; } = 0;
        public int MaxPlayers { get; set; } = 4;
    }
}
