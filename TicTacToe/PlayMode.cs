using static System.Console;
namespace Assignment2
{
    // Class to set play mode and choose strategy (in case player 2 is Computer)
    class PlayMode
    {
        private int modeNumber;
        private int strategyNum;

        public int ModeNumber
        {
            get
            {
                return modeNumber;
            }
            set
            {
                if (value == 1 || value == 2)
                {
                    modeNumber = value;
                }
            }
        }


        public int StrategyNum
        {
            get
            {
                return strategyNum;
            }
            set
            {
                if (value == 1 || value == 2)
                {
                    strategyNum = value;
                }
            }

        }

        // Method to set play mode according to user's selection
        public void SetMode()
        {
            Write("Please choose a play mode number: 1 (Human Vs Human) or 2 (Human vs Computer) >> ");
            int input;
            while (!int.TryParse(ReadLine(), out input) || (input != 1 && input != 2))
            {
                Write("Invalid option! Please choose either 1 or 2 >> ");
            }
            ModeNumber = input;
            if (ModeNumber == 2)
            {
                Write("Please choose a strategy number for the Computer: 1 (Random Selection of move) or 2 (Alpha Beta Algorithm) >> ");
                int strategyInput;
                while (!int.TryParse(ReadLine(), out strategyInput) || (strategyInput != 1 && input != 2))
                {
                    Write("Invalid option! Please choose either 1 or 2 >> ");
                }
                StrategyNum = strategyInput;
            }
        }

        // Method to get player 1 assuming that player 1 is always Human Player
        public HumanPlayer GetPlayer1()
        {
            HumanPlayer player1 = new();
            return player1;
        }

        // Method to get player 2 based on different play mode
        public Player GetPlayer2(int modeNumber, int strategyNum)
        {
            Player player2;
            if (modeNumber == 1)
                player2 = new HumanPlayer();
            else
            {
                player2 = new ComputerPlayer();
                if (strategyNum == 1)
                    player2.SetStrategy(new RandomSelectionStrategy());
                else
                    player2.SetStrategy(new AlphaBetaStrategy());
            }
            return player2;
        }
    }

}



