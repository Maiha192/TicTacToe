using System;
using System.Text;
using static System.Console;
using System.IO;
namespace Assignment2
{
	public abstract class Game
	{
        // Abstract methods representing basic steps in a game
        protected abstract void PlayGame();
        protected abstract void ShowGameOptions();
        protected abstract void ContinueGame(Player player1, Player player2);
        protected abstract bool EndOfGame(int numOfMoves);
		protected abstract void PrintWinner(int numOfMoves, int playerTurn);
        protected abstract void SaveGame();
        protected abstract void LoadGame();

        // Public methods to be called from other Interfaces
        public abstract void MakePlay(Player player, int playerTurn);
        public abstract void UndoState();
        public abstract void RedoState();
        

        // Method to show help
        protected virtual void ShowHelp()
        {
            WriteLine("Press 1 if you want to play a new game.");
            WriteLine("Press 2 if you want to load the saved game to continue.");
            WriteLine("Press 3 if you want to load help.");
            WriteLine("Press 4 if you want to quit the game.");
            Write("Press ! to exit help >> ");
            string userInput = ReadLine() ?? "";
            while (!char.TryParse(userInput, out char option) || option != '!')
            {
                Write("Invalid option! Press ! to exit help >> ");
                userInput = ReadLine() ?? "";
            }
            this.InitialiseGame();
        }

        // Template method to be called for game initialisation in Program class
        public void InitialiseGame()
        {
            WriteLine("Main Menu");
            WriteLine("1 - New Game");
            WriteLine("2 - Load Game");
            WriteLine("3 - Load Help");
            WriteLine("4 - Quit");
            Write("Please choose an option >> ");
            string userInput = ReadLine() ?? "";
            int option;
            while (!int.TryParse(userInput, out option) || option < 1 || option > 4)
            {
                Write("Invalid option! Please enter a number between 1 and 4 >> ");
                userInput = ReadLine() ?? "";
            }   
            switch (option)
            {
                case 1:
                    PlayGame();
                    break;
                case 2:
                    LoadGame();
                    break;
                case 3:
                    ShowHelp();
                    break;
                case 4:
                    WriteLine("Thank you and see you again!");
                    return;
                default:
                    WriteLine("This option is not available!");
                    this.InitialiseGame();
                    break;
            }
        }
	}

	class TicTacToeGame: Game
	{
        private readonly TicTacToeBoard aBoard = new();
        private int playerTurn = 0;
        private int numOfMoves = 0;
        private readonly PlayMode playMode = new();
        private readonly CommandInvoker invoker = new();

        // Method to start a new game and get play mode
        protected override void PlayGame()
        {
            playMode.SetMode();
            int modeNum = playMode.ModeNumber;
            int strategyNum = playMode.StrategyNum;
            Player player1 = playMode.GetPlayer1();
            Write("Enter player 1 name >> ");
            player1.Name = ReadLine() ?? "";
            Player player2 = playMode.GetPlayer2(modeNum, strategyNum);
            if (modeNum == 1)
            {
                Write("Enter player 2 name >> ");
                player2.Name = ReadLine() ?? "";
            }
            else
            {
                player2.Name = "Computer";
                WriteLine("Player 2 is the computer!");

            }

            aBoard.DrawBoard();

            ContinueGame(player1, player2);
        }

        // Method to show different options during a game
        protected override void ShowGameOptions()
        {
            WriteLine("Gameplay options: ");
            WriteLine("1 - Continue");
            WriteLine("2 - Undo");
            WriteLine("3 - Redo");
            WriteLine("4 - Save Game");
            WriteLine("5 - Quit");
            Write("Please choose an option >> ");
            string userInput = ReadLine() ?? "";
            int option;
            while (!int.TryParse(userInput, out option) || option < 1 || option > 5)
            {
                Write("Invalid option! Please enter a number between 1 and 5 >> ");
                userInput = ReadLine() ?? "";
            }
            switch (option)
            {
                case 1:
                    break;
                case 2:
                    invoker.UndoLastCommand();
                    break;
                case 3: 
                    invoker.RedoLastUndoneCommand();
                    break;
                case 4:
                    SaveGame();
                    WriteLine("Game has been saved! See you later!");
                    Environment.Exit(0);
                    return;
                case 5:
                    WriteLine("Thank you and see you later!");
                    Environment.Exit(0);
                    return;
                default:
                    WriteLine("This option is not available!");
                    break;
            }
        }

        // Method to continue playing after selecting a game option
        protected override void ContinueGame(Player player1, Player player2)
        {
            while (!EndOfGame(numOfMoves))
            {
                if (playerTurn == 0)
                {
                    invoker.ExecuteCommand(new MakeMoveCommand(this, player1, playerTurn)); 
                }
                else
                {
                    invoker.ExecuteCommand(new MakeMoveCommand(this, player2, playerTurn)); 
                }
                playerTurn = (playerTurn + 1) % 2;
                numOfMoves += 1;
            }
        }

        // Method to check if the game is over with a win or a tie
        protected override bool EndOfGame(int numOfMoves)
		{
            bool isGameOver = false;

            for (int i = 0; i < 3; i++)
            {
                if ((aBoard.GetSquareValue(i, 0) == aBoard.GetSquareValue(i, 1) && aBoard.GetSquareValue(i, 1) == aBoard.GetSquareValue(i, 2)) ||
                    (aBoard.GetSquareValue(0, i) == aBoard.GetSquareValue(1, i) && aBoard.GetSquareValue(1, i) == aBoard.GetSquareValue(2, i)))
                {
                    isGameOver = true;
                    PrintWinner(numOfMoves, playerTurn);
                    break;
                }
            }

            if (!isGameOver && ((aBoard.GetSquareValue(0, 0) == aBoard.GetSquareValue(1, 1) && aBoard.GetSquareValue(1, 1) == aBoard.GetSquareValue(2, 2)) ||
                                (aBoard.GetSquareValue(0, 2) == aBoard.GetSquareValue(1, 1) && aBoard.GetSquareValue(1, 1) == aBoard.GetSquareValue(2, 0))))
            {
                isGameOver = true;
                PrintWinner(numOfMoves, playerTurn);
                
            }

            if (!isGameOver && numOfMoves == 9)
            {
                isGameOver = true;
                WriteLine("Game Over! It's a tie!");
            }

            return isGameOver;
        }

        // Method to print winner 
        protected override void PrintWinner(int numOfMoves, int playerTurn)
		{
            if (playerTurn == 1)
            {
                WriteLine("Game Over! The winner is Player 1 with Symbol X!");
            }
            else
            {
                WriteLine("Game Over! The winner is Player 2 with Symbol O!");
            }
        }

        // Method to save game to a file 
        protected override void SaveGame()
        {
            const string FILENAME = "Game.txt";

            FileStream outFile = new(FILENAME, FileMode.Create, FileAccess.Write);
            StreamWriter writer = new(outFile);

            writer.WriteLine(this.ToString());

            writer.Close();
            outFile.Close();
        }

        // Method to load a saved game from file and continue playing
        protected override void LoadGame()
        {
            string gameState;
            const string FILENAME = "Game.txt";

            FileStream inFile = new(FILENAME, FileMode.Open, FileAccess.Read);
            StreamReader reader = new(inFile);

            gameState = reader.ReadToEnd();
            string[] lines = gameState.Split(Environment.NewLine);

            int modeNumber = int.Parse(lines[0]);
            playMode.ModeNumber = modeNumber;

            int strategyNumber = int.Parse(lines[1]);
            playMode.StrategyNum = strategyNumber;

            playerTurn = int.Parse(lines[2]);

            numOfMoves = int.Parse(lines[3]);

            string boardState = lines[4];
            string movesState = string.Join(Environment.NewLine, lines.Skip(5));
            aBoard.SetMoveHistory(movesState);
            aBoard.FromString(boardState);
            aBoard.DrawBoard();

            Player player1 = playMode.GetPlayer1();
            Player player2 = playMode.GetPlayer2(modeNumber, strategyNumber);

            ContinueGame(player1, player2);

            reader.Close();
            inFile.Close();
        }

        // Method to return the game board instance
        public Board GetBoard()
        {
            return aBoard;
        }


        // Method to be used in MakeMoveCommand in ICommand interface for undo/redo of moves
        public override void MakePlay(Player player, int playerTurn)
        {
            if (playerTurn == 0)
                player.Symbol = 'X';
            else
            {
                player.Symbol = 'O';
            }
            if (!EndOfGame(numOfMoves))
            {
                WriteLine("Player turn: Player {0} - Player symbol: {1}", playerTurn + 1, player.Symbol);

                char position = player.MakeMove();
                if (position == 'o')
                {
                    ShowGameOptions();
                    MakePlay(player, playerTurn);
                }
                else
                {
                    while (!aBoard.IsPositionAvailable(position))
                    {
                        position = player.MakeMove();
                    }
                    aBoard.PlaceMove(player.Symbol, position);
                }
            }
            else
                PrintWinner(numOfMoves, playerTurn);
        }

        // Method to be used in MakeMoveCommand in ICommand interface for undo of moves
        public override void UndoState()
        {
            aBoard.UndoBoardState();
            aBoard.DrawBoard();
        }

        // Method to be used in MakeMoveCommand in ICommand interface for redo of moves
        public override void RedoState()
        {
            int numMovesToRedo = aBoard.GetNumberOfMoveHistoryToRedo();
            if (numMovesToRedo > 0)
            {
                aBoard.RedoBoardState();
                aBoard.DrawBoard();
            }
        }

        // Method to return a string represenation of a game used for saving game
        public override string ToString()
        {
            StringBuilder sb = new();

            sb.AppendLine(playMode.ModeNumber.ToString());

            sb.AppendLine(playMode.StrategyNum.ToString());

            sb.AppendLine(playerTurn.ToString());

            sb.AppendLine(numOfMoves.ToString());

            sb.Append(aBoard.ToString());

            return sb.ToString();
        }
    }
}

