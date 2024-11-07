using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Script.BackEnd.User
{
    public class PlayerInfo
    {
        public int AccountId { get; private set; }
        public string Name { get; private set; }

        public PlayerInfo(int accountId, string name)
        {
            AccountId = accountId;
            Name = name;
        }
    }
}
