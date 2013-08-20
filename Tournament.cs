using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Brackets2012;
using C5;
using System.Collections.ObjectModel;
using System.Windows;


namespace Brackets2012
{
    /// <summary>
    /// 
    /// </summary>
    public class Tournament
    {
        //Naming and basics of tournament
        public int NumberOfSquads;
        public int StartingLane;
        public int EndingLane;
        public int BowlersPerLane;
        public int NumberOfGamesPerSquad;
        public int MaxScore;
        public int NumberOfBowlers;
        public String StartingDate;
        public String BowlingCenterName;
          
        //Tournament options
        public bool IsHandicap;
        public bool AutoCalcHandicapAverages;
        public bool SplitsCountAsStrikes;
        public bool BowlersAreInDivisions;
        public bool IsProAmTournament;
        public bool IsNoTap;

        //Individual Handicap variables       
        public int HandicapPercentage;
        public int BaseLeagueAverage;
        public int VacancyScore;
        public int BlindAverage;
        public int BookAverageNotPresentScore;
          
        //Non-Standard Options
        public int DummyScore;
        public int BaseMinusHandicap;

        //Prices, Fees, and Financials
        public double EntryFee;
        public double LinageFee;
        public double HouseProfit;

        //Sidepots - Scratch and Handicap
        public double ScratchSidepotsFee;
        public double ScratchSidepotsHouseProfit;
        public double HandicapSidepotsFee;
        public double HandicapSidepotsHouseProfit;

        //Mystery Doubles - Scratch and Handicap
        public double ScratchMysteryDoublesFee;
        public double HandicapMysteryDoublesFee;
        public double ScratchMysteryDoublesHouseProfit;
        public double HandicapMysteryDoublesHouseProfit;

        //Sweeper Doubles - Scratch and Handicap
        public double ScratchSweeperDoublesFee;
        public double HandicapSweeperDoublesFee;
        public double ScratchSweeperDoubleHouseProfit;
        public double HandicapSweeperDoublehouseProfit;

        private ObservableCollection<Bracket> Brackets;

        public SortedList<int, SortedList<int, Match>> TournamentRoundMatches { get; private set; }
        public Match ThirdPlaceMatch { get; private set; }

        public Tournament(int rounds)
        {
        }

        /// <summary>
        /// CalculateSingleEliminationRounds
        /// 
        /// This method calculates the number of rounds necessary
        /// in a single elimination bracket, etc needed.
        /// 
        /// Assumptions: 
        /// Two (2) competitors per match-up, and N competitors total.
        /// There will be R rounds needed...
        /// specifically:  R = [log2(N)], if N is a power of 2.
        /// Therefore N = 2^(R).
        /// 
        /// If N is not a power of 2, in the opening round
        /// [(2^R) - N] competitors will recieve a bye.
        /// 
        /// 
        /// </summary>
        /// <returns></returns>
        public int CalculateSingleEliminationRounds(){

            if(IsPowerOfTwo(NumberOfBowlers)){

                return ((int)(Math.Log(NumberOfBowlers,2.0)));

            }
            else{
                MessageBox.Show("Byes will be needed!");
                return((int)(Math.Log(NumberOfBowlers,2.0))-NumberOfBowlers);
            }



        }

        public int CalculateDoubleEliminationRounds()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// IsPowerOfTwo()
        /// 
        /// This method returns a bool value for a given number X
        /// true if X is a power of two, otherwise it returns false.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        bool IsPowerOfTwo(int x)
        {
            return (x != 0) && ((x & (x - 1)) == 0);
        }



    }
}
