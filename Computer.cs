using System;
using System.Collections.Generic;
using System.Text;

namespace PokerGame
{
    class Computer : Player
    {
        #region Constructors
        public Computer()
        {
            Name = SetComputerName();
        }
        #endregion

        #region Methods
        private string SetComputerName()
        {
            Random rand = new Random();
            string[] names = { "Alex", "Ben", "Chris", "Daniel", "Edward" };

            return names[rand.Next(0, names.Length)];
        }
        #endregion
    }
}
