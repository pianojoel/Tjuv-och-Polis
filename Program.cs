using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace TjuvOchPolis
{
    class Program
    {
        static void Main(string[] args)
        {
            //Initialize 
            int numberOfCitizens = 10;
            int numberOfThieves = 10;
            int numberOfPolice = 5;
            Console.SetWindowSize(130, 40);
            Console.BackgroundColor = ConsoleColor.White;
            var rnd = new Random();
            Console.CursorVisible = false;
            List<Person> citizens = new List<Person>();
            List<Thief> thieves = new List<Thief>();
            List<Person> policemen = new List<Person>();
            List<Thief> inmates = new List<Thief>();
            int x;
            int y;
            for (int i = 0; i < numberOfCitizens; i++)
            {
                do
                {
                    x = rnd.Next(1, 100);
                    y = rnd.Next(1, 25);

                }
                while ((citizens.Any(person => person.X == x && person.Y == y)) || (thieves.Any(thief => thief.X == x && thief.Y == y)) || (policemen.Any(police => police.X == x && police.Y == y)));
                citizens.Add(new Citizen(x, y));
                citizens[i].SetDir(rnd.Next(0, 8));
                Console.WriteLine(citizens[i].X + " " + citizens[i].Y);
            }
            for (int i = 0; i < numberOfThieves; i++)
            {

            
                do
                {
                    x = rnd.Next(1, 100);
                    y = rnd.Next(1, 25);

                }
                while ((citizens.Any(person => person.X == x && person.Y == y)) || (thieves.Any(thief => thief.X == x && thief.Y == y)) || (policemen.Any(police => police.X == x && police.Y == y)));
            thieves.Add(new Thief(x, y));
                thieves[i].SetDir(rnd.Next(0, 8));
            }
        for (int i = 0; i < numberOfPolice; i++)
            {
                do
                {
                    x = rnd.Next(1, 100);
                    y = rnd.Next(1, 25);
                    
                }
                while ((citizens.Any(person => person.X == x && person.Y == y)) || (thieves.Any(thief => thief.X == x && thief.Y == y)) || (policemen.Any(police => police.X == x && police.Y == y)));
                policemen.Add(new Police(x, y));
                policemen[i].SetDir(rnd.Next(0, 8));
            }
                
                
               
                
            
            Console.Clear();
            for (int i = 0; i < 100; i++)
            {
                Console.Write("-");
            }
            for (int i = 1; i < 25; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write("|");
                Console.SetCursorPosition(100, i);
                Console.Write("|");
            }
            Console.SetCursorPosition(0, 25);
            for (int i = 0; i < 100; i++)
            {
                Console.Write("-");
            }
            //Play Game
            do
            {
                

                //Update prison
                Console.SetCursorPosition(80, 26);
                Console.Write($"I fängelse: {inmates.Count()} {(inmates.Count() == 1 ? "tjuv" : "tjuvar")}");
                foreach (Thief thief in inmates)
                {
                    Console.SetCursorPosition(80, Console.CursorTop + 1);
                    var timeleft = 30 - (DateTime.Now - thief.TimeOfBooking).Seconds;
                    Console.Write("Tid kvar på straff : " + timeleft +" ");
                    if (timeleft <= 0)
                    {
                        
                        thief.InJail = false;
                        thieves.Add(thief);
                        Console.SetCursorPosition(80, Console.CursorTop);
                        Console.Write("Tjuven släpps från fängelset");
                        //Thread.Sleep(2500);
                        //Console.Clear();
                    }


                }
                inmates.RemoveAll(inmate => !inmate.InJail);
                
                
                //Update Citizens
                foreach (Citizen citizen in citizens)
                {
                    Console.SetCursorPosition(citizen.X, citizen.Y);
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.Write(" ");
                    citizen.Update();
                    Console.BackgroundColor = ConsoleColor.Yellow;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.SetCursorPosition(citizen.X, citizen.Y);
                    Console.Write("M");
                }
                
                //Update thieves
                foreach(Thief thief in thieves)
                {
                    Console.SetCursorPosition(thief.X, thief.Y);
                    Console.BackgroundColor = ConsoleColor.White;

                    Console.Write(" ");
                    thief.Update();
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.SetCursorPosition(thief.X, thief.Y);
                    Console.Write("T");
                }
                
                //Update Police
                foreach(Police police in policemen)
                {
                    Console.SetCursorPosition(police.X, police.Y);
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.Write(" ");
                    police.Update();
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.SetCursorPosition(police.X, police.Y);
                    Console.Write("P");
                }
                
                

                // Detect Collision 
                Console.SetCursorPosition(0, 26);
                foreach (Citizen citizen in citizens)
                {
                    foreach (Thief thief in thieves)
                    {
                        if (citizen.X == thief.X && citizen.Y == thief.Y)
                        {
                            clearTextBox();
                            Console.WriteLine("Medborgare möter tjuv ");
                            if (citizen.Inventory.Count() > 0)
                            {
                                var rndPossesion = rnd.Next(0, citizen.Inventory.Count());
                                thief.Loot.Add(citizen.Inventory[rndPossesion]);
                                citizen.Inventory.RemoveAt(rndPossesion);
                                Console.WriteLine("I tjuvens ägo: " + thief.Loot.Count() + " saker ");
                                Console.WriteLine("I medborgarens ägo: " + citizen.Inventory.Count() + " saker ");
                            }
                            else
                            {
                                Console.WriteLine("Medborgaren har inga ägodelar");
                            }
                            

                            continue;
                        }

                    }
                }
                foreach (Thief thief in thieves)
                {
                    foreach(Police police in policemen)
                    {
                        if (thief.X == police.X && thief.Y == police.Y)
                        {
                            clearTextBox();
                            Console.WriteLine("Tjuv möter polis ");
                            if (thief.Loot.Count() > 0)
                            {
                                foreach (Valuable item in thief.Loot)
                                {
                                    police.SeizedArticles.Add(item);
                                }
                                thief.Loot.Clear();
                                Console.WriteLine("I tjuvens ägo: " + thief.Loot.Count() + " saker ");
                                Console.WriteLine("I polisens ägo: " + police.SeizedArticles.Count() + " saker ");
                            }
                            else
                            {
                                Console.WriteLine("Tjuven har inget stöldgods");
                            }
                            thief.InJail = true;
                            thief.TimeOfBooking = DateTime.Now;

                            continue;
                        }
                    }
                    
                }
                inmates.AddRange(thieves.Where(item => item.InJail));
                thieves.RemoveAll(item => item.InJail); 
                



                Thread.Sleep(200);


            }
            while (true);
            
        }
        static void clearTextBox()
        {
            Console.SetCursorPosition(0, 26);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.Write(new string(' ', Console.WindowWidth));
            Console.Write(new string(' ', Console.WindowWidth));
            Console.Write(new string(' ', Console.WindowWidth));
            Console.Write(new string(' ', Console.WindowWidth));
            Console.Write(new string(' ', Console.WindowWidth));


            Console.SetCursorPosition(0, 26);
        }
    }
    class Person
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Xdir { get; set; }
        public int Ydir { get; set; }

        public Person(int x, int y)
        {
            X = x;
            Y = y;
        }
        public void SetDir(int dir)
        {
            switch (dir)
            {
                case 0:
                    Xdir = -1;
                    Ydir = -1;
                    break;
                case 1:
                    Xdir = 0;
                    Ydir = -1;
                    break;
                case 2:
                    Xdir = 1;
                    Ydir = -1;
                    break;
                case 3:
                    Xdir = -1;
                    Ydir = 0;
                    break;
                case 4:
                    Xdir = 1;
                    Ydir = 0;
                    break;
                case 5:
                    Xdir = -1;
                    Ydir = 1;
                    break;
                case 6:
                    Xdir = 0;
                    Ydir = 1;
                    break;
                case 7:
                    Xdir = 1;
                    Ydir = 1;
                    break;

            }
        }
            public void Update()
            {
            X += Xdir;
            Y += Ydir;
            if (X < 1)
            {
                X = 99;
            }
            if (X > 99)
            {
                X = 1;
            }
            if (Y > 24)
            {
                Y = 1;
            }
            if (Y < 1)
            {
                Y = 24;
            }

        }
        public Person()
        {

        }

    }
    class Citizen : Person
    {
        public List<Valuable> Inventory { get; set; }
        
        public Citizen(int x, int y)
        {
            X = x;
            Y = y;
            Inventory = new List<Valuable>();
            Inventory.Add(new Valuable("Nycklar"));
            Inventory.Add(new Valuable("Mobiltelefon"));
            Inventory.Add(new Valuable("Pengar"));
            Inventory.Add(new Valuable("Klocka"));


        }
    }
    class Thief : Person
    {
        public List<Valuable> Loot { get; set; }
        public bool InJail { get; set; }

        public DateTime TimeOfBooking { get; set; }

        public Thief(int x, int y)
        {
            Loot = new List<Valuable>();
            X = x;
            Y = y;
            InJail = false;
        }
    }
    class Police : Person
    {
        public List<Valuable> SeizedArticles { get; set; }
        public Police (int x, int y)
        {
            SeizedArticles = new List<Valuable>();
            X = x;
            Y = y;
        }
    }
           
        
            


    class Valuable
    {
        public string Name { get; set; }
        public Valuable(string name)
        {
            Name = name;
        }
    }
   
}
