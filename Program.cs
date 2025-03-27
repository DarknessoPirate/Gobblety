
using Gobblety.Implementations;

namespace Gobblety{
    class Program
    {
        static void Main(string[] args)
        {
            var gameController = new GameController(
                new ConsoleUI(),
                new GameBoard(3),
                new GameRules()
            );
            
            gameController.StartGame();
        }
    }
}