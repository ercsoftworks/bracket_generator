using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Brackets2012
{
    class PrizeFundManager
    {
        public int N;     //N Places to be paid (teams/individuals)
        public int M;     //N-1
        public double K;   // (N*M)/2
        public double moneyUsed;   //the total money used for all places
        public double difference;  //The money differnce between each place.
        public double lastPlacePayout;  //money paid to last place
        public double firstPlacePayout;  //money paid to first place.

        public int[] payouts;

        /// <summary>
        /// Constructor for PrizeFundManager
        /// </summary>
        /// <param name="numTeams"></param>
        /// <param name="prizeMoney"></param>
        public PrizeFundManager(int numTeams, double firstPlace, double lastPlace, double prizeMoney)
        {
            this.moneyUsed = prizeMoney;
            this.N = numTeams;
            this.M = N - 1;
            this.K = (N * M) / 2;
            this.firstPlacePayout = firstPlace;
            this.lastPlacePayout = lastPlace;
        }
    }
}
