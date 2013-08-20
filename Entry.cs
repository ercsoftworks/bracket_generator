using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Brackets2012
{
    public class Entry
    {
        public Player player1;
        public Player player2;
        public bool hasBeenUsed;
        public String player1Name
        {
            get
            {
                return this.player1.wholeName;
            }
            set
            {
                player1Name = player1.wholeName;
            }
        }
        public String player2Name
        {
            get
            {
                return this.player2.wholeName;
            }
            set
            {
                player2Name = player2.wholeName;
            }
        }
          
        public Entry(Player p1, Player p2)
        {
            this.player1 = p1;
            this.player2 = p2;
            this.hasBeenUsed = false;
        }
    }
}
