using Raylib_cs;
using System;
using System.Numerics;
using System.Collections.Generic;

namespace HelloWorld
{
    class Program
    {
        private static bool eatfood = false;
        private static bool dead = false;
        private static int foodCount = 0;
        const int screenWidth = 640;
        const int screenHeight = 480;
        private static List<Vector2> snakeBody;
        private static Vector2 foodPosition;
        private static Vector2 direction = new Vector2(20, 0); // 初始方向为向右
        private static float speed = 20.0f; // 每次移动的距离

        public struct Snake
        {
            public int size;
            public int dir;
            public int speed;
        }

        static void InitGame()
        {
            // 初始化窗口
            Raylib.InitWindow(screenWidth, screenHeight, "snake game window");
            Raylib.SetTargetFPS(10);

            // 初始化蛇
            snakeBody = new List<Vector2>
            {
                new Vector2(100, 100),
                new Vector2(80, 100),
                new Vector2(60, 100)
            };

            // 初始化食物
            GenerateFood();
        }

        public static void Main()
        {
            InitGame();

            while (!Raylib.WindowShouldClose())
            {
                Update();
                Draw();
            }

            Raylib.CloseWindow();
        }

        static void Update()
        {
            // 更新方向
            if (Raylib.IsKeyDown(KeyboardKey.Right) && direction != new Vector2(-20, 0))
                direction = new Vector2(20, 0);
            if (Raylib.IsKeyDown(KeyboardKey.Left) && direction != new Vector2(20, 0))
                direction = new Vector2(-20, 0);
            if (Raylib.IsKeyDown(KeyboardKey.Up) && direction != new Vector2(0, 20))
                direction = new Vector2(0, -20);
            if (Raylib.IsKeyDown(KeyboardKey.Down) && direction != new Vector2(0, -20))
                direction = new Vector2(0, 20);

            // 移动蛇的身体
            for (int i = snakeBody.Count - 1; i > 0; i--)
            {
                snakeBody[i] = snakeBody[i - 1];
            }
            snakeBody[0] += direction;

            // 检查是否吃到食物
            if (Vector2.Distance(snakeBody[0], foodPosition) < 20)
            {
                eatfood = true;
                snakeBody.Add(snakeBody[snakeBody.Count - 1]); // 增加蛇的长度
                GenerateFood();
            }

            // 检查是否撞到边界
            if (snakeBody[0].X < 0 || snakeBody[0].X >= screenWidth ||
                snakeBody[0].Y < 0 || snakeBody[0].Y >= screenHeight)
            {
                dead = true;
            }

            // 检查是否撞到自己
            for (int i = 1; i < snakeBody.Count; i++)
            {
                if (snakeBody[0] == snakeBody[i])
                {
                    dead = true;
                }
            }

            if (dead)
            {
                // 重新初始化游戏
                InitGame();
            }
        }

        static void Draw()
        {
            Raylib.BeginDrawing();

            Raylib.ClearBackground(Color.White);

            // 绘制食物
            Raylib.DrawRectangleV(foodPosition, new Vector2(20, 20), Color.Red);

            // 绘制蛇
            foreach (Vector2 segment in snakeBody)
            {
                Raylib.DrawRectangleV(segment, new Vector2(20, 20), Color.Gold);
            }

            Raylib.EndDrawing();
        }

        static void GenerateFood()
        {
            Random rand = new Random();
            Vector2 newFoodPosition;
            bool validPosition;

            do
            {
                validPosition = true;
                newFoodPosition = new Vector2(
                    rand.Next(0, screenWidth / 20) * 20,
                    rand.Next(0, screenHeight / 20) * 20
                );

                foreach (var segment in snakeBody)
                {
                    if (Vector2.Distance(newFoodPosition, segment) < 20)
                    {
                        validPosition = false;
                        break;
                    }
                }
            } while (!validPosition);

            foodPosition = newFoodPosition;
        }
    }
}