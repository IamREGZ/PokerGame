using System;

namespace PokerGame
{
    class Program
    {
        #region Fields
        static Card[] cards;
        static readonly Random rand = new Random();
        #endregion

        #region Main Program
        static void Main(string[] args)
        {
            Console.WriteLine("POKER GAME\n----------");

            MenuNavigation(0);
        }

        // Function to navigate the console to different screens.
        private static void MenuNavigation(int menuCode)
        {
            string menuTitle = "";
            string[] menuOptions = { };
            string command;
            int selected = -1;

            // Change the menu title and options list.
            switch (menuCode)
            {
                case 0:
                    menuTitle = "MAIN MENU";
                    menuOptions = new string[2] { "Play Game", "Generate Cards" };
                    break;
                case 1:
                    menuTitle = "PLAY GAME";
                    menuOptions = new string[2] { "One Player - Pending", "Multiplayer - Pending" };
                    break;
                case 2:
                    menuTitle = "GENERATE CARDS";
                    menuOptions = new string[3] { "Random Cards Generation", "Random Cards Selection", "Manual Card Entry" };
                    break;
            }

            do
            {
                Console.WriteLine();
                Console.WriteLine(menuTitle);

                // Displaying all valid options, including back/exit.
                for (int i = 0; i < menuOptions.Length; i++)
                {
                    Console.WriteLine($"[{i + 1}] {menuOptions[i]}");
                }
                Console.WriteLine($"[0] {(menuCode != 0 ? "Back" : "Exit")}");

                Console.Write("Enter your command: ");
                command = Console.ReadLine().Trim();

                if (IsValidCommand(command, menuOptions.Length))
                {
                    selected = Convert.ToInt32(command);

                    if (menuCode == 2 && selected != 0)
                    {
                        // Temporary only
                        cards = new Card[5];
                        Console.WriteLine();
                    }

                    switch (selected)
                    {
                        case 1:
                            switch (menuCode)
                            {
                                case 0:
                                    // Main Menu > Play Game
                                    MenuNavigation(1);
                                    break;
                                case 1:
                                    // Main Menu > Play Game > One Player - Pending
                                    break;
                                case 2:
                                    // Main Menu > Generate Cards > Random Cards Generation
                                    RandomGeneratedCards();
                                    break;
                            }
                            break;
                        case 2:
                            switch (menuCode)
                            {
                                case 0:
                                    // Main Menu > Generate Cards
                                    MenuNavigation(2);
                                    break;
                                case 1:
                                    // Main Menu > Play Game > Multiplayer - Pending
                                    break;
                                case 2:
                                    // Main Menu > Generate Cards > Random Cards Selection
                                    RandomCardSelection();
                                    break;
                            }
                            break;
                        case 3:
                            switch (menuCode)
                            {
                                case 2:
                                    // Main Menu > Generate Cards > Manual Cards Entry
                                    ManualCardEntry();
                                    break;
                            }
                            break;
                    }

                    if (menuCode == 2 && selected != 0) ShowHandResults();  // Temporary only
                }

                // Screen ends once the command entered is 0.
            } while (selected != 0);
        }
        #endregion

        #region Play Game

        #endregion

        #region Generate Cards
        // Function to generate five random cards.
        private static void RandomGeneratedCards()
        {
            cards = SetCardDeck(true);
        }

        // Function to select five random cards.
        private static void RandomCardSelection()
        {
            Card[] cardDeck = SetCardDeck();
            string selectedNumbers = "";

            for (int i = 0; i < cards.Length; i++)
            {
                string selection = "";
                bool isValid = false;

                while (!isValid)
                {
                    Console.Write($"Enter a card number from 1 to 52 for Card #{i + 1}: ");
                    selection = Console.ReadLine().Trim();

                    if (selection == "")
                    {
                        Console.WriteLine("ERROR: Please enter your card number.\n");
                        continue;
                    }
                    else if (!int.TryParse(selection, out _))
                    {
                        Console.WriteLine("ERROR: Invalid card number.\n");
                        continue;
                    }
                    else if (Convert.ToInt32(selection) < 1 || Convert.ToInt32(selection) > 52)
                    {
                        Console.WriteLine("ERROR: Please enter a value from 1 to 52.\n");
                        continue;
                    }
                    else if (selectedNumbers.Contains($"|{selection}|"))
                    {
                        Console.WriteLine("ERROR: Card number already selected.\n");
                        continue;
                    }

                    isValid = true;
                }

                selectedNumbers = $"{(selectedNumbers.Length > 0 ? selectedNumbers : "|")}{selection}|";
                cards[i] = cardDeck[Convert.ToInt32(selection) - 1];
                Console.WriteLine();
            }
        }

        // Function to enter five cards manually.
        private static void ManualCardEntry()
        {
            Console.WriteLine("VALID CARD VALUES: 1-13 (1 = A; 11 = J; 12 = Q; 13 = K)");
            Console.WriteLine("VALID CARD SUITS: C (Club), D (Diamond), H (Heart), S (Spade)");
            Console.WriteLine();

            for (int i = 0; i < cards.Length; i++)
            {
                string cardValue, cardSuit;

                Console.Write($"Enter card value for Card #{i + 1}: ");
                cardValue = Console.ReadLine().Trim();

                Console.Write($"Enter card suit for Card #{i + 1}: ");
                cardSuit = Console.ReadLine().ToUpper().Trim();

                if (ValidateCard(cardValue, cardSuit))
                {
                    // Add Card object to the array.
                    cards[i] = new Card(ConvertValue(Convert.ToInt32(cardValue)), cardSuit[0].ToString());
                }
                else
                {
                    // If invalid, decrement the counter to try entering the current card again.
                    i--;
                }

                Console.WriteLine();
            }
        }
        #endregion

        #region Generic Methods
        // Function to set card deck in a randomized order.
        private static Card[] SetCardDeck(bool isGenerated = false)
        {
            string cardDeckInput = "";
            int length = isGenerated ? 5 : 52;
            Card[] cardDeck = new Card[length];

            for (int i = 0; i < length; i++)
            {
                string card = $"{ConvertValue((rand.Next(1, 14)))}|{ConvertSuit(rand.Next(1, 5))}";

                if (i == 0)
                {
                    // First card
                    cardDeckInput = card;
                }
                else
                {
                    // Checking for duplicates
                    if (!cardDeckInput.Contains(card))
                    {
                        // Append the card
                        cardDeckInput = $"{cardDeckInput}{card}";
                    }
                    else
                    {
                        // Try again if duplicate found
                        i--;
                        continue;
                    }
                }

                cardDeck[i] = new Card(card.Split('|')[0], card.Split('|')[1]);
            }

            return cardDeck;
        }

        // Function to display hand results after card generation.
        private static void ShowHandResults()
        {
            Console.Write("Your cards: ");
            foreach (Card card in cards)
            {
                Console.Write($"{card.CardValue}{card.CardSuit} ");
            }

            Console.WriteLine($"({ConvertPokerHand(RankPokerHand())})");
            Console.ReadKey();
        }
        #endregion

        #region Poker Hand Methods
        // Function to determine the poker hand.
        private static int RankPokerHand()
        {
            bool isStraight = CheckForStraight();
            bool isFlush = CheckForFlush();
            string[] valueCounters = CountValueCards();

            if (isStraight && isFlush)
            {
                // Royal Flush (if highest card is A) or Straight Flush
                return (GetCardHierarchy(isStraight, valueCounters)[0] == "A") ? 10 : 9;
            }
            else if (CheckForMultiCards(valueCounters, 4) == 1)
            {
                // Four of a Kind
                return 8;
            }
            else if (CheckForMultiCards(valueCounters, 3) == 1 && CheckForMultiCards(valueCounters, 2) == 1)
            {
                // Full House
                return 7;
            }
            else if (isFlush)
            {
                // Flush
                return 6;
            }
            else if (isStraight)
            {
                // Straight
                return 5;
            }
            else if (CheckForMultiCards(valueCounters, 3) == 1)
            {
                // Three of a Kind
                return 4;
            }
            else if (CheckForMultiCards(valueCounters, 2) == 2)
            {
                // Two Pairs
                return 3;
            }
            else if (CheckForMultiCards(valueCounters, 2) == 1)
            {
                // One Pair
                return 2;
            }
            else
            {
                // High Card
                return 1;
            }
        }

        // Function to check if the hand is straight.
        private static bool CheckForStraight()
        {
            int currentValue = 0, endValue = 0;

            // Check if there's an ace (can be the lowest or highest).
            if (SearchCardValue("A"))
            {
                if (SearchCardValue("2"))
                {
                    // A and 2 are in the hand.
                    currentValue = 3;
                    endValue = 5;
                }
                else if (SearchCardValue("K"))
                {
                    // A and K are in the hand.
                    currentValue = 10;
                    endValue = 12;
                }
                else
                {
                    // Since ace has no consecutive card value, it is automatically not a straight.
                    return false;
                }
            }
            else
            {
                // If there's no ace, search for the lowest card value.
                for (int i = 2; i <= 13; i++)
                {
                    if (SearchCardValue(ConvertValue(i)))
                    {
                        currentValue = i + 1;
                        endValue = i + 4;
                        break;
                    }
                }
            }

            while (currentValue <= endValue)
            {
                // Loop ends if the next expected card value didn't match or all five cards are in a succession.
                if (SearchCardValue(ConvertValue(currentValue)))
                {
                    currentValue++;
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        // Function to check if the hand is flush.
        private static bool CheckForFlush()
        {
            // Get the suit from the first card.
            string suit = cards[0].CardSuit;

            foreach (Card card in cards)
            {
                // If there's a mismatch, it is not a flush.
                if (card.CardSuit != suit) return false;
            }

            return true;
        }

        // Function to count number of cards with that value.
        private static string[] CountValueCards()
        {
            string valueCounters = "";
            int foundCards = 0;

            // Count cards per value.
            for (int i = 1; i <= 13; i++)
            {
                int valueCount = 0;  // Initial/reset value
                string cardValue = ConvertValue(i);

                foreach (Card card in cards)
                {
                    if (card.CardValue == cardValue)
                    {
                        valueCount++;  // Current card value to be searched
                        foundCards++;  // Cards searched
                    }
                }

                // Add only if there's at least one card value found.
                if (valueCount > 0)
                {
                    valueCounters = $"{(valueCounters.Length > 0 ? $"{valueCounters}," : "")}{cardValue}|{valueCount}";
                }

                // Break the loop once all five cards are searched.
                if (foundCards == 5) break;
            }

            return valueCounters.Split(',');
        }

        // Function to get the hierarchy of card values.
        private static string[] GetCardHierarchy(bool isStraight, string[] valueCounters)
        {
            string cardValueList = "";
            string valueCountJoined = string.Join(',', valueCounters);
            int totalCards = 0;

            for (int i = 4; i >= 1; i--)
            {
                // Skip to the next quantity if none found.
                if (CheckForMultiCards(valueCounters, i) == 0) continue;

                // Ace is the highest if there's an ace and it is not straight, or it is straight and has ace and king.
                if ((!isStraight && valueCountJoined.Contains($"A|{i}")) ||
                    (isStraight && valueCountJoined.Contains("A|1") && valueCountJoined.Contains("K|1")))
                {
                    cardValueList = $"{(cardValueList.Length > 0 ? $"{cardValueList}," : "")}A";
                    totalCards += i;

                    if (totalCards == 5) break;  // Stop the loop if all cards are found.

                    // Skip to the next quantity if there are more than two set of same cards found.
                    if (i > 2) continue;
                }

                // Getting the card value from King to Ace (given the conditions above weren't satisfied).
                for (int j = 13; j >= 1; j--)
                {
                    string cardValue = ConvertValue(j);

                    if (valueCountJoined.Contains($"{cardValue}|{i}"))
                    {
                        cardValueList = $"{(cardValueList.Length > 0 ? $"{cardValueList}," : "")}{cardValue}";
                        totalCards += i;

                        if (totalCards == 5) goto EndLoop;  // Stop the loop if all cards are found.

                        // Skip to the next quantity if there are more than two set of same cards found
                        // or the hand has two pairs and the last card is yet to be scanned.
                        if (i > 2 || (i == 2 && totalCards == 4)) break;
                    }
                }
            }
        EndLoop:
            return cardValueList.Split(',');
        }

        // Function to check for possible pairs, threes or fours.
        private static int CheckForMultiCards(string[] valueCounters, int valueCount)
        {
            int totalCount = 0;

            foreach (string cardValueCount in valueCounters)
            {
                if (Convert.ToInt32(cardValueCount.Split('|')[1]) == valueCount) totalCount++;
            }

            return totalCount;
        }

        // Function to search cards with that value.
        private static bool SearchCardValue(string value)
        {
            foreach (Card card in cards)
            {
                if (card.CardValue == value) return true;
            }

            return false;
        }
        #endregion

        #region Validations
        // Function to check if the command entered is valid.
        private static bool IsValidCommand(string command, int menuOptions)
        {
            if (command == "")
            {
                Console.WriteLine("ERROR: Please enter your command.");
                return false;
            }
            else if (!int.TryParse(command, out _))
            {
                Console.WriteLine("ERROR: Invalid command.");
                return false;
            }
            else if (Convert.ToInt32(command) < 0 || Convert.ToInt32(command) > menuOptions)
            {
                Console.WriteLine("ERROR: Command must be one of the options above.");
                return false;
            }

            return true;
        }

        // Function to validate cards.
        private static bool ValidateCard(string value, string suit)
        {
            bool isValid = true;

            // Card values
            if (value == "")
            {
                isValid = false;
                Console.WriteLine("ERROR: Card value cannot be blank.");
            }
            else if (!int.TryParse(value, out _))
            {
                isValid = false;
                Console.WriteLine("ERROR: Invalid card value.");
            }
            else if (Convert.ToInt32(value) < 1 || Convert.ToInt32(value) > 13)
            {
                isValid = false;
                Console.WriteLine("ERROR: Please enter a card value between 1 and 13.");
            }

            // Card suits
            if (suit == "")
            {
                isValid = false;
                Console.WriteLine("ERROR: Card suit cannot be blank.");
            }
            else if (suit[0] != 'C' && suit[0] != 'D' && suit[0] != 'H' && suit[0] != 'S')
            {
                isValid = false;
                Console.WriteLine("ERROR: Please enter a valid card suit.");
            }

            // If both card values and suits are valid.
            if (isValid)
            {
                foreach (Card card in cards)
                {
                    if (!(card is null))
                    {
                        // If the entered card has the same existing card.
                        if ($"{ConvertValue(Convert.ToInt32(value))}{suit[0]}" == $"{card.CardValue}{card.CardSuit}")
                        {
                            isValid = false;
                            Console.WriteLine("ERROR: Card entered already exists.");
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return isValid;
        }
        #endregion

        #region Value Conversions
        // Function to convert numbers to face cards or Ace.
        private static string ConvertValue(int num)
        {
            switch (num)
            {
                case 1:
                    return "A";
                case 11:
                    return "J";
                case 12:
                    return "Q";
                case 13:
                    return "K";
                default:
                    return num.ToString();
            }
        }

        // Function to convert numeric value to suit characters.
        private static char ConvertSuit(int num)
        {
            switch (num)
            {
                case 1:
                    return 'C';
                case 2:
                    return 'D';
                case 3:
                    return 'H';
                case 4:
                    return 'S';
                default:
                    return 'X';
            }
        }

        // Function to convert numeric value to suit characters.
        private static string ConvertPokerHand(int num)
        {
            switch (num)
            {
                case 10:
                    return "Royal Flush";
                case 9:
                    return "Straight Flush";
                case 8:
                    return "Four of a Kind";
                case 7:
                    return "Full House";
                case 6:
                    return "Flush";
                case 5:
                    return "Straight";
                case 4:
                    return "Three of a Kind";
                case 3:
                    return "Two Pairs";
                case 2:
                    return "One Pair";
                case 1:
                    return "High Card";
                default:
                    return "Unknown";
            }
        }
        #endregion
    }
}