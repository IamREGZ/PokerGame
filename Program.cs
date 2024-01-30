using System;

namespace PokerGame
{
    class Program
    {
        static Card[] cards;
        static readonly Random rand = new Random();

        static void Main(string[] args)
        {
            Console.WriteLine("POKER GAME\n----------");

            MenuNavigation(0);
        }  // end Main

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
        }  // end MenuNavigation

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
                Console.WriteLine("ERROR: Command must be a numeric value.");
                return false;
            }
            else if (Convert.ToInt32(command) < 0 || Convert.ToInt32(command) > menuOptions)
            {
                Console.WriteLine("ERROR: Command must be one of the options above.");
                return false;
            }

            return true;
        }  // end IsValidCommand

        // Function to generate five random cards.
        private static void RandomGeneratedCards()
        {
            cards = SetCardDeck(true);
        }  // end RandomGeneratedCards

        // Function to select five random cards.
        private static void RandomCardSelection()
        {
            Card[] cardDeck = SetCardDeck();
            string selectedNumbers = "";

            for (int i = 0; i < cards.Length; i++)
            {
                string selection = "";
                bool isValid = false;

                while(!isValid)
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
        }  // end RandomCardSelection

        // Function to set card deck in a randomized order.
        private static Card[] SetCardDeck(bool isGenerated = false)
        {
            string cardDeckInput = "";
            int length = isGenerated ? 5 : 52;
            Card[] cardDeck = new Card[length];

            for (int i = 0; i < length; i++)
            {
                string card = $"{ConvertValue((rand.Next(1, 14)).ToString())}|{ConvertSuit(rand.Next(1, 5))}";

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
        }  // end SetCardDeck

        // Function to enter five cards manually.
        private static void ManualCardEntry()
        {
            Console.WriteLine("VALID CARD VALUES: 1-13 (1 = A; 11 = J; 12 = Q; 13 = K)");
            Console.WriteLine("VALID CARD SUITS: C (Club), D (Diamond), H (Heart), S (Spade)");

            for (int i = 0; i < 5; i++)
            {
                bool isValid = false;

                while (!isValid)
                {
                    string cardValue, cardSuit;

                    Console.Write($"Enter card value for Card #{i + 1}: ");
                    cardValue = Console.ReadLine().Trim();

                    Console.Write($"Enter card suit for Card #{i + 1}: ");
                    cardSuit = Console.ReadLine().ToUpper().Trim();

                    isValid = ValidateCard(cardValue, cardSuit);

                    if (isValid)
                    {
                        // Add Card object to the array.
                        cards[i] = new Card(ConvertValue(cardValue), cardSuit[0].ToString());
                    }

                    Console.WriteLine();
                }
            }
        }  // end ManualCardEntry

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
                Console.WriteLine("ERROR: Please enter a numeric card value.");
            }
            else if (Convert.ToInt32(value) < 1 || Convert.ToInt32(value) > 13)
            {
                isValid = false;
                Console.WriteLine("ERROR: Please enter a number between 1 and 13.");
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
                        if ($"{ConvertValue(value)}{suit[0]}" == $"{card.GetCardValue()}{card.GetCardSuit()}")
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
        }  // end ValidateCard

        // Function to display hand results after card generation.
        private static void ShowHandResults()
        {
            Console.Write("Your cards: ");
            foreach (Card card in cards)
            {
                Console.Write($"{card.GetCardValue()}{card.GetCardSuit()} ");
            }

            Console.Write($"({RankPokerHand()})");
            Console.ReadLine();
        }  // end ShowHandResults

        // Function to determine the poker hand.
        private static string RankPokerHand()
        {
            bool isStraight = CheckForStraight();
            bool isFlush = CheckForFlush();
            string[] valueCounters = CountValueCards();
            string highestCard = GetCardHierarchy(isStraight, valueCounters)[0];

            if (isStraight && isFlush)
            {
                // 1. Royal Flush - straight flush + highest card is A
                // 2. Straight Flush - combination of straight and flush
                return (highestCard == "A") ? "Royal Flush" : "Straight Flush";
            }
            else if (CheckForMultiCards(valueCounters, 4) == 1)
            {
                // 3. Four of a Kind - hand with four cards with same values
                return "Four of a Kind";
            }
            else if (CheckForMultiCards(valueCounters, 3) == 1 && CheckForMultiCards(valueCounters, 2) == 1)
            {
                // 4. Full House - three cards with same values + two cards with same values
                return "Full House";
            }
            else if (isFlush)
            {
                // 5. Flush - all cards have one common suit
                return "Flush";
            }
            else if (isStraight)
            {
                // 6. Straight - all cards in a succession (A-5 up to 10-A)
                return "Straight";
            }
            else if (CheckForMultiCards(valueCounters, 3) == 1)
            {
                // 7. Three of a Kind - hand with three cards with same values
                return "Three of a Kind";
            }
            else if (CheckForMultiCards(valueCounters, 2) == 2)
            {
                // 8. Two Pairs - two sets of paired cards
                return "Two Pairs";
            }
            else if (CheckForMultiCards(valueCounters, 2) == 1)
            {
                // 9. One Pair - one set of paired cards
                return "One Pair";
            }
            else
            {
                // 10. High Card - get the highest card
                return $"High Card - {highestCard}";
            }
        }  // end RankPokerHand

        // Function to check if the hand is straight.
        private static bool CheckForStraight()
        {
            int currentValue = 0;  // Starting value + 1.
            int endValue = 0;  // Expected highest value.

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
                    if (SearchCardValue(ConvertValue(i.ToString())))
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
                if (SearchCardValue(ConvertValue(currentValue.ToString())))
                {
                    currentValue++;
                }
                else
                {
                    return false;
                }
            }

            return true;
        }  // end CheckForStraight

        // Function to check if the hand is flush.
        private static bool CheckForFlush()
        {
            // Get the suit from the first card.
            string suit = cards[0].GetCardSuit();

            foreach (Card card in cards)
            {
                // If there's a mismatch, it is not a flush.
                if (card.GetCardSuit() != suit) return false;
            }

            return true;
        }  // end CheckForFlush

        // Function to count number of cards with that value.
        private static string[] CountValueCards()
        {
            string[] valueCounters = { "", "", "", "", "" };
            int valueIndex = 0, foundCards = 0;

            // Count cards per value.
            for (int i = 1; i <= 13; i++)
            {
                int valueCount = 0;  // Initial/reset value
                string cardValue = ConvertValue(i.ToString());

                foreach (Card card in cards)
                {
                    if (card.GetCardValue() == cardValue)
                    {
                        valueCount++;  // Current card value to be searched
                        foundCards++;  // Cards searched
                    }
                }

                // Add only if there's at least one card value found.
                if (valueCount > 0)
                {
                    valueCounters[valueIndex] = $"{cardValue}|{valueCount}";
                    valueIndex++;
                }

                // Break the loop once all five cards are searched.
                if (foundCards == 5) break;
            }

            return valueCounters;
        }  // end CountValueCards

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
                    string cardValue = ConvertValue(j.ToString());
  
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
        }  // end GetCardHierarchy

        // Function to check for possible pairs, threes or fours.
        private static int CheckForMultiCards(string[] valueCounters, int valueCount)
        {
            int totalCount = 0;

            foreach (string cardValueCount in valueCounters)
            {
                if (cardValueCount == "") break;

                if (Convert.ToInt32(cardValueCount.Split('|')[1]) == valueCount) totalCount++;
            }

            return totalCount;
        }  // end CheckForMultiCards

        // Function to search cards with that value.
        private static bool SearchCardValue(string value)
        {
            foreach (Card card in cards)
            {
                if (card.GetCardValue() == value) return true;
            }

            return false;
        }  // end SearchCardValue

        // Function to convert numbers to face cards or Ace.
        private static string ConvertValue(string value)
        {
            switch(value)
            {
                case "1":
                    return "A";
                case "11":
                    return "J";
                case "12":
                    return "Q";
                case "13":
                    return "K";
                default:
                    return value;
            }
        }  // end ConvertValue

        // Function to convert numeric value to suit characters.
        private static string ConvertSuit(int num)
        {
            switch(num)
            {
                case 1:
                    return "C";
                case 2:
                    return "D";
                case 3:
                    return "H";
                case 4:
                    return "S";
                default:
                    return "X";
            }
        }  // end ConvertSuit
    }  // end Program
}  // end PokerGame
