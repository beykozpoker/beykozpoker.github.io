using System;
using System.Collections.Generic;
using System.Text;

namespace PokerBatakGameServer.SaveLoad
{
    [System.Serializable]
    public class DataManager
    {
        public Dictionary<string, Players> players = new Dictionary<string, Players>();
        public DataManager(Dictionary<string, Players> _players)
        {
            players = _players;
        }
    }
}
