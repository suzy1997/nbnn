using Raylib_cs;
using System;
using System.Numerics;
using System.Collections.Generic;
using System.Runtime.CompilerServices;


namespace HelloWorld
{


    class Program
    {
        //window status
        const int screenWidth = 640;
        const int screenHeight = 480;
        static int level = 0;

        static bool intialgame = true;
        static bool startgame = false;
        static bool endgame = false;
        static bool pause = false;

        //snake status
        private static float speed=10.0f;
        private static Vector2 direction = new Vector2(10, 0);
        private static List<Vector2> SnakeBody;
        private static Vector2 HeadPosition;
        static bool CantTurn = false;

        //food status
        private static Vector2 FoodPosition;
        private static Vector2 foodPos;
        private static bool eatfood=false;
        private static int foodAcount=0;

        // Timer
        private static float MoveInterval = 0.1f; // Move every 0.1 seconds
        private static float MoveTimer = 0.0f;

        static void InitGame()
        {

            level = 0;
            foodAcount = 0;
           
            pause = false;
            CantTurn = false;
            
            // intial window
            Raylib.InitWindow(screenWidth, screenHeight, "snake game window");
            // !when press keyboard beacause the fps value too high, is not senstive 
            Raylib.SetTargetFPS(10);

                


            //intial snake position
            //HeadPosition = new Vector2(Raylib.GetRandomValue(0, screenWidth / 10) * 10, Raylib.GetRandomValue(0, screenHeight / 10) * 10);
            HeadPosition = new Vector2(240, 140);
            SnakeBody = new List<Vector2>()
                        {
                        new Vector2(HeadPosition.X,HeadPosition.Y),
                        new Vector2(HeadPosition.X-10,HeadPosition.Y),
                        new Vector2(HeadPosition.X-20,HeadPosition.Y)
                        };

            //generate food
            FoodPosition = new Vector2(130, 130);

        }


        public static void Update()
        {
            ChangPage();


            if (startgame)
            {
                //player input
                if (Raylib.IsKeyDown(KeyboardKey.Right) && direction != new Vector2(-10, 0) && !CantTurn)
                {
                    direction = new Vector2(10, 0);

                }
                if (Raylib.IsKeyDown(KeyboardKey.Left) && direction != new Vector2(10, 0) && !CantTurn)
                {
                    direction = new Vector2(-10, 0);

                }
                if (Raylib.IsKeyDown(KeyboardKey.Up) && direction != new Vector2(0, 10) && !CantTurn)
                {
                    direction = new Vector2(0, -10);

                }
                if (Raylib.IsKeyDown(KeyboardKey.Down) && direction != new Vector2(0, -10) && !CantTurn)
                {
                    direction = new Vector2(0, 10);

                }

                //move timer
                MoveTimer += Raylib.GetFrameTime();
                if (MoveTimer >= MoveInterval)
                {
                    MoveTimer = 0.0f;
                    //snake movement
                    HeadPosition = SnakeBody[0] + direction;

                    //cross wall
                    if (HeadPosition.X < 0 || HeadPosition.X > screenWidth || HeadPosition.Y > screenHeight || HeadPosition.Y < 0)
                    {
                        CantTurn = true;
                    }
                    else
                    { CantTurn = false; }

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

                    /*switch (HeadPosition.X)
                    {
                        case -10:
                            HeadPosition.X = screenWidth;
                            break;
                        case screenWidth:
                            HeadPosition.X = -10;
                            break;

                    }

                    switch (HeadPosition.Y)
                    {
                        case -10:
                            HeadPosition.Y = screenHeight;
                            break;
                        case screenHeight:
                            HeadPosition.Y = -10;
                            break;

                    }*/


                    SnakeBody.RemoveAt(SnakeBody.Count - 1);
                    SnakeBody.Insert(0, HeadPosition);



                    //check whether eat food
                    if (Vector2.Distance(SnakeBody[0], FoodPosition) < 10)
                    {
                        eatfood = true;
                        SnakeBody.Add(SnakeBody[SnakeBody.Count - 1]);
                        foodAcount++;
                        level = foodAcount;
                        //level = foodAcount / 4;
                        Generate();
                        //Raylib.SetTargetFPS(10 * (1 + level / 2));
                        

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
                        startgame = true;
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

            if (eatfood)
            {

               
                eatfood= false;
                foodPos = new Vector2(Raylib.GetRandomValue(0, (screenWidth / 10) - 2) * 10,Raylib.GetRandomValue(0, (screenHeight / 10) - 2) * 10);

                //make foodPos except snakePos
                do
                {
                    foodPos = new Vector2(Raylib.GetRandomValue(0, (screenWidth / 10) - 2) * 10, Raylib.GetRandomValue(0, (screenHeight / 10) - 2) * 10);
                } while (SnakeBody.Contains(foodPos));


                FoodPosition = foodPos;

                //FoodPosition = new Vector2(Raylib.GetRandomValue(0, screenWidth/10)*10, Raylib.GetRandomValue(0, screenHeight / 10) * 10);
            }
            
        }

        public static void Draw()
        {
            Raylib.BeginDrawing();

            Raylib.ClearBackground(Color.White);


            if (intialgame)
            {
                Raylib.DrawText("PRESS [ENTER] TO PLAY GAME", Raylib.GetScreenWidth() / 2 - Raylib.MeasureText("PRESS [ENTER] TO PLAY GAME", 20) / 2, Raylib.GetScreenHeight() / 2 - 50, 20, Color.Gray);

            }
            else  if (startgame)
            {
                // draw food
                Raylib.DrawRectangleV(FoodPosition, new Vector2(10, 10), Color.Green);

                // draw snake
                for (int i = 0; i < SnakeBody.Count; i++)
                {
                    Vector2 segment = SnakeBody[i];
                    //random snake color
                    Color segmentColor = new Color(0, Raylib.GetRandomValue(0, 255), Raylib.GetRandomValue(0, 255), 255);
                    Raylib.DrawRectangleV(segment, new Vector2(10, 10), segmentColor);
                }

                //draw Score counting
                Raylib.DrawText("Score:" + foodAcount, 10, 10, 30, Color.Pink);
                Raylib.DrawText("Lvel:" + level, 10, 40, 30, Color.Pink);
                Raylib.DrawText("MoveInterval:" + MoveInterval, 10, 70, 30, Color.Pink);

                if (pause)
                {
                    Raylib.DrawText("GAME PAUSED", screenWidth / 2 - Raylib.MeasureText("GAME PAUSED", 40) / 2, screenHeight / 2 - 40, 40, Color.Gray);
                }
            }
            else if(endgame)
            {
                Raylib.DrawText("GAMEOVER! PRESS [ENTER] TO PLAY AGAIN", Raylib.GetScreenWidth() / 2 - Raylib.MeasureText("GAMEOVER! PRESS [ENTER] TO PLAY AGAIN", 20) / 2, Raylib.GetScreenHeight() / 2 - 50, 20, Color.Gray);
            }




                Raylib.EndDrawing();
        }


    }
}