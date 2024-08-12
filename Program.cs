using Raylib_cs;
using System;
using System.Numerics;
using System.Collections.Generic;
using System.Runtime.CompilerServices;



namespace HelloWorld
{ 

    public abstract class GameObject
    {
        public Vector2 Position { get; protected set; }
        public Vector2 Size { get; protected set; }        
        public int Acount { get; protected set; }


        public GameObject(Vector2 position, Vector2 size,  int acount)
        {
            Position = position;
            Size = size;           
            Acount = acount;
        }


         public abstract void Draw();
    }

    public class Food : GameObject
    {
        public bool FoodEat { get; set; }
        public Food(Vector2 position, int acount) : base(position, new Vector2(10, 10), acount)
        {
            position = new Vector2(130, 130);
            acount = 0;
        }

        public override void Draw()
        {
            Raylib.DrawRectangleV(Position, Size, Color.Green);
        }

        public void Generate(int screenWidth, int screenHeight)
        {

            Position = new Vector2(Raylib.GetRandomValue(0, (screenWidth / 10) - 2) * 10, Raylib.GetRandomValue(0, (screenHeight / 10) - 2) * 10);

        }

        public void Add()
        {
            Acount++;

        }

    }

    public class Spider : GameObject
    {
        private static Vector2 timeBarStart;
        private static Vector2 timeBarEnd;

        private float flashTimer = 0.0f; 
        private bool flashVisible = true;

        private float maxLifeTime = 200f;

        public bool SpiderActive { get; protected set; }

        public float Lifetime { get;  set; }

        public Spider(Vector2 position, int acount) : base(position, new Vector2(10, 10),  acount)
        {
            position = new Vector2(230, 230);
            acount = 1; 
            Lifetime = 200;
            SpiderActive = true;
        }

        public void Update(float deltaTime)           
        {           
            Lifetime -= deltaTime;

            if (Lifetime < maxLifeTime && Lifetime > 50)
            {
                TimeBar();
                Draw();
            }
            else if (Lifetime <= 50 && Lifetime > 0)
            {
                TimeBar(); 

                flashTimer += deltaTime;
                if (flashTimer >= 0.4f)
                {
                    flashVisible = !flashVisible;
                    flashTimer = 0.0f;
                }

                if (flashVisible)
                {                    
                    Draw();
                }
            }
            else if(Lifetime <= 0)
            {
                Die();               
            }

        }

        public void Die()
        {
            Position = new Vector2(1000,1000);
            Lifetime = 0;
            SpiderActive = false;
        }

        public void Respawn(int screenWidth,int screenHeight)
        {
            Position = new Vector2(Raylib.GetRandomValue(0, (screenWidth / 10) - 2) * 10, Raylib.GetRandomValue(0, (screenHeight / 10) - 2) * 10);
            Lifetime = 200;
            SpiderActive = true;
        }

        public void Add()
        {
            Acount++;
        }
        public override void Draw()
        {                    
            Raylib.DrawRectangleV(Position, Size, Color.Blue);            
        }

        public void TimeBar()
        {
            timeBarStart = new Vector2((640 / 2) - Lifetime / 2, 10);
            timeBarEnd = new Vector2((640 / 2) + Lifetime / 2, 10);
            Raylib.DrawLineEx(timeBarStart, timeBarEnd, 10.0f, Color.Brown);
        }

    }



    class Program
    {
        //OOP
        private static Food foods;
        private static Spider spiders;

        //window status
        const int screenWidth = 640;
        const int screenHeight = 480;
        static int level = 0;
        static int score = 0;
        static bool intialgame = true;
        static bool startgame = false;
        static bool endgame = false;
        static bool pause = false;
       
        //snake status
        private static Vector2 direction = new Vector2(10, 0);
        private static List<Vector2> SnakeBody;
        private static Vector2 HeadPosition;
        private static Vector2 PixelSize;

        //spider timer
        private static float spiderSpawnTimer = 0.0f; 
        private static float nextSpawnTime=200; 


        // Timer
        private static float MoveInterval = 0.1f; // Move every 0.1 seconds
        private static float MoveTimer = 0.0f;



        static void InitGame()
        {
            //OOP
            foods = new Food(new Vector2(130, 130),0);
            spiders = new Spider(new Vector2(230, 230), 0);

            level = 0;           
            pause = false;
            PixelSize = new Vector2(10, 10);

            // intial window
            Raylib.InitWindow(screenWidth, screenHeight, "snake game window");                        
            

            //intial snake position           
            HeadPosition = new Vector2(240, 140);
            SnakeBody = new List<Vector2>()
                        {
                        new Vector2(HeadPosition.X,HeadPosition.Y),
                        new Vector2(HeadPosition.X-10,HeadPosition.Y),
                        new Vector2(HeadPosition.X-20,HeadPosition.Y)
                        };
        }


        public static void Update()
        {
            ChangPage();
           //PRESS TAB TO FULL SCREEN
          if (Raylib.IsKeyDown(KeyboardKey.Tab))
          {
                Raylib.ToggleFullscreen();
          }
            

            if (startgame)
            {
                //player input
                if (Raylib.IsKeyDown(KeyboardKey.Right) && direction != new Vector2(-10, 0) )
                {
                    direction = new Vector2(10, 0);

                }
                if (Raylib.IsKeyDown(KeyboardKey.Left) && direction != new Vector2(10, 0) )
                {
                    direction = new Vector2(-10, 0);

                }
                if (Raylib.IsKeyDown(KeyboardKey.Up) && direction != new Vector2(0, 10) )
                {
                    direction = new Vector2(0, -10);

                }
                if (Raylib.IsKeyDown(KeyboardKey.Down) && direction != new Vector2(0, -10))
                {
                    direction = new Vector2(0, 10);

                }

                //move timer                
                MoveTimer += 0.0003f;
                if (MoveTimer >= MoveInterval)
                {
                    MoveTimer = 0.0f;
                    //snake movement
                    HeadPosition = SnakeBody[0] + direction;

                    //cross wall                  
                    if (HeadPosition.X < 0)
                    {
                        HeadPosition.X = screenWidth;
                    }
                    else if (HeadPosition.X > screenWidth)
                    {
                        HeadPosition.X = 0;
                    }
                    else if (HeadPosition.Y > screenHeight)
                    {
                        HeadPosition.Y = 0;
                    }
                    else if (HeadPosition.Y < 0)
                    {
                        HeadPosition.Y = screenHeight;
                    }


                    SnakeBody.RemoveAt(SnakeBody.Count - 1);
                    SnakeBody.Insert(0, HeadPosition);



                    //check whether eat food

                    //Vector2 foodPostion = foods.Position; //?
                    
                    if (Vector2.Distance(SnakeBody[0], foods.Position) < 10)
                    {                        
                        foods.FoodEat = true;                        
                        SnakeBody.Add(SnakeBody[SnakeBody.Count - 1]);
                        foods.Add();
                        level = foods.Acount + spiders.Acount;
                        Generate();

                        MoveInterval = 0.1f / (1 + (level * 0.1f));
                    }

                    //check whether eat spider
                    if (Vector2.Distance(SnakeBody[0], spiders.Position) < 10)
                    {
                        spiders.Die();
                        spiders.Add();
                        
                        SnakeBody.Add(SnakeBody[SnakeBody.Count - 1]);                        
                        level = foods.Acount + spiders.Acount;                       
                        MoveInterval = 0.1f / (1 + (level * 0.1f));
                    }

                    //snakehead touch snakebody
                    for (int i = 1; i < SnakeBody.Count; i++)
                    {
                        if (SnakeBody[0] == SnakeBody[i])
                        {
                            endgame = true;
                            startgame = false;
                        }
                    }
                }



               // 
                spiders.Update(0.01f);
                spiderSpawnTimer += 0.002f;
                
                
                if (spiderSpawnTimer >= nextSpawnTime )
                {
                    if (spiders.Lifetime <= 0)
                    {
                    spiders.Respawn(screenWidth, screenHeight);

                        do
                        {
                        spiders.Respawn(screenWidth, screenHeight);

                        } while (SnakeBody.Contains(spiders.Position) || spiders.Position == foods.Position);

                    spiderSpawnTimer = 0.0f; 
                    nextSpawnTime = Raylib.GetRandomValue(150, 300); 
                    }
                }

            }
        }

        static void ChangPage()
        {
            //intial page
            if (intialgame)
            {
                if (Raylib.IsKeyDown(KeyboardKey.Enter))
                {
                    //intial status                  
                    intialgame = false;
                    startgame = true;
                }
                return;
            }

            // game page
            if (startgame)
            {

                if (Raylib.IsKeyDown(KeyboardKey.Space))
                {
                    pause = !pause;
                }
                return;
            }

            //gameover page
            if (endgame)
            {

                if (Raylib.IsKeyDown(KeyboardKey.Enter))
                {
                    endgame = false;
                    intialgame = true;
                    Raylib.CloseWindow();
                    InitGame();
                }
                return;
            }

        }



        public static void Main(string[] args)
        {
            InitGame();
            
            while (!Raylib.WindowShouldClose())
            {
                Update();

                 Draw();
            }
            
            Raylib.CloseWindow();

        }

        static void Generate()
        {

            if (intialgame)
            {
                intialgame = false;
            }

            //food generation
            if (foods.FoodEat)
            {
                foods.FoodEat = false;

                foods.Generate(screenWidth, screenHeight);

                //make foodPos except snakePos
                do
                {
                    foods.Generate(screenWidth, screenHeight);

                } while (SnakeBody.Contains(foods.Position));

            }

            //Snake generation
            
        }

        public static void Draw()
        {
            Raylib.BeginDrawing();

            Raylib.ClearBackground(Color.White);

            if (intialgame)
            {

                Raylib.DrawText("PRESS [ENTER] TO PLAY GAME", Raylib.GetScreenWidth() / 2 - Raylib.MeasureText("PRESS [ENTER] TO PLAY GAME", 20) / 2, Raylib.GetScreenHeight() / 2 - 50, 20, Color.DarkGray);
                Raylib.DrawText("PRESS [TAB] TO FULL SCREEN", Raylib.GetScreenWidth() / 2 - Raylib.MeasureText("PRESS [TAB] TO FULL SCREEN", 20) / 2, Raylib.GetScreenHeight() / 2 , 20, Color.Gray);

            }
            else if (startgame)
            {
                #region GridLines
                // Draw grid lines
                /*
                for (int i = 0; i < Raylib.GetScreenWidth() / 10 + 1; i++)
                {
                    Raylib.DrawLineV((Vector2){ 10* i +offset.x / 2, offset.y / 2}, (Vector2){ PixelSize* i +offset.x / 2, screenHeight - offset.y / 2}, Color.Gray);
                }
                for (int i = 0; i < Raylib.GetScreenHeight() / 10 + 1; i++)
                {
                    Raylib.DrawLineV((Vector2)(0, 10 * i), (Vector2)(0, 10 * i), Color.Gray);
                }
                */
                #endregion

                // draw food
                //Raylib.DrawRectangleV(FoodPosition, new Vector2(10, 10), Color.Green);
                foods.Draw();

                //DRAW SPIDER
               // spiders.Draw();
                
                // draw snake
                for (int i = 0; i<SnakeBody.Count; i++)
                    {
                    Vector2 segment = SnakeBody[i];
                    //random snake color
                    Color segmentColor = new Color(0, Raylib.GetRandomValue(0, 255), Raylib.GetRandomValue(0, 255), 255);
                    Raylib.DrawRectangleV(segment, new Vector2(10, 10), segmentColor);
                    }

                //draw Score counting
                score= foods.Acount + 3*spiders.Acount;
                Raylib.DrawText("Score:" + score, 10, 10, 20, Color.Pink);
                Raylib.DrawText("Lvel:" + level, 10, 30, 20, Color.Pink);
                Raylib.DrawText("Spider:" + spiders.Acount, 10, 50, 20, Color.Pink);
                    
                if (pause)
                {
                    Raylib.DrawText("GAME PAUSED", screenWidth / 2 - Raylib.MeasureText("GAME PAUSED", 40) / 2, screenHeight / 2 - 40, 40, Color.Gray);
                }
            }
            else if (endgame)
            {
                Raylib.DrawText("GAMEOVER! PRESS [ENTER] TO PLAY AGAIN", Raylib.GetScreenWidth() / 2 - Raylib.MeasureText("GAMEOVER! PRESS [ENTER] TO PLAY AGAIN", 20) / 2, Raylib.GetScreenHeight() / 2 - 50, 20, Color.Gray);
            }
            
            




                Raylib.EndDrawing();
        }


    }
}