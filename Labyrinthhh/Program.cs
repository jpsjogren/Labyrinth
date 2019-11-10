using System;
using System.Media;
using System.Text;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            const byte BoxX = 60;       //
            const byte BoxY = 20;       //Storlek på spelytan
            int player;

            Random rnd = new Random();
            int fininsh = rnd.Next(5, 50);

            int[] position = new int[2] { 15, (BoxY + 2) }; //Start position (+2 är för att det ska se lite snyggare ut)

            #region Definiera väggar och ägg..
            //Skapa Egg att hämta och samla poäng
            Eggs[] egg = new Eggs[5];

            egg[0] = new Eggs();
            egg[1] = new Eggs();
            egg[2] = new Eggs();
            egg[3] = new Eggs();
            egg[4] = new Eggs();

            egg[0].SetInfo(15, 15, 0);
            egg[1].SetInfo(50, 10, 0);
            egg[2].SetInfo(8, 17, 0);
            egg[3].SetInfo(25, 15, 0);
            egg[4].SetInfo(30, 6, 0);


            //Skapa en array av väggar
            Wall[] wall = new Wall[7];

            wall[0] = new Wall();
            wall[1] = new Wall();
            wall[2] = new Wall();
            wall[3] = new Wall();
            wall[4] = new Wall();
            wall[5] = new Wall();
            wall[6] = new Wall();
            //Skapa nya väggar                         Gå ej utanför boxen. då blir det för fel
            wall[0].SetInfo(10, 13, 13, 1);
            wall[1].SetInfo(8, 17, 15, 1);                 // Length, Start X, Start Y, 0(X) eller 1(Y) 
            wall[2].SetInfo(30, 13, 13, 0);                //  Y = mellan 3 och 23
            wall[3].SetInfo(2, 43, 21, 1);                 //  X = mellan 2 och 61
            wall[4].SetInfo(5, 43, 13, 1);
            wall[5].SetInfo(40, 21, 7, 0);
            wall[6].SetInfo(28, 1, 5, 0);


            #endregion


            WallPosition[] custumWall = new WallPosition[500];

            for (int i = 0; i < custumWall.Length; i++)
            {
                custumWall[i] = new WallPosition
                {
                    X = 0,
                    Y = 0
                };
            }
            int numberOfPoints = CreatePoint(wall);
            WallPosition[] point = new WallPosition[numberOfPoints];

            //Fyll i dina menyval
            string[] meny = new string[] { "Spela", "Skapa ny bana (\u03B2eta)", "Avsluta" };
            int choice = Meny(meny); // "Renderar" menyn och låter dig välja med piltangenter och enter
          
            //Choice == 1 -> Spela inbyggd bana. Choice == 2 -> Skapa egen bana och spela den sedan. 
            Console.Clear();
            if (choice == 1)
            {
                point = Fillpoints(wall, point); //Skapa kordinater för varje "väggblock"
                player = 1; //Spela vanligt
            }
            else if (choice == 2)
            {
                //Skapa en egen bana. Behöver fixas! EDIT, Funkar rätt ok nu. Bara "äggen" kvar
                custumWall = CreateWall(position, BoxX, BoxY, fininsh, custumWall);
                player = 0; // spela din custom bana
                position[0] = 15; //Start position
                position[1] = BoxY + 2;
                point = Fillpoints(wall, point);
            }
            else if (choice == 3)
            {
                player = 1;
                Environment.Exit(0);
            }
            else 
            {
                //Tänkte att datorn kunde spela själv. Men det var lättare sagt än gjort...
                //Så du får speka själv ;)
                point = Fillpoints(wall, point);
                player = 1;
            }


            //The playing

            do
            {
                if (player == 1)
                {
                    Box(BoxX, BoxY, fininsh);   //Bygger upp ytterramarna för spelet

                    DisplayWall(point);         //Visar väggarna
                    Score(egg);                 //Viasr dina "poäng till höger"
                    ShowEgg(egg);               //Visar "äggen" på spelplanen
                    GettEgg(position, egg);     //Tar bort "äggen" och ger dig poäng

                    //Flyttar spelaren och anropar även Hit() så att du inte kan gå igenom väggar
                    position = Move(position, BoxX, BoxY, fininsh, point); 
                }
                else if (player == 0)
                {
                    Box(BoxX, BoxY, fininsh);

                    DisplayCustomWall(custumWall, 500);
                    Score(egg);
                    ShowEgg(egg);
                    GettEgg(position, egg);

                    position = Move(position, BoxX, BoxY, fininsh, custumWall);
                }

            } while (position[0] != (fininsh + 1) || position[1] != 2);

            
            Console.SetCursorPosition(position[0], position[1]);
            Console.WriteLine(" ");
            Console.ResetColor();

            Win(egg);
            Console.ReadKey();
        }

        static int Meny(string[] choices)
        {
            ConsoleKeyInfo input = new ConsoleKeyInfo();
            int choice = 1; //Nuvarnade val
            int cursorPoint = 11; //Startposition av '-->'

            do
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.Clear();
                Console.Write("\n*******************************************");
                Console.Write(@"
 __    _      _                          
|| |  | |    | |                         
|| |  | | ___| | ___ ___  _ __ ___   ___ 
|| |/\| |/ _ \ |/ __/ _ \| '_ ` _ \ / _ \
\\  /\  /  __/ | (_| (_) | | | | | |  __/
 \\/  \/ \___|_|\___\___/|_| |_| |_|\___| 
"); //Welcome
                Console.WriteLine("\n*******************************************\n");

                for (int i = 0; i < choices.Length; i++)
                {
                    Console.Write("      ");
                    Console.WriteLine(choices[i]);
                    Console.WriteLine();
                }

                Console.ForegroundColor = ConsoleColor.DarkGray;
                if (choice == 1)
                {
                    Console.SetCursorPosition(0, cursorPoint);
                    Console.Write("-->");
                    Console.SetCursorPosition(0, 0);
                    input = Console.ReadKey(true);
                    switch (input.Key) //Switch on Key enum
                    {
                        case ConsoleKey.UpArrow:
                            break;
                        case ConsoleKey.DownArrow:
                            cursorPoint += 2;
                            choice++;
                            break;
                        case ConsoleKey.Enter:

                            break;
                        default:
                            break;
                    }
                }
                else if (choice == choices.Length)
                {
                    Console.SetCursorPosition(0, cursorPoint);
                    Console.Write("-->");
                    Console.SetCursorPosition(0, 0);
                    input = Console.ReadKey(true);
                    switch (input.Key) //Switch on Key enum
                    {
                        case ConsoleKey.UpArrow:
                            cursorPoint -= 2;
                            choice--;
                            break;
                        case ConsoleKey.DownArrow:

                            break;
                        case ConsoleKey.Enter:

                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    Console.SetCursorPosition(0, cursorPoint);
                    Console.Write("-->");
                    Console.SetCursorPosition(0, 0);
                    input = Console.ReadKey(true);
                    switch (input.Key) //Switch on Key enum
                    {
                        case ConsoleKey.UpArrow:
                            cursorPoint -= 2;
                            choice--;
                            break;
                        case ConsoleKey.DownArrow:
                            cursorPoint += 2;
                            choice++;
                            break;
                        case ConsoleKey.Enter:

                            break;
                        default:
                            break;
                    }
                }

            } while (input.Key != ConsoleKey.Enter);

            return choice;
        }

        static void GettEgg(int[] position, Eggs[] egg)
        {
            for (int i = 0; i < egg.Length; i++)
            {
                if (egg[i].x == position[0] && egg[i].y == position[1])
                {
                    if (egg[i].has != 1)
                    {
                        SoundPlayer player = new SoundPlayer();
                        player.SoundLocation = AppDomain.CurrentDomain.BaseDirectory + "\\Speech.wav";
                        player.Play();
                    }
                    egg[i].has = 1;

                }
            }
        }
        static async void ShowEgg(Eggs[] egg)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            for (int i = 0; i < egg.Length; i++)
            {
                if (egg[i].has == 0)
                {
                    Console.SetCursorPosition(egg[i].x, egg[i].y);
                    Console.Write("\u25CF");
                }
            }
            Console.ResetColor();
        }

        static void Win(Eggs[] egg)
        {
            Console.Clear();

            //SoundPlayer player = new SoundPlayer();
            //player.SoundLocation = AppDomain.CurrentDomain.BaseDirectory + "\\win.wav";
            //player.Play();

            int score = 0;
            string winning = @"

__    __            __    _ _____ _   _ 
\\ \ / /           || |  | |_   _| \ | |
 \\ V /___  _   _  || |  | | | | |  \| |
  \\ // _ \| | | | || |/\| | | | | . ` |
  || | (_) | |_| | \\  /\  /_| |_| |\  |
  \\_/\___/ \__,_|  \\/  \/ \___/\_| \_/";
            foreach (var item in egg)
            {
                score += item.has;
            }

            for (int i = 0; i < 7; i++)
            {
                if (i <= score)
                {
                    Console.SetCursorPosition(30, 15);
                    Console.Write("Score ");
                    Console.WriteLine(i + "000");
                    Console.SetCursorPosition(0, 0);
                    System.Threading.Thread.Sleep(50);
                }
                foreach (ConsoleColor c in Enum.GetValues(typeof(ConsoleColor)))
                {

                    Console.ForegroundColor = c;
                    Console.WriteLine(winning);
                    System.Threading.Thread.Sleep(50); // 0,1 sec. deley
                    Console.SetCursorPosition(0, 0);

                }
            }
        }
        static async void DisplayWall(WallPosition[] points)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            for (int i = 0; i < points.Length; i++)
            {
                Console.SetCursorPosition(points[i].X, points[i].Y);
                Console.Write("#");
            }
            Console.ResetColor();
        }

        static async void DisplayCustomWall(WallPosition[] points, int wallNumber)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            for (int i = 0; i < wallNumber; i++)
            {
                Console.SetCursorPosition(points[i].X, points[i].Y);
                Console.Write("#");
            }
            Console.ResetColor();
        }

        static async void Box(int x, int y, int goal)
        {
            Console.SetCursorPosition(0, 2);
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write(" ");
            for (int i = 0; i < x; i++)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                if (i == goal) { Console.Write(" "); }
                else if (i == (goal + 1) || i == (goal - 1))
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("_");
                    Console.ResetColor();
                }
                else if (i == (goal + 2) || i == (goal - 2))
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.Write("_");
                    Console.ResetColor();
                }

                else { Console.Write("_"); }

            }
            Console.WriteLine("");
            for (int i = 0; i < y; i++)
            {
                Console.Write("|");
                for (int j = 0; j < x; j++)
                {
                    if (i == (y - 1))
                    {
                        Console.Write("_");
                    }
                    else
                    {
                        Console.Write(" ");
                    }
                }
                Console.WriteLine("|");
            }
            Console.ResetColor();
        }
        static int[] Move(int[] posi, int boxx, int boxy, int goal, WallPosition[] wallPoint)
        {
            boxy += 2; //Offset the box for nice looks
            string dir = "n";
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.SetCursorPosition(posi[0], posi[1]);
            Console.WriteLine("\u263B");
            Console.SetCursorPosition(0, 0);
            var input = Console.ReadKey(true);

            switch (input.Key) //Switch on Key enum
            {
                case ConsoleKey.UpArrow:
                    dir = "n";
                    if (posi[1] != 3 && !Hit(posi, dir, wallPoint))
                    {
                        posi[1]--;
                    }
                    else if (posi[1] == 3 && posi[0] == (goal + 1))
                    {
                        posi[1]--;

                        Console.ResetColor();
                    }
                    break;
                case ConsoleKey.DownArrow:
                    dir = "s";

                    if (posi[1] != boxy && !Hit(posi, dir, wallPoint))
                    {
                        posi[1]++;
                    }
                    break;
                case ConsoleKey.LeftArrow:
                    dir = "e";
                    if (posi[0] != 1 && !Hit(posi, dir, wallPoint))
                    {
                        posi[0]--;
                    }
                    break;
                case ConsoleKey.RightArrow:
                    dir = "w";
                    if (posi[0] != boxx && !Hit(posi, dir, wallPoint))
                    {
                        posi[0]++;
                    }
                    break;
                default:
                    break;
            }

            Console.ResetColor();
            return posi;
        }
        static void Score(Eggs[] egg)
        {
            int eggsInBasket = 0;
            int eggsLeft = 0;

            for (int i = 0; i < egg.Length; i++)
            {
                if (egg[i].has == 1)
                {
                    eggsInBasket++;
                }
                else
                {
                    eggsLeft++;
                }
            }


            Console.SetCursorPosition(70, 15);
            Console.Write("Eggs collected: " + eggsInBasket);

            Console.SetCursorPosition(70, 16);
            Console.Write("Egg left to collect: " + eggsLeft);
        }

        static bool Hit(int[] myPosition, string dir, WallPosition[] wallPoint)
        {
            bool hit = false;

            for (int i = 0; i < wallPoint.Length; i++)
            {
                switch (dir)
                {
                    case "n":
                        if (wallPoint[i].X == myPosition[0] && wallPoint[i].Y == myPosition[1] - 1)
                        {
                            hit = true;
                        }
                        break;
                    case "s":
                        if (wallPoint[i].X == myPosition[0] && wallPoint[i].Y == myPosition[1] + 1)
                        {
                            hit = true;
                        }
                        break;
                    case "e":
                        if (wallPoint[i].X == myPosition[0] - 1 && wallPoint[i].Y == myPosition[1])
                        {
                            hit = true;
                        }
                        break;
                    case "w":
                        if (wallPoint[i].X == myPosition[0] + 1 && wallPoint[i].Y == myPosition[1])
                        {
                            hit = true;
                        }
                        break;

                    default:
                        break;
                }

            }

            return hit;
        }

        static int CreatePoint(Wall[] wall)
        {
            int numberOfPoints = 0;
            for (int i = 0; i < wall.Length; i++)
            {
                numberOfPoints += wall[i].length;
            }
            return numberOfPoints;
        }
        static WallPosition[] Fillpoints(Wall[] walls, WallPosition[] points)
        {
            int pointCount = 0;
            for (int j = 0; j < walls.Length; j++)
            {
                for (int i = 0; i < walls[j].length; i++)
                {
                    points[pointCount] = new WallPosition();

                    if (walls[j].dir == 0)
                    {
                        int x = walls[j].startx + i;
                        int y = walls[j].starty;
                        points[pointCount].SetInfo(x, y);
                    }

                    if (walls[j].dir == 1)
                    {
                        int x = walls[j].startx;
                        int y = walls[j].starty + i;
                        points[pointCount].SetInfo(x, y);
                    }
                    pointCount++;
                }
            }
            return points;
        }

        static WallPosition[] CreateWall(int[] posi, int boxx, int boxy, int goal, WallPosition[] points)
        {

            int numberOfWalls = 0;
            points[0] = new WallPosition();
            points[0].SetInfo(1, 0);

            while (posi[0] != (goal + 1) || posi[1] != 2)
            {
                Box(boxx, boxy, goal);
                //DisplayWall(custumWall);

                DisplayCustomWall(points, numberOfWalls);
                Console.BackgroundColor = ConsoleColor.DarkYellow;
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.SetCursorPosition(posi[0], posi[1]);
                Console.Write(" ");
                var input = Console.ReadKey(true);

                switch (input.Key) //Switch on Key enum
                {
                    case ConsoleKey.W:
                        if (posi[1] != 3)
                        {
                            posi[1]--;
                        }
                        else if (posi[1] == 3 && posi[0] == (goal + 1))
                        {
                            posi[1]--;
                        }
                        break;
                    case ConsoleKey.S:

                        if (posi[1] != boxy + 2)
                        {
                            posi[1]++;
                        }
                        break;
                    case ConsoleKey.A:
                        if (posi[0] != 1)
                        {
                            posi[0]--;
                        }
                        break;
                    case ConsoleKey.D:
                        if (posi[0] != boxx)
                        {
                            posi[0]++;
                        }
                        break;
                    case ConsoleKey.Spacebar:
                        Console.SetCursorPosition(posi[0], posi[1]);
                        bool exist = false;
                        for (int i = 0; i < points.Length; i++)
                        {
                            if (posi[0] == points[i].X && posi[1] == points[i].Y)
                            {
                                points[i].SetInfo(0, 0);
                                Console.Write(" ");
                                exist = true;
                                break;
                            } 
                        }

                        if (!exist)
                        {
                            points[numberOfWalls].SetInfo(posi[0], posi[1]);
                            Console.Write("#");
                        }


                        numberOfWalls++;
                        break;
                    default:
                        break;
                }
                Console.ResetColor();
                if (numberOfWalls == 498) { break; }
            }

            return points;
        }

        class Wall
        {
            public int length;
            public int startx;
            public int starty;
            public int dir;


            public void SetInfo(int length, int startx, int starty, int dir)
            {
                this.length = length;
                this.startx = startx;
                this.starty = starty;
                this.dir = dir;
            }
        }
        class Eggs
        {
            public int x;
            public int y;
            public int has;

            public void SetInfo(int x, int y, int has)
            {
                this.x = x;
                this.y = y;
                this.has = has;
            }
        }
        class WallPosition
        {
            public int X;
            public int Y;


            public void SetInfo(int X, int Y)
            {
                this.X = X;
                this.Y = Y;
            }
        }
    }
}
