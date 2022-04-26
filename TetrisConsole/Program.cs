using TetrisLibrary;

namespace TetrisConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            GameEngine game = new GameEngine(true, true);
            game.Start();
        }
    }
}