using Game_Player;

namespace list
{
    internal class Program
    {
        public static void Main()
        {
            Game.players = new List<Game>();
            Players_Game.Menu();
        }
    }
}