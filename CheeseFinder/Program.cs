using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheeseFinder
{

    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Dog Mayhem";
            //fit the stuff
            Console.SetWindowSize(75, 30);
          
            Intro();
            
            new BallSniffer().PlayGame();

        }
        /// <summary>
        /// The intro with ascii graphics, cheap and effective
        /// </summary>
        public static void Intro()
        {
            
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(@"________                     _____                .__                    
\______ \   ____   ____     /     \ _____  ___.__.|  |__   ____   _____  
 |    |  \ /  _ \ / ___\   /  \ /  \\__  \<   |  ||  |  \_/ __ \ /     \ 
 |    `   (  <_> ) /_/  > /    Y    \/ __ \\___  ||   Y  \  ___/|  Y Y  \
/_______  /\____/\___  /  \____|__  (____  / ____||___|  /\___  >__|_|  /
        \/      /_____/           \/     \/\/          \/     \/      \/");
            System.Threading.Thread.Sleep(1500);
            Console.WriteLine(@"             In collaboration with noCat studios
We will go make our own games with dogs, beer and strippers.");
            Console.WriteLine();
            System.Threading.Thread.Sleep(3500);
            Console.WriteLine(@"ps. the closest you'll see to a stripper in this game is the 'M' in Mayhem. Also no actual dogs 
or balls were harmed in the making of this crazy game");
            Console.WriteLine();
            Console.ResetColor();
            System.Threading.Thread.Sleep(6500);
            Console.WriteLine(@"You're a dog, when you get the ball, you get more excited. If you 
stay away from the ball you'll get bored.    Oh look there's a ball (~), you see a ball? Go get the ball.");
            System.Threading.Thread.Sleep(6000);
            Console.WriteLine("ps there are vacuums chasing you. Don't ask why, just get the ball.");
            Console.WriteLine();
            System.Threading.Thread.Sleep(1800);
            Console.WriteLine(@"pss those are roomba vacuums, very smart! but a clumsy human is spilling beer on one 
every time they cheer you for collecting a certain amount of balls, how high can you go?");
            Console.WriteLine("Press any key to continue:"); Console.ReadKey();
        }
        /// <summary>
        /// the point on the array class, holds enums status, and assigns grid
        /// </summary>
        public class Point
        {
            /// <summary>
            /// the enums
            /// </summary>
            public enum PointStatus
            {
                Empty = 1,
                Ball,
                Dog,
                Vacuum,
                VacuumAndBall

            }
            //the x value
            public int X { get; set; }
            //the y value
            public int Y { get; set; }
            //grabs enum for assigning to grid
            public PointStatus Status { get; set; }
            //grid point
            public Point(int x, int y)
            {
                this.X = x;
                this.Y = y;
                //make sure to assign to empty so it knows for dogs and balls
                this.Status = PointStatus.Empty;
            }
        }
        /// <summary>
        /// the dog class
        /// </summary>
        public class Dog
        {
            public Point Position { get; set; }
            public int Energy { get; set; }
            //whether the vacuum scared the dog off
            public bool HasBeenFreakedOut { get; set; }
            public Dog()
            {
                //give energy until boredom a value
                this.Energy = 50;
                this.HasBeenFreakedOut = false;
            }
        }
        /// <summary>
        /// class for vacuum, we build it later
        /// </summary>
        public class Vacuum
        {
            public Point Position { get; set; }
            public Vacuum()
            { }
        }
        /// <summary>
        /// We are building everything in the ballsniffer
        /// </summary>
        public class BallSniffer
        {
            //a rng for all sorts of things
            Random rng = new Random(DateTime.Now.Millisecond);
            // its going to be a (x, y) grid
            public Point[,] Grid { get; set; }
            //get dog
            public Dog Dog { get; set; }
            //get a ball
            public Point Ball { get; set; }
            //get the ball count
            public int BallCount { get; set; }
            //get how moves have been made
            public int Moves { get; set; }
            //a list of enemises that might appear
            public List<Vacuum> Vacuums { get; set; }

            public BallSniffer()
            {
                //build the grid, you can assign any value to these
                this.Grid = new Point[5, 25];
                this.Moves = 0;
                this.Dog = new Dog();
                this.BallCount = 0;
                this.Vacuums = new List<Vacuum>();
                //assigns each location an x and y
                for (int y = 0; y < this.Grid.GetLength(1); y++)
                {
                    for (int x = 0; x < this.Grid.GetLength(0); x++)
                    {
                        this.Grid[x, y] = new Point(x, y);
                    }
                }
                //random gens a dog on the map
                this.Dog.Position = this.Grid[rng.Next(0, this.Grid.GetLength(0)), rng.Next(0, this.Grid.GetLength(1))];
                this.Dog.Position.Status = Point.PointStatus.Dog;
                //calls the ball placer
                PlaceBall();

            }
            /// <summary>
            /// holds the drawing of the grid and chars
            /// </summary>
            public void DrawGrid()
            {
                
                Console.Clear();
                for (int y = 0; y < this.Grid.GetLength(1); y++)
                {
                    for (int x = 0; x < this.Grid.GetLength(0); x++)
                    {
                        switch (this.Grid[x, y].Status)
                        {
                                //empty draw an invisible border
                            case Point.PointStatus.Empty: Console.ForegroundColor = ConsoleColor.Black; Console.Write("[      ]"); Console.ResetColor();
                                break;
                                //the dog
                            case Point.PointStatus.Dog: Console.ForegroundColor = ConsoleColor.DarkGray; Console.Write("[u(oo)u]"); Console.ResetColor();
                                break;
                                //the ball
                            case Point.PointStatus.Ball: Console.ForegroundColor = ConsoleColor.Yellow; Console.Write("[  (~) ]"); Console.ResetColor();
                                break;
                                //the vacuum
                            case Point.PointStatus.Vacuum:
                            case Point.PointStatus.VacuumAndBall:
                                Console.Write("[vacuum]");
                                break;
                            default:
                                break;

                        }
                    }
                    //for clarity
                    Console.WriteLine();
                }
            }
            /// <summary>
            /// grabs the validated user input
            /// </summary>
            /// <returns>the uinput for validation</returns>
            private ConsoleKey GetUserMove()
            {
                bool inputValid = false;
                //while valid, accepts user input and adds to the move counter
                while (!inputValid)
                {
                    ConsoleKeyInfo inputKey = Console.ReadKey(true);
                    if (inputKey.Key == ConsoleKey.LeftArrow || inputKey.Key == ConsoleKey.RightArrow || inputKey.Key == ConsoleKey.UpArrow || inputKey.Key == ConsoleKey.DownArrow)
                    {
                        if (ValidMove(inputKey.Key))
                        {
                            inputValid = true;
                            this.Moves++;
                            return inputKey.Key;
                        }
                    }
                    Console.WriteLine();
                    //if its nto an arrow key
                    Console.WriteLine("There isn't a dog in this dog world that knows that trick!");
                }
                //you should never reach this point
                return ConsoleKey.DownArrow;
            }
            /// <summary>
            /// validates user input
            /// </summary>
            /// <param name="input">what the user is inputting</param>
            /// <returns>true or false if user is unoutting an arrow key</returns>
            public bool ValidMove(ConsoleKey input)
            {
                int currentMouseX = this.Dog.Position.X;
                int currentMouseY = this.Dog.Position.Y;
                switch (input)
                {
                        //our grid has no negative numbers
                    case ConsoleKey.UpArrow: currentMouseY--; return currentMouseY >= 0;
                        //adaptable to different sizes
                    case ConsoleKey.DownArrow: currentMouseY++; return currentMouseY < this.Grid.GetLength(1);
                    case ConsoleKey.LeftArrow: currentMouseX--; return currentMouseX >= 0;
                    case ConsoleKey.RightArrow: currentMouseX++; return currentMouseX < this.Grid.GetLength(0);
                    default:
                        return false;
                }
            }
            /// <summary>
            /// moves the dog
            /// </summary>
            /// <param name="input"></param>
            public void MoveDog(ConsoleKey input)
            {
                int moveX = this.Dog.Position.X;
                int moveY = this.Dog.Position.Y;
                this.Dog.Energy--;
                    switch (input)
                    {
                            //moves the dog based on input
                        case ConsoleKey.DownArrow:
                            moveY++;
                            break;
                        case ConsoleKey.UpArrow:
                            moveY--;
                            break;
                        case ConsoleKey.LeftArrow:
                            moveX--;
                            break;
                        case ConsoleKey.RightArrow:
                            moveX++;
                            break;
                        default:
                            break;
                    }
                //adds a ball and energy very rarely for balance
                    if (rng.Next(10) < 1)
                    {
                        this.Dog.Energy += 1;
                        BallCount++;
                        Console.WriteLine();
                        Console.BackgroundColor = ConsoleColor.DarkBlue;
                        Console.WriteLine("              You found a random ball!");
                        Console.ResetColor();

                    }
                //Going to check for conditions to draw the dog to new position
                Point futureDogPosition = this.Grid[moveX, moveY];
                //grabs the ball and takes over space.
                if (futureDogPosition.Status == Point.PointStatus.Ball)
                {

                    this.BallCount++;
                    this.Dog.Energy += 10;
                    PlaceBall();
                    if (BallCount % 2 == 0)
                    {
                        AddVacuum();
                    }
                    futureDogPosition.Status = Point.PointStatus.Dog;
                }
                    //if the vacuum scared the dog
                else if (futureDogPosition.Status == Point.PointStatus.Vacuum)
                {
                    this.Dog.HasBeenFreakedOut = true;
                    futureDogPosition.Status = Point.PointStatus.Vacuum;
                }
                    //can move, it must be empty
                else
                {
                    futureDogPosition.Status = Point.PointStatus.Dog;
                }
                //move the dog into spot
                this.Dog.Position.Status = Point.PointStatus.Empty;
                this.Dog.Position = futureDogPosition;
            }
            /// <summary>
            /// places ball on map
            /// </summary>
            void PlaceBall()
            {
                bool isEmpty = false;
                //checks if space is empty
                while (!isEmpty)
                {
                    Point ballChecker = this.Grid[rng.Next(0, Grid.GetLength(0)), rng.Next(0, Grid.GetLength(1))];
                    //if it is empty we place the ball
                    if (ballChecker.Status == Point.PointStatus.Empty)
                    {
                        isEmpty = true;
                        ballChecker.Status = Point.PointStatus.Ball;
                        this.Ball = ballChecker;

                    }
                }
            }
            /// <summary>
            /// 
            /// </summary>
            public void AddVacuum()
            {
                Vacuum roomba = new Vacuum();
                this.Vacuums.Add(roomba);
                PlaceVacuum(roomba);

            }
            /// <summary>
            /// places a vacuum
            /// </summary>
            /// <param name="vacuum"></param>
            private void PlaceVacuum(Vacuum vacuum)
            {
                bool isEmpty = false;
                //ifs its empty we can place it
                while (!isEmpty)
                {
                    Point vacuumBallChecker = this.Grid[rng.Next(0, this.Grid.GetLength(0)), rng.Next(0, this.Grid.GetLength(1))];
                    //checks for emptiness and places it
                    if (vacuumBallChecker.Status == Point.PointStatus.Empty)
                    {
                        isEmpty = true;
                        vacuumBallChecker.Status = Point.PointStatus.Vacuum;
                        vacuum.Position = vacuumBallChecker;

                    }
                }

            }
            /// <summary>
            /// the roomba has advanced moving skills by being able to move to the diagonally
            /// </summary>
            /// <param name="vacuum"></param>
            void MoveVacuum(Vacuum vacuum)
            {
                //since they are advanced moving, there is less of a chance of moving in dogs direction
                if (rng.Next(10) >= 4)
                {
                    //the numbers for finding vacuum closer to dog
                    int xVacuumHunt = this.Dog.Position.X - vacuum.Position.X;
                    int yVacuumHunt = this.Dog.Position.Y - vacuum.Position.Y;
                    //normal 4 way movement
                    bool tryLeft = (xVacuumHunt < 0);
                    bool tryRight = (xVacuumHunt > 0);
                    bool tryUp = (yVacuumHunt < 0);
                    bool tryDown = (yVacuumHunt > 0);
                    //diagonally
                    bool tryDownLeft = (xVacuumHunt < 0) && (yVacuumHunt > 0);
                    bool tryDownRight = (xVacuumHunt > 0) && (yVacuumHunt > 0);
                    bool tryUpLeft = (yVacuumHunt < 0) && (xVacuumHunt < 0);
                    bool tryUpRight = (yVacuumHunt < 0) && (xVacuumHunt > 0);
                    //where the vacuum is going
                    Point targetPosition = vacuum.Position;
                    bool validMove = false;
                    while (!validMove && tryDown || tryLeft || tryRight || tryUp || tryDownLeft ||tryDownRight ||tryUpLeft ||tryUpRight)
                    {
                        int vacuumMoveX = vacuum.Position.X;
                        int vacuumMoveY = vacuum.Position.Y;
                        if (tryUp)
                        {
                            targetPosition = Grid[vacuumMoveX, --vacuumMoveY];
                            tryUp = false;
                        }
                        else if (tryDown)
                        {
                            targetPosition = Grid[vacuumMoveX, ++vacuumMoveY];
                            tryDown = false;
                        }
                        else if (tryRight)
                        {
                            targetPosition = Grid[++vacuumMoveX, vacuumMoveY];
                            tryRight = false;
                        }
                        else if (tryLeft)
                        {
                            targetPosition = Grid[--vacuumMoveX, vacuumMoveY];
                            tryLeft = false;
                        }
                        else if(tryUpLeft)
                        {
                            targetPosition = Grid[--vacuumMoveX, --vacuumMoveY];
                            tryUpLeft = false;
                        }
                        else if (tryUpRight)
                        {
                            targetPosition = Grid[++vacuumMoveX, --vacuumMoveY];
                            tryUpRight = false;
                        }
                        else if (tryDownLeft)
                        {
                            targetPosition = Grid[--vacuumMoveX, ++vacuumMoveY];
                            tryDownLeft = false;
                        }
                        else if (tryDownRight)
                        {
                            targetPosition = Grid[++vacuumMoveX, ++vacuumMoveY];
                            tryDownRight = false;
                        }
                        validMove = IsValidVacuumMove(targetPosition);
                    }
                    //checks for a ball
                    if (vacuum.Position.Status == Point.PointStatus.VacuumAndBall)
                    {
                        vacuum.Position.Status = Point.PointStatus.Ball;
                    }
                    else
                    {
                        vacuum.Position.Status = Point.PointStatus.Empty;
                    }
                    //gets the dog
                    if (targetPosition.Status == Point.PointStatus.Dog)
                    {
                        this.Dog.HasBeenFreakedOut = true;
                        targetPosition.Status = Point.PointStatus.Vacuum;
                    }
                        //takes over ball spot for a move
                    else if (targetPosition.Status == Point.PointStatus.Ball)
                    {
                        targetPosition.Status = Point.PointStatus.VacuumAndBall;
                    }
                        //its empty vacuum ahead
                    else
                    {
                        targetPosition.Status = Point.PointStatus.Vacuum;
                    }
                    vacuum.Position = targetPosition;
                }
            }
            /// <summary>
            /// checks the validity of vacuum move
            /// </summary>
            /// <param name="targetPosition"></param>
            /// <returns></returns>
            bool IsValidVacuumMove(Point targetPosition)
            {
                return (targetPosition.Status == Point.PointStatus.Empty || targetPosition.Status == Point.PointStatus.Dog);
            }
            /// <summary>
            /// lets play a dog game
            /// </summary>
            public void PlayGame()
            {
                //while the dog is good to go
                while (this.Dog.Energy > 0 && !this.Dog.HasBeenFreakedOut)
                {
                    DrawGrid();
                    Console.WriteLine();
                    Console.Write("Energy Storage Remaining: {0}                  Balls collected: {1}", this.Dog.Energy, this.BallCount);
                    Console.WriteLine();
                    MoveDog(GetUserMove());
                    foreach (Vacuum vacuum in this.Vacuums)
                    {
                        MoveVacuum(vacuum);
                    }
                }
                DrawGrid();
                if(BallCount == 6 || BallCount == 16 || BallCount == 26)
                {
                    this.Vacuums.Remove(Vacuums.First());
                }
                if (Dog.HasBeenFreakedOut || this.Dog.Energy == 0)
                {
                    Console.WriteLine();
                    Console.WriteLine("You collected {0} balls in {1} moves.", BallCount, Moves);
                    if(BallCount > 30)
                    {
                        System.Threading.Thread.Sleep(1500);
                        Console.WriteLine();
                        Console.WriteLine("You're a dog god!");
                        System.Threading.Thread.Sleep(1500);
                    }
                    else if(BallCount >= 15)
                    {
                        System.Threading.Thread.Sleep(1500);
                        Console.WriteLine();
                        Console.WriteLine("Your owner is still scratching his head, trying to figure out where {0} balls, and an army of vacuums came from.", BallCount);
                        System.Threading.Thread.Sleep(1500);
                    }
                    else if(BallCount < 15)
                    {
                        System.Threading.Thread.Sleep(1500);
                        Console.WriteLine();
                        Console.WriteLine("Do you not like balls?  Are you a real dog? or a cat?");
                        System.Threading.Thread.Sleep(1500);
                    }
                    //lets replay
                    Console.WriteLine("Would you like to play again? (Y/N)");
                    string uinput = Console.ReadLine();
                    if(uinput.ToLower() == "y")
                    {
                        new BallSniffer().PlayGame();
                    }
                    else
                    {
                        Console.WriteLine("thanks for playing!");
                    }
                    Console.ReadKey();
                }
            }

        }
    }
    
}
