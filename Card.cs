using System;
using System.Collections.Generic;
using System.Text;

namespace PokerGame
{
    class Card
    {
        private string value;
        private string suit;

        public Card(string value, string suit)
        {
            this.value = value;
            this.suit = suit;
        }

        public string GetCardValue()
        {
            return value;
        }

        public void SetCardValue(string value)
        {
            this.value = value;
        }

        public string GetCardSuit()
        {
            return suit;
        }

        public void SetCardSuit(string suit)
        {
            this.suit = suit;
        }
    }
}
