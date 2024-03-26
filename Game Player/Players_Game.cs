using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Player
{
    internal class Players_Game : Game
    {
        public static void Menu()
        {
            while (true)
            {
                Console.WriteLine("\nВыберите персонажа!");
                Console.WriteLine("\n1 - Создать персонажа\n2 - Выбрать созданного.");
                string? choice = Console.ReadLine();
                if (choice == "1")
                {
                    Game.players.Add(new Game());
                }
                else if (choice == "2")
                {
                    Console.WriteLine(" Имя:");
                    string? check_names = Console.ReadLine();//проверка
                    var names = from g in Game.players
                                where g.name == check_names
                                select g;
                    foreach (var name in names)
                    {
                        name.Archive(Game.players);
                    }
                }
            }
        }
    }
}
