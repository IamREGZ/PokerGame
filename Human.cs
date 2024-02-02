using System;
using System.Collections.Generic;
using System.Text;

namespace PokerGame
{
    class Human : Player
    {
        #region Constructors
        public Human(string name)
        {
            Name = (name != "") ? name : "Player";
        }
        #endregion
    }
}
