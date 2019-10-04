using Core.Games;
using GameLogic;

namespace SlidingPuzzle
{
    /// <summary>
    /// A Console Application as user interface! XD
    /// TODO: Create GUI
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                var engine = new GameEngine(CreateGame(3, 3));
                engine.Run();
            }
        }

        private static IGame CreateGame(int width, int height)
        {
            var state = new State(width, height);
            state.Shuffle(30);
            var players = new IPlayer[] { new Solver(width, height) };
            return new Game(state, players);
        }
    }
}
