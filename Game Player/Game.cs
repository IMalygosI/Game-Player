using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Game_Player
{
    internal class Game
    {

        private string Name;// Имя персонажа
        private int Koordinate_X;//Координаты        
        private int Koordinate_Y;//Координаты
        private int Xp;//Xp персонажей
        private bool lager;//За кого будет персонаж за Альянс или ЗА Орду
        private int Max_Xp;//Масимально возможное Xp
        private int damage;//Значение урона!
        protected bool life = true;// Жив или труп
        private int Victores;//Победы
        private bool mine_stranger;// свой - чужой

        static public List<Game> players;
        List<Game> Friend = new List<Game>();
        List<Game> Enemy = new List<Game>();
        public string? name { get { return Name; } }
        public bool Life { get { return life; } }
        public bool Mine_stranger { get { return mine_stranger; } }
        public int max_Xp { get { return Max_Xp; } }

        public Game() { Info(players); }

        private void Info(List<Game> players)
        {
            Console.WriteLine(" Имя игрока:");
            this.Name = Console.ReadLine();
            Console.WriteLine(" Альянс - 1 Орда - 2");
            int choice = Convert.ToInt32(Console.ReadLine());
            if (choice == 1 || choice < 1)
            {
                this.mine_stranger = true;
            }
            else if (choice == 2 || choice > 2)
            {
                this.mine_stranger = false;
            }
            Console.WriteLine(" Координаты по горизонтали Х, по вертикали Y)"); //Ввод Координат
            this.Koordinate_X = Convert.ToInt32(Console.ReadLine());
            this.Koordinate_Y = Convert.ToInt32(Console.ReadLine());
            Xp = 100;
            Max_Xp = Xp;
            Random random = new Random();
            damage = random.Next(Xp);
        }
        private void Drive_Koordinate_X(List<Game> players) //Перемещение по X
        {
            Console.WriteLine("Идти на лево - 1, на право - 2");
            while (true)
            {
                if (life == false)//Проверка
                {
                    return;
                }
                else if (players.Count(Game => Game.life == true && Game.mine_stranger == false) == 0)
                {
                    return;
                }
                else if (players.Count(Game => Game.life == true && Game.mine_stranger == true) == 0)
                {
                    return;
                }
                string? choice = Console.ReadLine();//лево или право
                switch (choice)
                {
                    case "1":
                        Koordinate_X -= 1;
                        break;
                    case "2":
                        Koordinate_X += 1;
                        break;
                }
                Console.WriteLine($"Координаты: {Koordinate_X},{Koordinate_Y}");
                foreach (Game person in players)
                {
                    if (Koordinate_X == person.Koordinate_X && Koordinate_Y == person.Koordinate_Y)
                    {
                        if (person.life == true)
                        {
                            if (mine_stranger != person.mine_stranger)
                            {
                                Inspection(players);
                            }
                        }
                    }
                }
                Archive(players);
            }
        }
        private void Drive_Koordinate_Y(List<Game> players) //Перемещение по Y
        {
            Console.WriteLine("Идти на вверх - 1, на вниз - 2");
            while (true)
            {
                if (life == false)
                {
                    return;
                }
                if (players.Count(Game => Game.life == true && Game.mine_stranger == false) == 0)
                {
                    return;
                }
                if (players.Count(Game => Game.life == true && Game.mine_stranger == true) == 0)
                {
                    return;
                }
                Console.ForegroundColor = ConsoleColor.Cyan;
                string? choice = Console.ReadLine();
                Console.ForegroundColor = ConsoleColor.White;
                switch (choice)
                {
                    case "вверх":
                        Koordinate_Y += 1;
                        break;
                    case "вниз":
                        Koordinate_Y -= 1;
                        break;
                }
                Console.WriteLine($"Координаты: {Koordinate_X},{Koordinate_Y}");
                foreach (Game player in players)
                {
                    if (Koordinate_X == player.Koordinate_X && Koordinate_Y == player.Koordinate_Y)
                    {
                        if (player.life == true)
                        {
                            if (mine_stranger != player.mine_stranger)
                            {
                                Inspection(players);
                            }
                        }
                    }
                }
                Archive(players);
            }
        }
        private void Inspection(List<Game> players)//Смотрим есть ли рядом враги
        {
            if (life == false)
            {
                return;
            }
            if (players.Count(Game => Game.life == true && Game.mine_stranger == false) == 0)
            {
                return;
            }
            if (players.Count(Game => Game.life == true && Game.mine_stranger == true) == 0)
            {
                return;
            }
            Console.WriteLine(" Рядом обнаружен противник!");
            Console.WriteLine(" Сделайте выбор: 1 - Атака; 2 - Ульта");
            string? choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    battle(players);
                    break;
                case "2":
                    Ulta(players);
                    break;
            }
        }
        private void battle(List<Game> players)//битва
        {
            foreach (Game player in players)
            {
                if (Koordinate_X == player.Koordinate_X && Koordinate_Y == player.Koordinate_Y)
                {
                    if (player.life == true)
                    {
                        if (mine_stranger != player.mine_stranger)
                        {
                            Enemy.Add(player);
                        }
                        else
                        {
                            Friend.Add(player);
                        }
                    }
                }
            }
            Console.WriteLine(" Наша команда:"); //Перечисление союзников
            foreach (Game player in Friend)
            {
                player.Information_Player();
            }
            Console.WriteLine(" Команда противника:"); //Перечисление врагов
            foreach (Game player in Enemy)
            {
                player.Information_Player();
            }
            while (true) //Создание общ.урона
            {
                int Friend_Uron = 0;
                int Enemy_Uron = 0;
                foreach (Game player in Friend) //Складываем урон членов команд
                {
                    Friend_Uron += player.damage;
                }
                foreach (Game player in Enemy)
                {
                    Enemy_Uron += player.damage;
                }
                Friend_Uron /= Enemy.Count; //Деление общего урона своих на количество противников
                Enemy_Uron /= Friend.Count; //Деление общего урона врагов на количество противников
                foreach (Game player in Friend) //Нанесение урона 
                {
                    player.Max_Xp -= Enemy_Uron;
                    if (player.Max_Xp <= 0)
                    {
                        player.life = false;
                    }
                }
                foreach (Game player in Enemy)
                {
                    player.Max_Xp -= Friend_Uron;
                    if (player.Max_Xp <= 0)
                    {
                        player.life = false;
                    }
                }
                if (life == false) //Проверка на живой/труп
                {
                    Console.WriteLine("Персонаж мертв");
                    return;
                }
                if (players.Count(Game => Game.life == true && Game.mine_stranger == mine_stranger) == 0)
                {
                    return;
                }
                if (players.Count(Game => Game.life == true && Game.mine_stranger != mine_stranger) == 0)
                {
                    return;
                }
                Console.WriteLine(" Состояние своих: "); //проверка ОЗ
                foreach (Game player in Friend)
                {
                    if (player.life == true)
                    {
                        Console.WriteLine($" Имя: {player.Name},\nXp: {player.Max_Xp},\n Урон получен: {Friend_Uron}");
                    }
                }
                Console.WriteLine(" Состояние противников: ");
                foreach (Game player in Enemy)
                {
                    if (player.life == true)
                    {
                        Console.WriteLine($" Имя: {player.Name},\nXp: {player.Max_Xp},\n Урон получен: {Friend_Uron}");
                    }
                    else
                    {
                        Console.WriteLine(" Вы победили!");
                        Victores += 1;
                        Archive(players);
                        return;
                    }
                }
                while (true)
                {
                    Console.WriteLine(" Сделайте выбор: 1 - Атака; 2 - Ульта");
                    string? choice = Console.ReadLine();
                    switch (choice)
                    {
                        case "1":
                            battle(players);
                            break;
                        case "2":
                            Ulta(players);
                            break;
                    }
                    break;
                }
                if (Enemy.Count(Game => Game.life == true) == 0) //проверка победы 
                {
                    Console.WriteLine(" Вы победили!");
                    Victores += 1;
                    Archive(players);
                    return;
                }
            }
        }
        private void Ulta(List<Game> players) //Ульта есть ульта...
        {
            foreach (Game player in players)
            {
                if (Koordinate_X == player.Koordinate_X && Koordinate_Y == player.Koordinate_Y)
                {
                    if (mine_stranger != player.mine_stranger)
                    {
                        player.life = false;
                        Console.WriteLine(" Победа");
                        Victores += 1;
                        Archive(players);
                    }
                }
            }
        }
        private void Treatment(List<Game> players) //Лечение товарищей
        {
            while (true)
            {
                Console.WriteLine(" Имя:");
                string? choice = Console.ReadLine();
                foreach (Game player in players)
                {
                    if (choice == player.Name)
                    {
                        if (player.mine_stranger == mine_stranger)
                        {
                            Console.WriteLine(" Сколько Xp восстановить:");
                            int hp = Convert.ToInt32(Console.ReadLine());
                            if (hp < Max_Xp)
                            {
                                if (hp < player.Xp)
                                {
                                    player.Max_Xp += hp;
                                    Max_Xp -= hp;
                                    break;
                                }
                                else
                                {
                                    Console.WriteLine(" ОШИБКА! Нельзя восстановить больше 100 Xp!");
                                }
                            }
                            else
                            {
                                Console.WriteLine(" ОШИБКА! Нельзя тратить больше Xp, чем у вас имеется!");
                            }
                        }
                        else
                        {
                            Console.WriteLine(" ОШИБКА! Нельзя лечить противников!");
                        }
                    }
                }
                break;
            }
            Archive(players);
        }
        private void Treatment_Yourself() //Лечение себя
        {
            Console.WriteLine("1 - Не полное восстановление; 2 - Полное восстановление");
            string? choice = Console.ReadLine();
            if (choice == "1")
            {
                if (Max_Xp <= 80)
                {
                    Max_Xp += 10;
                }
                else if (Max_Xp >= 80)
                {
                    Console.WriteLine("Ошибка! Ваше Xp больше или равно 80");
                }
            }
            else if (choice == "2")
            {
                if (Victores >= 5)
                {
                    Max_Xp = Xp;
                    Victores -= 5;
                    Console.WriteLine("Вы полностью восстановлены");
                }
                else
                {
                    Console.WriteLine($" ОШИБКА! У вас нет 5 побед, сейчас у вас {Victores} побед.");
                }
            }
            Archive(players);
        }
        private void Information_Player()//Архив
        {
            Console.WriteLine($" Имя: {Name}");
            Console.WriteLine($" Координаты: ({Koordinate_X},{Koordinate_Y})");
            Console.WriteLine($" Здоровье: Максимум: {Max_Xp}/Сейчас: {Xp}");
            if (mine_stranger == true)
            {
                Console.WriteLine($" Лагерь: Альянс");
            }
            else
            {
                Console.WriteLine($" Лагерь: Орда");
            }
            Console.WriteLine($" Дамаг: {damage}");
            Console.WriteLine($" Побед: {Victores}");           
        }
        public void Deserte(List<Game> players)//Дезертируем!
        {
            if (Xp <= 0)
            {
                Console.WriteLine("Некромантов нет, мертвых не поднимаем!");
            }//Если перс который выбран уже остыл!
            else
            {
                foreach (Game player in players)
                {
                    Friend.Remove(player);
                    Enemy.Add(player);
                    if (mine_stranger == true)
                    {
                        mine_stranger = false;
                    }
                    else if(mine_stranger == false)
                    {
                        mine_stranger = true;
                    }
                    Console.WriteLine("Вы дезертировали!");
                    Archive(players);
                }
            }
        }//Готово!
        public void Archive(List<Game> players)
        {
            foreach (Game player in players)//сверка
            {
                if (Koordinate_X == player.Koordinate_X && Koordinate_Y == player.Koordinate_Y)
                {
                    if (player.mine_stranger == true)
                    {
                        if (mine_stranger != player.mine_stranger)
                        {
                            Inspection(players);
                        }
                    }
                }
            }
            while (true)
            {
                if (players.Count(Game => Game.life == true && Game.mine_stranger != mine_stranger) == 0 && players.Count(Game => Game.life == true && Game.mine_stranger == mine_stranger) == 0)//true
                {
                    Console.WriteLine(" Ничья");
                    Archive(players);
                    return;
                }
                else if (players.Count(Game => Game.life == true && Game.mine_stranger != mine_stranger) == 0)//true
                {
                    Console.WriteLine(" Победа!");
                    Victores += 1;
                    Players_Game.Menu();
                    return;
                }
                else if (players.Count(Game => Game.life == true && Game.mine_stranger == mine_stranger) == 0)//true
                {
                    Console.WriteLine(" Вы проиграли!");
                    Players_Game.Menu();
                    return;
                }
                else
                {
                    if (life == false)
                    {
                        Console.WriteLine(" Персонаж мёртв ");
                        return;
                    }
                    else
                    {;
                        Console.WriteLine("\n1 - Информация о персонаже\n" +
                                          "2 - Переместиться горизонтали (влево или в право)\n" +
                                          "3 - Переместиться вертикали (вверх или вниз)\n" +
                                          "4 - Лечить себя\n" +
                                          "5 - Лечить союзников\n" +
                                          "6 - Выход в иговое меню\n" +
                                          "7 - Deserte\n");

                        string? choice = Console.ReadLine();
                        switch (choice)
                        {
                            case "1":
                                Information_Player();
                                Archive(players);
                                break;
                            case "2":
                                Drive_Koordinate_X(players);
                                break;
                            case "3":
                                Drive_Koordinate_Y(players);
                                break;
                            case "4":
                                Treatment_Yourself();
                                break;
                            case "5":
                                Treatment(players);
                                break;
                            case "6":
                                Players_Game.Menu();
                                break;
                            case "7":
                                Deserte(players);
                                break;
                        }
                    }
                }
            }
        }
    }
}
