using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Security;
using System.Windows;
using Brackets2012;
using C5;
using System.Collections;

namespace Brackets2012
{
    public class Player : IEquatable<Player>, IComparer<Player>
    {
        //used for the overridden ToString() method.
        private const string lastfirst = "lf";
        private const string firstlast = "fl";

#region PlayerMembers
        private String firstName;
        public String _firstName
        {
            get
            {
                return firstName;
            }
            set
            {
                this.firstName = value;
            }
        }
        private String lastName;
        public String _lastName
        {
            get
            {
                return lastName;
            }
            set
            {
                this.lastName = value;
            }
        }
        public String wholeName;
        private String middleInitial;
        public String _middleInitial
        {
            get
            {
                return middleInitial;
            }
            set
            {
                this.middleInitial = value;
            }
        }
        private String suffix;
        public String _suffix
        {
            get
            {
                return suffix;
            }
            set
            {
                this.suffix = value;
            }
        }
        private String gender;
        public String _gender
        {
            get
            {
                return gender;
            }
            set
            {
                this.gender = value;
            }
        }

        private int BowlerID;
        public int _BowlerID
        {
            get
            {
                return BowlerID;
            }
            set
            {
                this.BowlerID = value;
            }
        }
        private int phoneNumber;
        public int _PhoneNumber
        {
            get
            {
                return phoneNumber;
            }
            set
            {
                this.phoneNumber = value;
            }
        }
        private String emailAddress;
        public String _emailAddress
        {
            get
            {
                return emailAddress;
            }
            set
            {
                this.emailAddress = value;
            }
        }

        public string _dateOfBirth;

        private String streetAddress;
        public String _streetAddress
        {
            get
            {
                return this.streetAddress;
            }
            set
            {
                this.streetAddress = value;
            }
        }
        private String city;
        public String _city
        {
            get
            {
                return this.city;
            }
            set
            {
                this.city = value;
            }
        }
        private String state;
        public String _state
        {
            get
            {
                return this.state;
            }
            set
            {
                this.state = value;
            }
        }

        private bool isChosen;
        public bool _isChosen
        {
            get
            {
                return isChosen;
            }
            set
            {
                this.isChosen = value;
            }
        }

        private double pinsPerFrameAvg;
        public double _pinsPerFrameAvg
        {
            get
            {
                return pinsPerFrameAvg;
            }
            set
            {
                this.pinsPerFrameAvg = value;
            }
        }
        private int pointsWon;
        public int _pointsWon
        {
            get
            {
                return pointsWon;
            }
            set
            {
                this.pointsWon = value;
            }
        }
        private int totalPointsPossible;
        public int _totalPointsPossible
        {
            get
            {
                return totalPointsPossible;
            }
            set
            {
                this.totalPointsPossible = value;
            }
        }

        private int totalPins;
        public int _totalPins
        {
            get
            {
                return totalPins;
            }
            set
            {
                this.totalPins = value;
            }
        }
        private double bowlerAvg;
        public double _bowlerAvg
        {
            get
            {
                return bowlerAvg;
            }
            set
            {
                this.bowlerAvg = value;
            }
        }
        private int handicap;
        public int _handicap
        {
            get
            {
                return handicap;
            }
            set
            {
                this.handicap = value;
            }
        }

        public String desiredBrackets; //brackets entered in the GUI.
        public int maxBrackets;
        public int currentNumOfBrackets;
        private bool isAllType;    //is used to denote when a player opts to be in "ALL" brackets
        public bool _isAllType
        {
            get
            {
                return isAllType;
            }
            set
            {
                this.isAllType = value;
            }
        }

        public int numOfConfirmedBrackets;  //how many brackets a player has actually been entered into.
        public int bracketValue;   //the dollar amount a bracket costs.
        public double refundOwed;
        public double moneyOwedToHouse;

        private List<Game> games = new List<Game>();

        //default constructor
        public Player(String first, String last, string desiredBracks)
        {
            this._suffix = " ";
            ;
            this.BowlerID = this.GetHashCode();
            this.firstName = first;
            this.lastName = last;
            this.wholeName = firstName + " " + lastName;
            this.desiredBrackets = desiredBracks;
            this.numOfConfirmedBrackets = 0;
              
            //now parse the string to get number of brackets or ALL brackets type
            if (this.desiredBrackets.Contains('A') || this.desiredBrackets.Contains('a'))
            {
                //set the number of brackets to the maximum integer value (2^32 - 1)
                this.maxBrackets = Int32.MaxValue;
                this._isAllType = true;
            }
            /////this is the comment
            else
            {
                try
                {
                    this._isAllType = false;
                    this.maxBrackets = int.Parse(this.desiredBrackets);
                }
                catch (Exception e)
                {
                    MessageBox.Show("Invalid Number of Brackets Entered.");
                    Console.Write(e.StackTrace);
                }
            }
        }
#endregion PlayerMembers


        /// <summary>
        /// Clear the player's number of brackets
        /// </summary>
        public void ClearPlayerBrackets()
        {
            numOfConfirmedBrackets = 0;
        }

        /// <summary>
        /// After a game, update the points won by the player.
        /// </summary>
        /// <param name="points"></param>
        public void AddToPointsWon(int points)
        {
            if (points < 0)
            {
                return;
            }
            else
            {
                this._pointsWon += points;
            }
        }

        public void AddToTotalPins(int pins)
        {
            if (pins == 0 || pins < 0)
            {
                return;
            }
            else
            {
                this.totalPins += pins;
            }
        }

        //override the toString() method to return the player's first and last name separated by a space.
        public string ToString(string format)
        {
            if (format.Equals(lastfirst))
            {
                //return name in the last,first format.
                return this.lastName + "," + this.firstName;
            }
            else if (format.Equals(firstlast))
            {
                //return first_last format of player's name.
                return this.firstName + " " + this.lastName;
            }
            else
            {
                //otherwise, just return the last name.
                return this.lastName;
            }
        }

        public bool Equals(Player other)
        {
            if (Object.ReferenceEquals(other, null)) return false;

            if (Object.ReferenceEquals(this, other)) return true;

            return (wholeName.Equals(other.wholeName) && this.BowlerID.Equals(other.BowlerID));
        }

  
        public int Compare(Player x, Player y)
        {
            //if this.bowlerID is less than the other's bowlerID,
            //return -1
            if (x._BowlerID < y._BowlerID)
            {
                return -1;
            }
            //otherwise, return 1 because this ID is greater.
            else
            {
                return 1;
            }
        }
    }
}
