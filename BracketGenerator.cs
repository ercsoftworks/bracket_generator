/**
* BracketGenerator.cs
* 
* Author: Eric Carestia
* This is currently a work in progress, and as such
* it's incomplete and may not function properly
* 
**/
using System;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using C5;

namespace Brackets2012
{
    /// <summary>
    /// BracketGenerator Class
    /// 
    /// This class handles the generation of the tournament brackets.
    /// 
    /// Will recieve list of players in the tournament along with the specific type of bracket
    /// it will generate.
    /// 
    /// This class divides the types of players into two lists: NumericBrackets and AllBrackets
    /// 
    /// The NumericBracket list holds all of the players who requested/paid for
    /// a specific number of brackets
    /// 
    /// The AllBracket list holds all of the players who did not specify
    /// a set number of brackets.
    /// 
    /// There will be two generating algorithms when it comes to creating the 
    /// brackets.
    /// 
    /// The first of these - StartGeneration(), will use a boolean flag, several counters,
    /// and a shrinking list of available players to determine when to stop creating the
    /// brackets.
    /// 
    /// When there are no more players left, available players is less than the bracket size,
    /// or when all available brackets are used up, the generation algorithm will cease.
    /// 
    ///  
    /// This class will handle validation of the current set of generated brackets
    /// to ensure no duplicate entries exist within a given bracket.
    /// </summary>
    public class BracketGenerator : INotifyPropertyChanged
    {
        //Sample represents the list of available choices
        ArrayList<Player> Master = new ArrayList<Player>();  //Master list of Players
        ArrayList<Player> AllBrackets = new ArrayList<Player>();  //List of all Players who entered "ALL" for number of brackets
        ArrayList<Player> NumericBrackets = new ArrayList<Player>(); //List of all Players who entered a specific number of desired brackets
        ArrayList<Entry> ChosenPairs = new ArrayList<Entry>();  //The list that holds the newly formed "bracket".

        public ObservableCollection<RefundedPlayer> RefundList = new ObservableCollection<RefundedPlayer>();
        ArrayList<String> NamesInBracket = new ArrayList<String>();
        ArrayList<Entry> playerGroups = new ArrayList<Entry>();  // these three lists are used in removal of all repeated pairs
        ArrayList<Entry> copyPlayeGroups = new ArrayList<Entry>();
        ArrayList<Entry> distinctPlayerGroups = new ArrayList<Entry>();

        public ArrayList<ArrayList<Entry>> generatedBrackets = new ArrayList<ArrayList<Entry>>();

        public ObservableCollection<ArrayList<ArrayList<Entry>>> CompleteBrackets;  //the master list of all sets of completed brackets.

        private int averageNumberOfBrackets;    //used to ensure no brackets are excessively generated
        private int numberBracketsRemaining = 0;  //used to track how many brackets are left to make, based on sum of all numeric bracket entries

        public int bracketSize;              //used to create brackets of various sizes.
        public int numOfGames;
        public double entryFee;
        public double BracketValue;
        public double HouseProfit;
        public double LineageFee;


        /// <summary>
        /// Enumerator for each Type of bracket
        /// </summary>
        public enum BracketType
        {
            HandicapStraight, //normal 1,2,3 bracket (A player's games 1, 2, and 3 will count toward winning the bracket.
            ScratchStraight, //3-2-1
            HandicapReverse,
            ScratchReverse, //a 3,2,1 bracket
            StratchSweeper,
            HandicapSweeper
        };

        //Event for CompletedBrackets used in the databinding on CentralWindow.xaml
        public event PropertyChangedEventHandler PropertyChanged;

        //default constructor
        public BracketGenerator()
        {
            //populate members with some default values.
            this.bracketSize = 8;
            this.numOfGames = 3;
            this.entryFee = 1.00;
            this.BracketValue = entryFee * this.bracketSize;
            this.HouseProfit = entryFee;
            this.CompleteBrackets = new ObservableCollection<ArrayList<ArrayList<Entry>>>();
        }

        /// <summary>
        /// updates the binding in CentralWindow.xaml
        /// </summary>
        /// <param name="name"></param>
        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        /// <summary>
        /// SeedMasterList()
        /// Core Procedural Method
        /// 
        /// This method will recieve the players and all relevant data entered 
        /// from the GUI. All information/data recieved will go into the 
        /// Master player list which will be passed on to the bracket generating algorithm.
        ///
        /// For testing purposes, the players and their desired number of brackets are hard coded
        /// to serve as a control group.
        /// </summary>
        public void SeedMasterList()
        {

            Master.Add(new Player("John", "Smith", "2"));
            Master.Add(new Player("Mike", "Jones", "4"));
            Master.Add(new Player("Tony", "Russo", "1"));
            Master.Add(new Player("Eric", "Carestia", "3"));
            Master.Add(new Player("Homer", "Simpson", "2"));
            Master.Add(new Player("Peter", "Griffin", "4"));
            Master.Add(new Player("Phillip", "Fry", "All"));
            Master.Add(new Player("Nathan", "Explosion", "3"));
            Master.Add(new Player("Toki", "Wartooth", "2"));
            Master.Add(new Player("William", "Murderface", "1"));
            Master.Add(new Player("John", "Doe", "3"));
            Master.Add(new Player("Jason", "Vorhees", "2"));
            Master.Add(new Player("Freddy", "Kruger", "4"));
            Master.Add(new Player("Sarah", "Connor", "2"));
            Master.Add(new Player("Marc", "Carestia", "3"));
            Master.Add(new Player("Kristin", "Smith", "5"));
            Master.Add(new Player("Valerie", "Thompson", "4"));
            Master.Add(new Player("Jason", "Reck", "2"));
            Master.Add(new Player("Heather", "Kelly", "All"));
            Master.Add(new Player("Juan", "Cruz", "3"));
            Master.Add(new Player("Andrew", "Holtz", "3"));
            Master.Add(new Player("Dirk", "Diggler", "5"));
            Master.Add(new Player("Scott", "LaForge", "2"));
            Master.Add(new Player("Ryan", "Birmingham", "1"));
            Master.Add(new Player("Justin", "Hodnett", "3"));
            Master.Add(new Player("Anna", "Nguyen", "3"));
            Master.Add(new Player("Elmer", "Acevedo", "6"));
            Master.Add(new Player("Melissa", "Arcena", "3"));
            Master.Add(new Player("Christopher", "McCoy", "1"));
            Master.Add(new Player("Vanessa", "Victoria", "3"));
            Master.Add(new Player("Dana", "Cuffe", "2"));
            Master.Add(new Player("Matthew", "Todd", "5"));
            Master.Add(new Player("Jason", "DeSimone", "4"));
            Master.Add(new Player("Ximena", "Silva", "3"));
            Master.Add(new Player("Kellie", "Felt", "2"));
            Master.Add(new Player("Alyssa", "Billburg", "1"));
            Master.Add(new Player("Keith", "Prisco", "2"));
            Master.Add(new Player("Bridgette", "Sanford", "4"));
            Master.Add(new Player("Joey", "Taddeuci", "3"));




        }//End SeedMasterList();

        /// <summary>
        /// StartBracketGeneration()
        /// 
        /// This method generates random unique brackets based upon 
        /// the players contained in the master list.
        /// </summary>
        /// <param name="sb"></param>
        public void StandardBracketGeneration()
        {
            ///int sizeOfBracket, int numberGames, double bracketVal  generate all distinct pairs.
            //TODO: will code this as a parameter later
            /** this.bracketSize = sizeOfBracket;
            this.numOfGames = numberGames;
            this.BracketValue = bracketVal*this.bracketSize;
            this.HouseProfit = bracketVal;
            **/
            //allocate the CompletedBrackets ArrayList (nested).
            //will automate this later with database values from random bowlers that will
            //be pulled from a separate associative array that indicates the number of 
            //available bowlers who want to play.
            SeedMasterList();

            //Check to see if bracket size is greater than the list of players
            if (Master.Count < bracketSize)
            {
                MessageBox.Show("Not Enough Players to Make a Bracket of " + bracketSize + ",\n" +
                                "How Many Players Entered: " + Master.Count + "\n" +
                                "Please Add more players or choose a smaller bracket size");
                return;
            }

            //Separate Players into Numeric / All Bracket types
            SeparatePlayerLists();
            //Calculate the possible number of brackets
            CalculateBracketsPossible();
            GeneratePairsAndRemoveDuplicates();

            while (numberBracketsRemaining > 0 && !((numberBracketsRemaining) < bracketSize / 2))
            {

                distinctPlayerGroups.Shuffle();

                int i = 0;

                //While we have players to enter, the number of brackets
                while (ChosenPairs.Count < bracketSize / 2 && (numberBracketsRemaining > 0 || ChosenPairs.Count > 0) &&
                       distinctPlayerGroups.Count > bracketSize / 2)
                {
                    //If there aren't enough players to generate a bracket- stop.
                    if (numberBracketsRemaining < bracketSize / 2)
                    {
                        break;
                    }

                    if (!(i < distinctPlayerGroups.Count))
                    {
                        i = distinctPlayerGroups.Count - 1;
                    }
                    //if the current player is not chosen and the current player still has brackets remaining.
                    if (distinctPlayerGroups[i].hasBeenUsed == false && ((distinctPlayerGroups[i].player1.currentNumOfBrackets < distinctPlayerGroups[i].player1.maxBrackets) && (distinctPlayerGroups[i].player2.currentNumOfBrackets < distinctPlayerGroups[i].player2.maxBrackets)))
                    {
                        //check to see if there is a duplicate name in the bracket
                        bool stopChecking = false;
                        if (NamesInBracket.Count >= 2)
                        {
                            //now search the names in the bracket....
                            foreach (Entry e in distinctPlayerGroups)
                            {
                                if (stopChecking == true)
                                {
                                    break;
                                }
                                if (NamesInBracket.Contains(e.player1.wholeName) || NamesInBracket.Contains(e.player2.wholeName))
                                {
                                    continue;
                                }
                                else
                                {
                                    //otherwise, we've now found the entry with no names in the list
                                    i = distinctPlayerGroups.IndexOf(e);
                                    if (distinctPlayerGroups[i].hasBeenUsed == false && ((distinctPlayerGroups[i].player1.currentNumOfBrackets < distinctPlayerGroups[i].player1.maxBrackets) && (distinctPlayerGroups[i].player2.currentNumOfBrackets < distinctPlayerGroups[i].player2.maxBrackets)))
                                    {
                                        //Console.WriteLine("Index: " + i + " GOOD ENTRY FOUND: " + distinctPlayerPairs[i].player1.wholeName + " AND " + distinctPlayerPairs[i].player2.wholeName);
                                        NamesInBracket.Add(distinctPlayerGroups[i].player1.wholeName);
                                        NamesInBracket.Add(distinctPlayerGroups[i].player2.wholeName);
                                        stopChecking = true;
                                        break;
                                    }
                                    else
                                    {
                                        continue;
                                    }
                                }
                            }
                        }

                        //mark that player as chosen....
                        distinctPlayerGroups[i].hasBeenUsed = true;
                        distinctPlayerGroups[i].player1.currentNumOfBrackets++;
                        distinctPlayerGroups[i].player2.currentNumOfBrackets++;
                        //add them to the chosen list
                        ChosenPairs.Add(distinctPlayerGroups[i]);
                        if (NamesInBracket.Count == 0)
                        {
                            NamesInBracket.Add(distinctPlayerGroups[i].player1.wholeName);
                            NamesInBracket.Add(distinctPlayerGroups[i].player2.wholeName);
                        }

                        //if the person was a numeric bracket, deduct the number of they're bracket from
                        //the over all number of brackets.
                        if (distinctPlayerGroups[i].player1._isAllType == false)
                        {
                            numberBracketsRemaining--;
                        }
                        if (distinctPlayerGroups[i].player2._isAllType == false)
                        {
                            numberBracketsRemaining--;
                        }
                    }
                    else
                    {
                        //if we're at the end of the list
                        if (i == (distinctPlayerGroups.Count - 1))
                        {
                            //move the index position to the beginning of the list
                            //distinctPlayerPairs.Remove(distinctPlayerPairs[i]);
                            i = 0;
                            continue;    //and move to the next top level iteration.
                        }

                        //or just move to the next element;
                        i++;
                        continue;    // and onto next top level iteration
                    }
                    i++;
                }
                //If the Program reaches this point:
                //A bracket has been successfully generated.
                if (ChosenPairs.Count == bracketSize / 2)
                {
                    var temp = ChosenPairs.Clone();

                    generatedBrackets.Add((ArrayList<Entry>)temp);

                }
                else
                {
                    //now Print the list of the complete brackets.
                    ChosenPairs.Clear();
                    NamesInBracket.Clear();
                    break;
                }
                ChosenPairs.Clear();
                NamesInBracket.Clear();

                OnPropertyChanged("Brackets");
            }
            CompleteBrackets.Add(generatedBrackets);

            StringBuilder sb = new StringBuilder();
            //Let's generate our brackets!
            MessageBox.Show("Bracket Generation Complete. \n Brackets Made: " + generatedBrackets.Count);
            ClearLists();

            Master.Dispose();
            NumericBrackets.Dispose();
            AllBrackets.Dispose();
        }//End StartBracketGeneration();

        /// <summary>
        /// GeneratePairsAndRemoveDuplicates
        /// 
        /// This method generates all unique permutations of (MasterList.Count)P(2), 
        /// Basically only unique pairs of players are made, and duplicates made
        /// during the preprocessing stage (O(n^2) space and time complexity)
        /// 
        /// </summary>
        public void GeneratePairsAndRemoveDuplicates()
        {
            foreach (Player x in Master)
            {
                foreach (Player y in Master)
                {
                    if (x.Equals(y) || y.Equals(x))
                    {
                        continue;
                    }
                    else
                    {
                        playerGroups.Add(new Entry(x, y));
                        copyPlayeGroups.Add(new Entry(x, y));
                    }
                }
            }

            int pos = 0;
            Boolean broke = false;
            foreach (Entry e in playerGroups)
            {
                broke = false;
                pos = 0;
                foreach (Entry f in playerGroups)
                {
                    if ((f.player1.wholeName.Equals(e.player2.wholeName) && f.player2.wholeName.Equals(e.player1.wholeName)) && e.hasBeenUsed == false)
                    {
                        pos = playerGroups.IndexOf(f);
                        playerGroups[pos].hasBeenUsed = true;
                        e.hasBeenUsed = true;
                        f.hasBeenUsed = true;
                        distinctPlayerGroups.Add(f);

                        broke = true;
                    }

                    if (broke)
                        break;
                }
            }

            foreach (Entry en in distinctPlayerGroups)
            {
                en.hasBeenUsed = false;
            }
        }

        /// <summary>
        /// Just print all players 
        /// </summary>
        /// <param name="players"></param>
        private void PrintPlayers(ArrayList<Entry> players)
        {
            foreach (Entry e in players)
            {
                Console.WriteLine(e.player1.wholeName + "," + e.player2.wholeName);
            }
        }

        /// <summary>
        /// Generate blind double for the tournaments
        /// </summary>
        public void BlindDoubleGeneration()
        {
          throw new NotImplementedException();

        }

        /// <summary>
        /// generate nassaus for the tournament
        /// </summary>
        public void NassauGeneration()
        {
        throw new NotImplementedException();
        }

        /// <summary>
        /// CalculateBracketsPossible()
        /// Utility Method
        /// 
        /// calculateBracketsPossible determines both 
        /// the sum total of numeric bracket entries (all the "Numeric" players
        /// number of maximum brackets).
        /// 
        /// and the average number of brackets, which will be used to set all of the
        /// "All" players maximum brackets to the average to avoid duplicates
        /// </summary>
        private void CalculateBracketsPossible()
        {
            foreach (Player p in NumericBrackets)
            {
                numberBracketsRemaining += p.maxBrackets;
            }
            averageNumberOfBrackets = numberBracketsRemaining / NumericBrackets.Count;
        }//End calculateBracketsPossible();

        /// <summary>
        /// MakeAllPlayersAvailable()
        /// Utility Method
        /// This method sweeps through the master list of players
        /// and makes them available by flipping their boolean 
        /// _isChosen flag.
        /// </summary>
        private void MakeAllPlayersAvailable()
        {
            ///sweep through the list and set the isChosen flag to false
            for (int k = 0; k < Master.Count; k++)
            {
                Master[k]._isChosen = false;
            }
        }//End MakeAllPlayersNotChosen()

        /// <summary>
        /// Utility Method-
        /// Method takes both lists of players
        /// that have indicated a specific number of brackets
        /// from those who indicated all brackets.
        /// </summary>
        private void SeparatePlayerLists()
        {
            ///Method Extraction point ----SeparatePlayerLists()
            foreach (Player p in Master)
            {
                if (p._isAllType == true)
                {
                    //if they're all...add to the "AllBrackets" list
                    AllBrackets.Add(p);
                }
                else
                {
                    //otherwise, the player wants a specific number of brackets
                    //then add them to the "NumericBrackets" list.
                    NumericBrackets.Add(p);
                }
            }
        }//End MergePlayerLists();

        /// <summary>
        /// Utility method - MergePlayerList
        /// </summary>
        /// <param name="sb">String Builder _sb: this allows for testing and </param>
        private void MergePlayerLists(StringBuilder sb)
        {
            //iterate through the AllBrackets list, 
            //set each of those players maxBrackets to
            //the average brackets of the NumericBrackets'
            //
            foreach (Player p in AllBrackets)
            {
                p.maxBrackets = this.averageNumberOfBrackets;
                NumericBrackets.Add(p);
                sb.Append(p.ToString("fl") + " " + p.maxBrackets);
                sb.AppendLine();
            }
            sb.Append("Numeric Player Count: " + NumericBrackets.Count);
            //MessageBox.Show(_sb.ToString());
        }//End MergePlayerLists()

        /// <summary>
        /// ClearLists()
        /// This utility method clears the ALL, NUMERIC, and MASTER
        /// Player Lists
        /// </summary>
        private void ClearLists()
        {
            this.AllBrackets.Clear();
            this.NumericBrackets.Clear();
            this.Master.Clear();
        }//End ClearLists();

        /// <summary>
        /// CalculateTotalPayments()
        /// This utility method returns an ArrayList of all Players
        /// who are owed or who owe money to the house.
        /// </summary>
        /// <returns></returns>
        private void CalculateRefundsAndDebt()
        {
            RefundList.Clear();
            foreach (Player p in Master)
            {
                if (p.numOfConfirmedBrackets < p.maxBrackets)
                {
                    p.refundOwed = BracketValue * (p.numOfConfirmedBrackets - p.maxBrackets);
                    RefundList.Add(new RefundedPlayer(p));
                }

                return;
            }
        }
    }


}
