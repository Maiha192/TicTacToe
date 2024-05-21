using static System.Console;
namespace Assignment2
{
    // Base Class for 2 types of players
    public abstract class Player
    {
        private string name = "";
        private char symbol;

        public abstract char MakeMove();
        public abstract void SetStrategy(IStrategy strategy);

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }

        public char Symbol
        {
            get
            {
                return symbol;
            }
            set
            {
                symbol = value;
            }
        }
	}

    // Subclass for Human Player type
	public class HumanPlayer: Player
	{
        public override char MakeMove()
        {
            char position = ' ';
            do
            {
                Write("Enter a square number to place your move or press 'o' for game options >> ");
                string input = ReadLine()??"";
                if (input.ToLower() == "o")
                {
                    return 'o';
                }
                if (input.Length == 1 && char.IsDigit(input[0]) && input[0] >= '1' && input[0] <= '9')
                {
                    position = input[0];
                }
            } while (!char.IsDigit(position) || position < '1' || position > '9');
            return position;

        }
        public override void SetStrategy(IStrategy strategy)
        {
            throw new NotImplementedException();
        }
    }

    // Subclass for Computer Player type
	public class ComputerPlayer: Player
	{
        IStrategy strategy;

        public override void SetStrategy(IStrategy strategy)
        {
            this.strategy = strategy;
        }

        public override char MakeMove()
        {
            return strategy.GetNextMove();
        }
    }
}

