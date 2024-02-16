using System;
using System.Linq;
using System.Collections.Generic;

namespace PokerGame
{
    class Program
    {
        #region Fields
        static readonly Random rand = new Random();
        static bool isMultiplayer = false;
        #endregion

        #region Main Program
        static void Main(string[] args)
        {
            Console.WriteLine("POKER GAME\n----------");

            MenuNavigation(0);
        }

        // Method to navigate the console to different screens.
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
                    menuOptions = new string[2] { "One Player - WIP", "Multiplayer - WIP" };
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

                if (ValidateCommand(command, menuOptions.Length))
                {
                    selected = Convert.ToInt32(command);

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
                                    // Main Menu > Play Game > One Player - WIP
                                    isMultiplayer = false;
                                    GamePlay();
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
                                    // Main Menu > Play Game > Multiplayer - WIP
                                    isMultiplayer = true;
                                    GamePlay();
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
                }

                // Screen ends once the command entered is 0.
            } while (selected != 0);
        }
        #endregion

        #region Play Game
        // Method to process the whole gameplay.
        private static void GamePlay()
        {
            Console.WriteLine();

            Rules rules = new Rules();
            rules.SetRules(isMultiplayer);

            int[] playerWins = new int[rules.Players];
            int roundNumber = 1;
            bool gameFinished = false;

            // Players initialization
            Player[] players = CreatePlayers(rules.Players, rules.Humans);

            while (!gameFinished)
            {
                Console.WriteLine($"ROUND {roundNumber}");

                // Shuffling of card deck
                Card[] cardDeck = SetCardDeck();

                // Card selection
                for (int i = 0; i < 5; i++)
                {
                    foreach (Player player in players)
                    {
                        while (true)
                        {
                            Card currentCard = SelectCard(ref cardDeck, i, player);

                            if (!(currentCard is null))
                            {
                                player.SetCard(i, currentCard);
                                break;
                            }
                        }
                    }
                }

                // Winner determination
                int[] winningIndices = DetermineWinners(players);

                // Displaying the winner(s)
                string winningNames = "";

                // It is possible to have more than one winners
                foreach (int i in winningIndices)
                {
                    winningNames = $"{(winningNames.Length > 0 ? $"{winningNames}, " : "")}{players[i].Name}";
                }

                Console.WriteLine($"Winner{(winningIndices.Length > 1 ? "s" : "")} of this round: {winningNames}");

                Console.ReadLine();

                // Adding the point to the winner(s)
                foreach (int index in winningIndices)
                {
                    players[index].Wins++;
                    playerWins[index] = players[index].Wins;
                }

                // Displaying the statistics
                Console.WriteLine($"STATISTICS (Race to {rules.MaxWins})");

                foreach (Player player in players)
                {
                    Console.WriteLine($"{player.Name}: {player.Wins}");
                }

                Console.ReadLine();

                roundNumber++;

                // Game is over if a player gets the target number of wins or more, as long as the lead is at least one.
                gameFinished = playerWins.Max() >= rules.MaxWins && playerWins.Count(x => x == playerWins.Max()) == 1;

                // Rearrange only if the game is not yet finished.
                if (!gameFinished) players = ReorderPlayers(players);
            }

            Console.WriteLine($"GAME OVER: {players[Array.IndexOf(playerWins, playerWins.Max())].Name} " +
                "wins the game!");

            isMultiplayer = false;  // Reset to default value after the game.
        }

        // Method to initialize players.
        private static Player[] CreatePlayers(int numPlayers, int numHumans)
        {
            Player[] players = new Player[numPlayers];

            List<string> nameList = new string[] { "Alex", "Anne", "Ben", "Belle", "Chris", "Carla", "Daniel", "Donna", "Edward", "Eve", "Frank", "Faye", "George", "Gina", "Harold", "Hailey", "Ivan", "Ingrid", "Josh", "Josie", "Kevin", "Kath", "Leo", "Louise", "Mark", "Maddie", "Nathan", "Nina", "Oliver", "Olga", "Paul", "Pearl", "Quincy", "Queenie", "Ron", "Rita", "Steven", "Shane", "Ted", "Tricia", "Ulysses", "Ursula", "Victor", "Vera", "Willy", "Wanda", "Xavier", "Xandra", "Young", "Yelena", "Zack", "Zoe" }.ToList();

            string opponents = "";
            
            // Player creation
            for (int i = 0; i < players.Length; i++)
            {
                if (i < numHumans)
                {
                    Console.Write($"Enter player name{(isMultiplayer ? $" for Player #{i + 1}" : "")}: ");
                    players[i] = new Human(Console.ReadLine().Trim(), i + 1);
                }
                else
                {
                    players[i] = new Computer(ref nameList);
                    opponents = $"{(opponents.Length > 0 ? $"{opponents}, " : "")}{players[i].Name}";
                }
            }

            // Display only if there are any computers.
            if (opponents != "") Console.WriteLine($"Opponents: {opponents}");

            return ReorderPlayers(players, true);
        }

        // Method to rearrange the first player in the round as the last player.
        private static Player[] ReorderPlayers(Player[] players, bool isStart = false)
        {
            if (isStart)
            {
                players = DeterminePlayerOrder(players);
            }
            else
            {
                // The first player in the round will be the last one to play in the next round.
                Player tempPlayer = players[0];
                Array.Copy(players, 1, players, 0, players.Length - 1);
                players[^1] = tempPlayer;
            }

            return players;
        }

        // Method to determine playing order based on the card value selected.
        private static Player[] DeterminePlayerOrder(Player[] players)
        {
            int length = players.Length, currentIndex = 0;
            Player[] currentPlayerList = players;

            Player[] orderedPlayers = new Player[length];
            List<string> cardValues = new List<string>();
            List<Player[]> playerGroup = new List<Player[]>(), tempGroup = new List<Player[]>();

            Card[] cardDeck = SetCardDeck();

            Console.WriteLine("\nORDER DETERMINATION");

            while (currentIndex != -1)
            {
                // Card selection
                foreach (Player player in currentPlayerList)
                {
                    while (true)
                    {
                        Card currentCard = SelectCard(ref cardDeck, player: player, isStart: true);

                        if (!(currentCard is null))
                        {
                            cardValues.Add(currentCard.CardValue);
                            break;
                        }
                    }
                }

                // Displaying selected card values
                for (int i = 0; i < length; i++)
                {
                    Console.Write($"{currentPlayerList[i].Name}\'s card value is {cardValues[i]}.");
                    Console.ReadLine();
                }
                
                // Grouping players by card value
                if (cardValues.Contains("A"))
                {
                    playerGroup.Add(GroupPlayersByValue("A", currentPlayerList, cardValues));
                }

                for (int j = 13; j > 1; j--)
                {
                    if (cardValues.Contains(ConvertValue(j)))
                    {
                        playerGroup.Add(GroupPlayersByValue(ConvertValue(j), currentPlayerList, cardValues));
                    }

                    // Stopping the loop after all players are grouped.
                    if (playerGroup.Sum(x => x.Length) == length) break;
                }

                // Adding the remaining groups from the earlier list
                playerGroup.AddRange(tempGroup);
                
                // Ordering the players
                foreach (Player[] group in playerGroup)
                {
                    if (group.Length == 1)
                    {
                        orderedPlayers[currentIndex] = group[0];
                    }

                    currentIndex += group.Length;
                }

                Console.WriteLine("\nPLAYING ORDER");

                int counter = 0, groupIndex = 0;
                List<Player[]> tempTiedGroups = playerGroup.FindAll(x => x.Length > 1);

                // Displaying the playing order
                while (counter < orderedPlayers.Length)
                {
                    if (!(orderedPlayers[counter] is null))
                    {
                        Console.WriteLine($"{counter + 1}. {orderedPlayers[counter].Name}");
                        counter++;
                    }
                    else
                    {
                        Player[] tiedGroup = tempTiedGroups[groupIndex];

                        foreach (Player player in tiedGroup)
                        {
                            Console.WriteLine($"{counter + 1}. {player.Name} (TIE)");
                        }

                        counter += tiedGroup.Length;
                        groupIndex++;
                    }
                }

                Console.ReadLine();

                // Re-assigning variables
                playerGroup.RemoveAll(x => x.Length == 1);
                currentPlayerList = playerGroup.Count() > 0 ? playerGroup[0] : Array.Empty<Player>();
                length = currentPlayerList.Length;
                tempGroup = playerGroup.Skip(1).ToList<Player[]>();

                // Clearing lists
                playerGroup.Clear();
                cardValues.Clear();

                // Loop ends if all players have their definite playing orders (no ties).
                currentIndex = Array.IndexOf(orderedPlayers, null);
            }

            return orderedPlayers;
        }

        // Method to group players by card value for determining the playing order.
        private static Player[] GroupPlayersByValue(string value, Player[] players, List<string> cardValues)
        {
            List<Player> groupedPlayers = new List<Player>();

            for (int i = 0; i < players.Length; i++)
            {
                if (cardValues[i] == value) groupedPlayers.Add(players[i]);
            }

            return groupedPlayers.ToArray();
        }

        // Method to determine who is the winner of the round.
        private static int[] DetermineWinners(Player[] players)
        {
            int[] handRankings = new int[players.Length];
            List<int> winningIndices = new List<int>();
            int highestRank;

            // Display results
            for (int i = 0; i < players.Length; i++)
            {
                handRankings[i] = RankPokerHand(players[i].Hand);

                ShowHandResults(players[i].Hand, handRankings[i], players[i]);
            }

            highestRank = handRankings.Max();  // Determining the highest rank

            // Getting all players who got the highest rank
            for (int i = 0; i < handRankings.Length; i++)
            {
                if (handRankings[i] == highestRank)
                {
                    winningIndices.Add(i);
                }
            }

            if (winningIndices.Count() > 1)
            {
                winningIndices = Tiebreaker(winningIndices, players, new int[] { 10, 9, 5 }.Contains(highestRank));
            }

            return winningIndices.ToArray<int>();
        }

        // Method to break a tie between two or more players with the best hands.
        private static List<int> Tiebreaker(List<int> indices, Player[] players, bool isStraight)
        {
            List<string[]> cardHierarchies = new List<string[]>();
            List<bool> playerWins = new List<bool>();
            int currentIndex = 0;

            // Store initial values
            foreach (int index in indices)
            {
                cardHierarchies.Add(GetCardHierarchy(isStraight, CountValueCards(players[index].Hand)));
                playerWins.Add(false);
            }

            // Loop ends when there's only one player remaining or the winning index reached the last card.
            while (cardHierarchies.Count() > 1 && currentIndex < cardHierarchies[0].Length)
            {
                // Check if the players have ace in the current card.
                playerWins = HasTheHighestCard("A", playerWins, cardHierarchies, currentIndex);

                // If none found, proceed checking from King to 2.
                if (playerWins.Count(x => x) == 0)
                {
                    for (int i = 13; i > 1; i--)
                    {
                        playerWins = HasTheHighestCard(ConvertValue(i), playerWins, cardHierarchies, currentIndex);

                        // Stop finding if at least one player has the winning card value.
                        if (playerWins.Count(x => x) > 0) break;
                    }
                }

                // Eliminating players that don't have the winning card value.
                if (playerWins.Count(x => x) > 0)
                {
                    for (int j = cardHierarchies.Count() - 1; j >= 0; j--)
                    {
                        if (!playerWins[j])
                        {
                            indices.RemoveAt(j);
                            cardHierarchies.RemoveAt(j);
                            playerWins.RemoveAt(j);
                        }
                    }
                }

                currentIndex++;
            }

            return indices;
        }

        // Method to determine if each player has the winning card value.
        private static List<bool> HasTheHighestCard(string value, List<bool> playerWins,
            List<string[]> hierarchies, int currentIndex)
        {
            for (int i = 0; i < playerWins.Count(); i++)
            {
                playerWins[i] = hierarchies[i][currentIndex] == value;
            }

            return playerWins;
        }
        #endregion

        #region Generate Cards
        // Method to generate five random cards.
        private static void RandomGeneratedCards()
        {
            Card[] cardHand = SetCardDeck(true);

            Console.WriteLine();
            ShowHandResults(cardHand, RankPokerHand(cardHand));
        }

        // Method to select five random cards.
        private static void RandomCardSelection()
        {
            Card[] cardHand = new Card[5], cardDeck = SetCardDeck();

            Console.WriteLine();

            for (int i = 0; i < cardHand.Length; i++)
            {
                Card currentCard = SelectCard(ref cardDeck, i);

                if (!(currentCard is null))
                {
                    // Add Card object to the array.
                    cardHand[i] = currentCard;
                }
                else
                {
                    // If invalid, decrement the counter to try selecting the current card again.
                    i--;
                }
            }

            ShowHandResults(cardHand, RankPokerHand(cardHand));
        }

        // Method to enter five cards manually.
        private static void ManualCardEntry()
        {
            Card[] cardHand = new Card[5];

            Console.WriteLine();
            Console.WriteLine("VALID CARD VALUES: 1-13 (1 = A; 11 = J; 12 = Q; 13 = K)");
            Console.WriteLine("VALID CARD SUITS: C (Club), D (Diamond), H (Heart), S (Spade)\n");

            for (int i = 0; i < cardHand.Length; i++)
            {
                string cardValue, cardSuit;

                Console.Write($"Enter card value for Card #{i + 1}: ");
                cardValue = Console.ReadLine().Trim();

                Console.Write($"Enter card suit for Card #{i + 1}: ");
                cardSuit = Console.ReadLine().ToUpper().Trim();

                if (ValidateCard(cardValue, cardSuit, cardHand))
                {
                    // Add Card object to the array.
                    cardHand[i] = new Card(ConvertValue(Convert.ToInt32(cardValue)), cardSuit[0].ToString());
                }
                else
                {
                    // If invalid, decrement the counter to try entering the current card again.
                    i--;
                }

                Console.WriteLine();
            }

            ShowHandResults(cardHand, RankPokerHand(cardHand));
        }
        #endregion

        #region Generic Methods
        // Method to set card deck in a randomized order.
        private static Card[] SetCardDeck(bool isHand = false)
        {
            string cardDeckInput = "";
            Card[] cardDeck = new Card[isHand ? 5 : 52];

            for (int i = 0; i < cardDeck.Length; i++)
            {
                string card = $"{ConvertValue((rand.Next(1, 14)))}|{ConvertSuit(rand.Next(1, 5))}";

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

                cardDeck[i] = new Card(card.Split('|')[0], card.Split('|')[1]);
            }

            return cardDeck;
        }

        // Method to process card selection.
        private static Card SelectCard(ref Card[] cardDeck, int index = 0, Player player = null, bool isStart = false)
        {
            Card selectedCard = null;

            if (player is null || player is Human)
            {
                string selection;

                Console.Write($"{(isMultiplayer ? $"({player.Name}) " : "")}Enter card number{(!isStart ? $" for Card #{index + 1}" : "")} (1-52): ");
                selection = Console.ReadLine().Trim();

                if (ValidateSelectCard(selection, cardDeck))
                {
                    int selected = Convert.ToInt32(selection) - 1;
                    selectedCard = new Card(cardDeck[selected].CardValue, cardDeck[selected].CardSuit);

                    cardDeck[selected] = MarkAsSelected(cardDeck[selected]);
                }

                Console.WriteLine();
            }
            else
            {
                int cardNum = rand.Next(0, cardDeck.Length);
                selectedCard = new Card(cardDeck[cardNum].CardValue, cardDeck[cardNum].CardSuit);

                if (!CardAlreadySelected(selectedCard))
                {
                    cardDeck[cardNum] = MarkAsSelected(cardDeck[cardNum]);

                    Console.WriteLine($"{player.Name} selected Card #{cardNum + 1}");

                    Console.ReadLine();
                }
                else
                {
                    return null;
                }
            }

            return selectedCard;
        }

        // Method to check if the card in a deck is already selected.
        private static bool CardAlreadySelected(Card card)
        {
            return $"{card.CardValue}{card.CardSuit}" == "XX";
        }

        // Method to mark the card as selected.
        private static Card MarkAsSelected(Card selectedCard)
        {
            selectedCard.CardValue = "X";
            selectedCard.CardSuit = "X";

            return selectedCard;
        }

        // Method to display hand results after card generation.
        private static void ShowHandResults(Card[] cardHand, int rankHand, Player player = null)
        {
            Console.Write($"{(isMultiplayer || player is Computer ? $"{player.Name}\'s" : "Your")} cards: ");

            foreach (Card card in cardHand)
            {
                Console.Write($"{card.CardValue}{card.CardSuit} ");
            }

            Console.Write($"({ConvertPokerHand(rankHand)})");
            Console.ReadLine();
        }
        #endregion

        #region Poker Hand Methods
        // Method to determine the poker hand.
        private static int RankPokerHand(Card[] cardHand)
        {
            bool isStraight = CheckForStraight(cardHand);
            bool isFlush = CheckForFlush(cardHand);
            string[] valueCounters = CountValueCards(cardHand);

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

        // Method to check if the hand is straight.
        private static bool CheckForStraight(Card[] cardHand)
        {
            int currentValue = 0, endValue = 0;

            // Check if there's an ace (can be the lowest or highest).
            if (SearchCardValue("A", cardHand))
            {
                if (SearchCardValue("2", cardHand))
                {
                    // A and 2 are in the hand.
                    currentValue = 3;
                    endValue = 5;
                }
                else if (SearchCardValue("K", cardHand))
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
                    if (SearchCardValue(ConvertValue(i), cardHand))
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
                if (SearchCardValue(ConvertValue(currentValue), cardHand))
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

        // Method to check if the hand is flush.
        private static bool CheckForFlush(Card[] cardHand)
        {
            // Get the suit from the first card.
            string suit = cardHand[0].CardSuit;

            foreach (Card card in cardHand)
            {
                // If there's a mismatch, it is not a flush.
                if (card.CardSuit != suit) return false;
            }

            return true;
        }

        // Method to count number of cards with that value.
        private static string[] CountValueCards(Card[] cardHand)
        {
            string valueCounters = "";
            int foundCards = 0;

            // Count cards per value.
            for (int i = 1; i <= 13; i++)
            {
                int valueCount = 0;  // Initial/reset value
                string cardValue = ConvertValue(i);

                foreach (Card card in cardHand)
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

        // Method to get the hierarchy of card values.
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

        // Method to check for possible pairs, threes or fours.
        private static int CheckForMultiCards(string[] valueCounters, int valueCount)
        {
            int totalCount = 0;

            foreach (string cardValueCount in valueCounters)
            {
                if (Convert.ToInt32(cardValueCount.Split('|')[1]) == valueCount) totalCount++;
            }

            return totalCount;
        }

        // Method to search cards with that value.
        private static bool SearchCardValue(string value, Card[] cardHand)
        {
            foreach (Card card in cardHand)
            {
                if (card.CardValue == value) return true;
            }

            return false;
        }
        #endregion

        #region Validations
        // Method to check if the command entered is valid.
        private static bool ValidateCommand(string command, int menuOptions)
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

        // Method to validate cards.
        private static bool ValidateCard(string value, string suit, Card[] cardHand)
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
                foreach (Card card in cardHand)
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

        // Method to validate selected card.
        private static bool ValidateSelectCard(string selection, Card[] cardDeck)
        {
            if (selection == "")
            {
                Console.WriteLine("ERROR: Please enter your card number.");
                return false;
            }
            else if (!int.TryParse(selection, out _))
            {
                Console.WriteLine("ERROR: Invalid card number.");
                return false;
            }
            else if (Convert.ToInt32(selection) < 1 || Convert.ToInt32(selection) > 52)
            {
                Console.WriteLine("ERROR: Please enter a value from 1 to 52.");
                return false;
            }
            else if (CardAlreadySelected(cardDeck[Convert.ToInt32(selection) - 1]))
            {
                Console.WriteLine("ERROR: Card number already selected.");
                return false;
            }

            return true;
        }
        #endregion

        #region Value Conversions
        // Method to convert numbers to face cards or Ace.
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

        // Method to convert numeric value to suit characters.
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

        // Method to convert numeric value to suit characters.
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