using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Script.BackEnd.User
{
    public class PlayerSession
    {
        public static PlayerInfo CurrentPlayer { get; private set; }

        public static void SignIn(int accountId, string name)
        {
            // Create and store player info after successful sign-in
            CurrentPlayer = new PlayerInfo(accountId, name);
        }

        public static bool IsSignedIn => CurrentPlayer != null;  // Check if a player is signed in
    }
}
