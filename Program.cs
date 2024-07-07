   using Raylib_cs;
using System.Numerics;
using System.Collections.Generic;

namespace HelloWorld;



class Program
{



    private bool eatfood = false;
    private bool dead = false;
    private int foodCount = 0;
    const int screenWidth = 800;
    const int screenHeight = 450;
    private List<GameObject> body;
    struct Snake
    {
        int size;
        int dir;
        int speed;
        Vector2 postion;
    }

    void InitGame () 
    {

        
       


    }



    public static void Main()
    {
        
        // init page
        Raylib.InitWindow(screenWidth, screenHeight, "snake game window");
        Raylib.SetTargetFPS(60);
       

        Vector2 ballPosition = new Vector2(2,2) ;

        while (!Raylib.WindowShouldClose())
        {
            if (Raylib.IsKeyDown(KeyboardKey.Right)) 
                ballPosition.X += 2.0f;
            if (Raylib.IsKeyDown(KeyboardKey.Left)) ballPosition.X  -= 2.0f;
            if (Raylib.IsKeyDown(KeyboardKey.Up)) ballPosition.Y -= 2.0f;
            if (Raylib.IsKeyDown(KeyboardKey.Down)) ballPosition.Y += 2.0f;
            //----------------------------------------------------------------------------------

            // Draw
            //----------------------------------------------------------------------------------
            Raylib.BeginDrawing();

            Raylib.ClearBackground(Color.White);

            Raylib.DrawText("move the ball with arrow keys", 10, 10, 20, Color.Black);

            Raylib.DrawCircleV(ballPosition, 50, Color.Blue);

            Raylib.EndDrawing();
        }

        Raylib.CloseWindow();
    }



}