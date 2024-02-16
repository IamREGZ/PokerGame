using System;
using System.Collections.Generic;
using System.Text;

namespace PokerGame
{
    class Computer : Player
    {
        #region Constructors
        public Computer(ref List<string> names)
        {
            Name = SetComputerName(ref names);
        }
        #endregion

        #region Methods
        private string SetComputerName(ref List<string> names)
        {
            Random rand = new Random();
            string name = names[rand.Next(0, names.Count)];
            names.Remove(name);  // To avoid duplicate computer names.

            return name;
        }
        #endregion
    }
}
