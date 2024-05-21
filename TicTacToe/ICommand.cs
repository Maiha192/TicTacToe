namespace Assignment2
{
    // Interface to implement undo/redo of make move command
    public interface ICommand
    {
        void Execute();
        void Undo();
        void Redo();
    }

    // Concrete Command to implement making move by player
    public class MakeMoveCommand : ICommand
    {
        private readonly Game game;
        private readonly Player player;
        private int playerTurn;

        public MakeMoveCommand(Game game, Player player, int playerTurn)
        {
            this.game = game;
            this.player = player;
            this.playerTurn = playerTurn;
        }

        public void Execute()
        {
            game.MakePlay(player, playerTurn);
        }

        public void Undo()
        {
            game.UndoState();
            game.MakePlay(player, playerTurn);
        }


        public void Redo()
        {
            game.RedoState();
            playerTurn = (playerTurn + 1) % 2;
            game.MakePlay(player, playerTurn);
        }
    }

    // Invoker class
    public class CommandInvoker
    {
        private readonly Stack<ICommand> executedCommands = new();
        private readonly Stack<ICommand> undoneCommands = new();

        public void ExecuteCommand(ICommand command)
        {
            command.Execute();
            executedCommands.Push(command);
            undoneCommands.Clear();
        }

        public void UndoLastCommand()
        {
            if (executedCommands.Count > 0)
            {
                ICommand commandToUndo = executedCommands.Pop();
                undoneCommands.Push(commandToUndo);
                commandToUndo.Undo();
            }
        }

        public void RedoLastUndoneCommand()
        {
            if (undoneCommands.Count > 0)
            {
                ICommand commandToRedo = undoneCommands.Pop();
                executedCommands.Push(commandToRedo);
                commandToRedo.Redo();
            }
        }
    }
}

