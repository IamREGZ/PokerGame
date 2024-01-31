using System;
using System.Collections.Generic;
using System.Text;

namespace PokerGame
{
    class Card
    {
        #region Properties
        private string value;
        private string suit;
        #endregion

        #region Constructors
        public Card(string value, string suit)
        {
            this.value = value;
            this.suit = suit;
        }
        #endregion

        #region Getters
        public string GetCardValue()
        {
            return value;
        }

        public string GetCardSuit()
        {
            return suit;
        }
        #endregion

        #region Setters
        public void SetCardValue(string value)
        {
            this.value = value;
        }

        public void SetCardSuit(string suit)
        {
            this.suit = suit;
        }
        #endregion
    }
}
