namespace Assignment2
{
	// Interface to implement different strategy for Computer Player
	public interface IStrategy
	{
		char GetNextMove();
	}

    // TODO: Fix issue for Alpha Beta Algorithm Strategy
	public class AlphaBetaStrategy: IStrategy
	{
        private int MiniMax(Board board, int depth, bool isMaximizing, int alpha, int beta)
        {
            if (depth == 0)
            {
                return 0; 
            }

            if (isMaximizing)
            {
                int bestScore = int.MinValue;
                foreach (char position in board.GetAvailablePositions())
                {
                    Board newBoard = board.Clone(); 
                    newBoard.PlaceMove('O', position);
                    int score = MiniMax(newBoard, depth - 1, false, alpha, beta);
                    bestScore = Math.Max(bestScore, score);
                    alpha = Math.Max(alpha, bestScore);

                    if (beta <= alpha)
                        break; 
                }
                return bestScore;
            }
            else
            {
                int bestScore = int.MaxValue;
                foreach (char position in board.GetAvailablePositions())
                {
                    Board newBoard = board.Clone(); 
                    newBoard.PlaceMove('X', position);
                    int score = MiniMax(newBoard, depth - 1, true, alpha, beta);
                    bestScore = Math.Min(bestScore, score);
                    beta = Math.Min(beta, bestScore);

                    if (beta <= alpha)
                        break; 
                }
                return bestScore;
            }
        }

        public char GetNextMove()
        {
            TicTacToeGame game = new();
            Board board = game.GetBoard();
            List<char> availableOptions = board.GetAvailablePositions();
            int bestScore = int.MinValue;
            char bestMove = ' ';

            foreach (char position in availableOptions)
            {
                TicTacToeBoard newBoard = (TicTacToeBoard)board.Clone(); 
                newBoard.PlaceMove('O', position);
                int score = MiniMax(newBoard, 5, false, int.MinValue, int.MaxValue);

                if (score > bestScore)
                {
                    bestScore = score;
                    bestMove = position;
                }
            }

            return bestMove;
        }
    }

	// Implement Random selection of move strategy
	public class RandomSelectionStrategy: IStrategy
    {
        public char GetNextMove()
        {
            TicTacToeGame game = new ();
            Board board = game.GetBoard();
            List<char> availableOptions = board.GetAvailablePositions();
            Random rand = new();
            int index = rand.Next(0, availableOptions.Count);
            return availableOptions[index];
        }
    }
}

