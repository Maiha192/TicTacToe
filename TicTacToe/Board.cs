using System.Text;
using static System.Console;
namespace Assignment2
{
	abstract class Board
	{
        // Methods to be called by other classes
        public abstract void DrawBoard();
        public abstract void PlaceMove(char symbol, char position);
        public abstract bool IsPositionAvailable(char position);
        public abstract char GetSquareValue(int row, int col);
        public abstract void UndoBoardState();
        public abstract void RedoBoardState();
        public abstract void FromString(string boardState);
        public abstract Board Clone();
        public abstract List<char> GetAvailablePositions();
    }
    
	class TicTacToeBoard: Board
	{
        private char[,] squares = new char[,] { { '1', '2', '3' }, { '4', '5', '6' }, { '7', '8', '9' } };
        private readonly List<string> MoveHistory = new();
        private readonly List<string> MoveHistoryToRedo = new();

        // Method to populate squares from move history used for undo board state
        private void PopulateSquaresFromMoveHistory(List<string> moveHistory)
        {
            squares = new char[,] { { '1', '2', '3' }, { '4', '5', '6' }, { '7', '8', '9' } };
            foreach (string move in moveHistory)
            {
                string[] moveComponents = move.Split(':');
                if (moveComponents.Length == 2)
                {
                    char playerSymbol = moveComponents[0][0];
                    int position = int.Parse(moveComponents[1]);

                    int rowIndex = (position - 1) / 3;
                    int columnIndex = (position - 1) % 3;

                    squares[rowIndex, columnIndex] = playerSymbol;
                }
            }
        }

        // Method to get the MoveHistoryToRedo
        public int GetNumberOfMoveHistoryToRedo()
        {
            return MoveHistoryToRedo.Count;
        }

        // Method to set MoveHistory from load game
        public void SetMoveHistory(string movesState)
        {
            string[] lines = movesState.Trim().Split('\n');
            foreach (string line in lines)
            {
                MoveHistory.Add(line);
            }
        }

        // Method to initialise a grid board
        public override void DrawBoard()
        {
            WriteLine("_________________");
            for (int i = 0; i < 3; i++)
            {
                WriteLine("     |     |     |");
                for (int j = 0; j < 3; j++)
                {

                    Write("  {0}  |", squares[i, j]);
                }
                WriteLine("\n_____|_____|_____|");
            }
            WriteLine();
        }

        // Method to place a move and add moves to history
        public override void PlaceMove(char symbol, char position)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {

                    if (position == squares[i, j])
                    {
                        squares[i, j] = symbol;
                        string move = $"{symbol}:{position}";
                        MoveHistory.Add(move);
                    }
                }
            }
            DrawBoard();
        }

        // Method to check if a square is available to place move
        public override bool IsPositionAvailable(char position)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (squares[i, j] == position)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        // Method to return the value of a square
        public override char GetSquareValue(int row, int col)
        {
            if (row >= 0 && row < 3 && col >= 0 && col < 3)
            {
                return squares[row, col];
            }
            else
            {
                ArgumentOutOfRangeException argumentOutOfRangeException = new ("Row and column must be in the range 0 to 2.");
                throw argumentOutOfRangeException;
            }
        }

        // Method to undo board state
        public override void UndoBoardState()
        {
            MoveHistoryToRedo.Insert(0, MoveHistory.Last());
            MoveHistory.RemoveAt(MoveHistory.Count - 1);
            PopulateSquaresFromMoveHistory(MoveHistory);
        }

        // Method to redo board state
        public override void RedoBoardState()
        {
            MoveHistory.Add(MoveHistoryToRedo[0]);
            MoveHistoryToRedo.RemoveAt(0);
            PopulateSquaresFromMoveHistory(MoveHistory);
        }
        
        // Method to be used for loading a save game in Game class
        public override void FromString(string boardState)
        {
            int index = 0;
            for (int i = 0; i<3; i ++)
            {
                for (int j = 0; j < 3; j ++)
                {
                    squares[i, j] = boardState[index];
                    index++;
                }
            }
        }

        // Method to return a string representation of a board used for saving game
        public override string ToString()
        {
            StringBuilder sb = new();

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    sb.Append(squares[i, j]);
                }
            }

            sb.AppendLine();

            foreach (string move in MoveHistory)
            {
                sb.AppendLine(move);
            }

            return sb.ToString();
        }

        // Method to create a new board copy used in IStrategy
        public override Board Clone()
        {
            TicTacToeBoard clonedBoard = new();
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    clonedBoard.squares[i, j] = this.squares[i, j];
                }
            }
            clonedBoard.MoveHistory.AddRange(this.MoveHistory);
            clonedBoard.MoveHistoryToRedo.AddRange(this.MoveHistoryToRedo);
            return clonedBoard;
        }

        // Method to get available positions for computer player to apply strategy in making move
        public override List<char> GetAvailablePositions()
        {
            List<char> availablePositions = new();
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (squares[i, j] >= '1' && squares[i, j] <= '9')
                    {
                        availablePositions.Add(squares[i, j]);
                    }
                }
            }
            return availablePositions;
        }
    }
}
