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
        static int framesCounter = 0;
        private static bool GameOver = false;
        static bool pause = false;

        //snake status
        private static float speed=10.0f;
        private static Vector2 direction = new Vector2(10, 0);
        private static List<Vector2> SnakeBody;
        private static Vector2 HeadPosition;
        static bool allowMove = false;

        //food status
        private static Vector2 FoodPosition;
        private static Vector2 foodPos;
        private static bool eatfood=false;
        private static int foodAcount=0;


        static void InitGame()
        {

            framesCounter = 0;
            foodAcount = 0;
            GameOver = false;
            pause = false;
            allowMove = false;
            
            // intial window
            Raylib.InitWindow(screenWidth, screenHeight, "snake game window");
            Raylib.SetTargetFPS(10);


            if(Raylib.IsKeyDown(KeyboardKey.Enter))
            {
                //intial status
                GameOver = false;
                eatfood = false;
            }


            if (!GameOver)
            {
                //intial snake position
                HeadPosition = new Vector2(Raylib.GetRandomValue(0, screenWidth / 10) * 10, Raylib.GetRandomValue(0, screenHeight / 10) * 10);
                SnakeBody = new List<Vector2>()
                {
                new Vector2(HeadPosition.X,HeadPosition.Y),
                new Vector2(HeadPosition.X-10,HeadPosition.Y),
                new Vector2(HeadPosition.X-20,HeadPosition.Y)
                };

                //generate food
                Generate();


            }
            

        }

        public static void Update(string[] args)
        {
            if (!GameOver)
            {

                if (Raylib.IsKeyDown(KeyboardKey.Enter))
                { 
                pause = !pause;
                }

                if (!pause)
                {
                    //player input
                    if (Raylib.IsKeyDown(KeyboardKey.Right) && direction != new Vector2(-10, 0))
                    {
                        direction = new Vector2(10, 0);
                    }
                    if (Raylib.IsKeyDown(KeyboardKey.Left) && direction != new Vector2(10, 0))
                    {
                        direction = new Vector2(-10, 0);
                    }
                    if (Raylib.IsKeyDown(KeyboardKey.Up) && direction != new Vector2(0, 10))
                    {
                        direction = new Vector2(0, -10);
                    }
                    if (Raylib.IsKeyDown(KeyboardKey.Down) && direction != new Vector2(0, -10))
                    {
                        direction = new Vector2(0, 10);
                    }

                    //snake movement
                    HeadPosition = SnakeBody[0] + direction;
                    SnakeBody.RemoveAt(SnakeBody.Count - 1);
                    SnakeBody.Insert(0, HeadPosition);

                    //check whether eat food
                    if (Vector2.Distance(SnakeBody[0], FoodPosition) < 10)
                    {
                        eatfood = true;
                        SnakeBody.Add(SnakeBody[SnakeBody.Count - 1]);
                        foodAcount++;
                        Generate();
                    }
                }

                //Game Over
                if (HeadPosition.X == screenWidth ||
                    HeadPosition.X == 0 ||
                    HeadPosition.Y == screenHeight || HeadPosition.Y == 0)
                {
                    GameOver = true; 

                }

            }
            else
            {

                if (Raylib.IsKeyDown(KeyboardKey.Enter))
                {
                    Raylib.CloseWindow();
                    InitGame();
                    GameOver = false;
                }
            }

        }


        public static void Main(string[] args)
        {
            InitGame();

            while (!Raylib.WindowShouldClose())
            {
                Update(args);
                
                Draw();
            }

            Raylib.CloseWindow();

        }

        static void Generate()  
        {
            //intial food position  
            foodPos = new Vector2(Raylib.GetRandomValue(0, screenWidth), Raylib.GetRandomValue(0, screenHeight));
            while (HeadPosition == foodPos)
            {
                foodPos = new Vector2(Raylib.GetRandomValue(0, screenWidth), Raylib.GetRandomValue(0, screenHeight));
            }
            FoodPosition = foodPos;

            if (eatfood)
            {
                FoodPosition = new Vector2(Raylib.GetRandomValue(0, screenWidth/10)*10, Raylib.GetRandomValue(0, screenHeight / 10) * 10);
            }
            
        }

        public static void Draw()
        {
            Raylib.BeginDrawing();

            Raylib.ClearBackground(Color.White);

            if (!GameOver)
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
                Raylib.DrawText("Score:" + foodAcount , 10, 10, 30, Color.Pink);

                if (pause)
                {
                    Raylib.DrawText("GAME PAUSED", screenWidth / 2 - Raylib.MeasureText("GAME PAUSED", 40) / 2, screenHeight / 2 - 40, 40, Color.Gray); 
                }
            }
            else
            {
                Raylib.DrawText("PRESS [ENTER] TO PLAY AGAIN", Raylib.GetScreenWidth() / 2 - Raylib.MeasureText("PRESS [ENTER] TO PLAY AGAIN", 20) / 2, Raylib.GetScreenHeight() / 2 - 50, 20, Color.Gray);
            }

           

            Raylib.EndDrawing();
        }


    }
}