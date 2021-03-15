using System;
using Raylib_cs;
using System.Numerics;
using System.Timers;

namespace slutprojektet
{
    class Enemy
    {
        public int width, height;
        public int column, row;
        public int x, y, gap;

        public Enemy()
        {
            width = 40;
            height = 40;
            column = 16;
            row = 3;
            x = 40;
            y = 40;
            gap = 20;
        }
        public void DrawEnemies()
        {
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < column; j++)
                {
                    Raylib.DrawRectangle(x, y, width, height, Color.GREEN);
                    x += gap + width;
                }
                y += gap + height;
                x = 40;
            }
        }
    }
    class Program   //TODO: PLAYER SHOOTING, ENEMY COLLISION, ENEMY SHOOTING, PLAYER COLLISION, GAMEOVER, NEXT LEVEL, RANDOM ENEMY SKINS (CASE SWITCH?), UNLOCKABLE PLAYER SKINS
    {
        static void Main(string[] args)
        {
            //PREGAME VALUES
            const int screenWidth = 1024;
            const int screenHeight = 768; 
            string gameState = "game";

            //PLAY VALUES
            float playerX = 492;
            float playerY = 708;
            float playerSpeed = 5; 

            //SHOOT VALUES


            //SCREEN VALUES
            Raylib.InitWindow(screenWidth, screenHeight, "Space Inv*ders");
            Raylib.SetTargetFPS(60);               

            while (!Raylib.WindowShouldClose())
            {
                while(gameState == "game")
                {
                    //GET ENEMY CLASS
                    Enemy enemy = new Enemy(); 
                    

                    //MOVEMENT METOD
                    (float pX, float pSpeed) returnedMovement = PlayerMovement(playerX, playerSpeed);
                    playerX = returnedMovement.pX;
                    playerSpeed = returnedMovement.pSpeed;

                    //GRAFIK
                    Raylib.BeginDrawing();
                    Raylib.ClearBackground(Color.BLACK);
                    Raylib.DrawRectangle((int)playerX, (int)playerY, 40, 40, Color.RED);
                    enemy.DrawEnemies(); 
                    Raylib.EndDrawing();
                }
            }
        }
        static (float, float) PlayerMovement(float pX, float pSpeed)
        {
            //KEY MOVEMENT && COLLISION CHECK AGAINST WINDOW
            if (Raylib.IsKeyDown(KeyboardKey.KEY_A) && pX > 0)
            {
                pX -= pSpeed;
            }
            if (Raylib.IsKeyDown(KeyboardKey.KEY_D) && pX < 984)
            {
                pX += pSpeed;
            }
        return (pX, pSpeed);
        }
    }
}

                