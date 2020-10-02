using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RPS_GameMvc.Models;

namespace RPS_GameMvc.GamePlay
{
	public class Rps_GameMethods
	{
        /// <summary>
        /// Get a choice of 1 (play) or 2 (quit) from the user
        /// </summary>
        /// <returns>int</returns>
        public static int GetUsersIntent()
        {
            int choice;//this is be out variable choice of the player to 1 (play) or 2 (quit)
            bool inputInt;
            do//prompt loop
            {
                System.Console.WriteLine("Please choose 1 for Play or 2 for Quit");
                string input = Console.ReadLine();
                inputInt = int.TryParse(input, out choice);
            } while (!inputInt || choice <= 0 || choice >= 3);//end of promt loop

            return choice;
        }

        /// <summary>
        /// This method takes 3 lists from the Db and prints out the contents of all of them.
        /// </summary>
        /// <param name="games"></param>
        /// <param name="players"></param>
        /// <param name="rounds"></param>
        public static void PrintAllCurrentData(List<Game> games, List<Player> players, List<Round> rounds)
        {
            foreach (var game in games)
            {
                Console.WriteLine($"Player1 Name => {game.Player1.Name}\ncomputer Name => {game.Computer.Name}\n winner is => {game.winner.Name}");
                Console.WriteLine($"\n\t--- Here are the details of the games rounds --- ");
                foreach (Round round in game.rounds)
                {
                    Console.WriteLine($"player1 => {round.player1.Name}, p1 choice => {round.p1Choice}");
                    Console.WriteLine($"player2 => {round.Computer.Name}, computer choice => {round.ComputerChoice}");
                    Console.WriteLine($"The Outcome (Winner) of this round is player {round.Outcome}\n");
                }
            }
            Console.WriteLine("Here is the list of players.");
            foreach (var player in players)
            {
                Console.WriteLine($"This players name is {player.Name} and he has {player.Wins} wins and {player.Losses} losses");
            }

            Console.WriteLine("Here are all the rounds played in all games so far.");
            foreach (var round in rounds)
            {
                Console.WriteLine($"Round#{round.RoundId}");
                Console.WriteLine($"Player1, {round.player1.Name}, chose {round.p1Choice}");
                Console.WriteLine($"Player2, {round.Computer.Name}, chose {round.ComputerChoice}");
                Console.WriteLine($"The winner of this round was player {round.Outcome}\n");
            }
        }

        /// <summary>
        /// Gets the players name from the user and returns a string
        /// </summary>
        /// <returns></returns>
        public static string GetPlayerName()
        {
            //TODO : add checking for user enters "Computer" as their name
            Console.WriteLine("What is your name?");
            string playerName = Console.ReadLine();
            playerName = playerName.Trim();//take off beginning or ending white space
            return playerName;
        }

        /// <summary>
        /// checks the list of players to see if the player is returning. If not, creates a new player and adds him to the List<Player>
        /// </summary>
        /// <returns></returns>
        public static bool VerifyPlayer(List<Player> players, string playerName)
        {
            // check the list of players to see if this player is a returning player.
            foreach (Player item in players)
            {
                if (item.Name == playerName)
                {
                    //p1 = item;
                    Console.WriteLine("You are a returning player. Game ON!");
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Returns a random Choice of Rock,Paper, Scissors
        /// </summary>
        /// <param name="y"></param>
        /// <returns></returns>
        public static Choice GetRandomChoice()
        {
            Random rand = new Random();
            return (Choice)rand.Next(3);
        }

        /// <summary>
        /// take the round and return 0 for a tie, 1 for p1 wins, 2 for computer wins
        /// </summary>
        /// <param name="round"></param>
        /// <returns></returns>
        public static int GetRoundWinner(Round round)
        {
            if (round.p1Choice == round.ComputerChoice)
            {
                //round.Outcome = 0; // it’s a tie . the default is 0 so this line is unnecessary.
                Console.WriteLine("this round was a tie");
                return 0;
            }
            else if ((int)round.p1Choice == ((int)round.ComputerChoice + 1) % 3)
            { //If users pick is one more than the computer’s, user wins
                //round.Outcome = 1;
                return 1;
            }
            else
            { //If it’s not a tie and p1 didn’t win, then computer wins.
                //round.Outcome = 2;
                return 2;
            }
        }

        /// <summary>
        /// determines if either player has won 2 rounds yet.
        /// </summary>
        /// <param name="game"></param>
        /// <returns>int</returns>
        public static int GetWinner(Game game)
        {
            // get how many rounds p1 has won.
            if (game.rounds.Count(x => x.Outcome == 1) == 2) { return 1; }
            else if (game.rounds.Count(x => x.Outcome == 2) == 2) { return 2; }// get how many rounds computer has won.
            else return 0;
        }


    }//end of class
}//end of namespace
