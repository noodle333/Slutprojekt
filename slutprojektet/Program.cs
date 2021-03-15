using System;
using Raylib_cs;
using System.Numerics;
using System.Timers;

namespace slutprojektet
{
    class Program   //TODO: ENEMY ARRAY; PLAYER SHOOTING; ENEMY MOVEMENT; BACKGROUND + RANDOM ENEMY SKINS(CASE SWITCH?); CLASS FOR ENEMY SHOOTING; START MENU; GAME OVER SCREEN; UNLOCKABLES;
    {
        static void Main(string[] args)
        {
            //PREGAME VALUES
            const int screenWidth = 800;
            const int screenHeight = 600;
            string gameState = "game";

            //PLAY VALUES
            float playerX = 380;
            float playerY = 540;
            float playerSpeed = 5; 

            //SCREEN VALUES
            Raylib.InitWindow(screenWidth, screenHeight, "Space InvaderZ no copyright intended");
            Raylib.SetTargetFPS(60);               

            while (!Raylib.WindowShouldClose())
            {
                while(gameState == "game")
                {
                    //MOVEMENT METOD
                    (float pX, float pSpeed) returnedMovement = PlayerMovement(playerX, playerSpeed);
                    playerX = returnedMovement.pX;
                    playerSpeed = returnedMovement.pSpeed;

                    //GRAFIK
                    Raylib.BeginDrawing();
                    Raylib.ClearBackground(Color.BLACK);
                    Raylib.DrawRectangle((int)playerX, (int)playerY, 40, 40, Color.RED);
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
            else if (Raylib.IsKeyDown(KeyboardKey.KEY_D) && pX < 760)
            {
                pX += pSpeed;
            }
    
        return (pX, pSpeed);
        }   
    }
}

                