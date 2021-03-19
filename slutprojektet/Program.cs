using System;
using Raylib_cs;
using System.Numerics;
using System.Timers;

namespace slutprojektet
{
    class Program   //TODO: PLAYER SHOOTING, ENEMY COLLISION, ENEMY SHOOTING, PLAYER COLLISION, GAMEOVER, NEXT LEVEL, RANDOM ENEMY SKINS (CASE SWITCH?), UNLOCKABLE PLAYER SKINS
    {
        static void Main(string[] args)
        {
            //PREGAME VALUES
            const int screenWidth = 1024;
            const int screenHeight = 768; 
            string gameState = "start";
            int menuTarget = 1;

            //COLOR VALUES
            Color startColor = Color.BLACK;
            Color optionsColor = Color.BLACK;
            Color exitColor = Color.BLACK;

            //PLAY VALUES
            float playerX = 492;
            float playerY = 708;
            float playerSpeed = 5; 

            //SHOOT VALUES
            bool readyToShoot = true;
            float bulletX = 0;
            float bulletY = 700;


            //SCREEN VALUES
            Raylib.InitWindow(screenWidth, screenHeight, "Space Invaders");
            Raylib.SetTargetFPS(60);               

            while (!Raylib.WindowShouldClose())
            {
                while (gameState == "start")
                {
                    //GET MENUTARGET METHOD
                    menuTarget = MenuTarget(menuTarget);

                    //MENU LOGIC 
                    if (menuTarget == 1)
                    {
                        startColor = Color.WHITE;
                        if (Raylib.IsKeyPressed(KeyboardKey.KEY_SPACE) || Raylib.IsKeyPressed(KeyboardKey.KEY_ENTER))
                        {
                            gameState = "game";        
                        }
                    }
                    else
                    {
                        startColor = Color.BLACK;
                    }

                    if (menuTarget == 2)
                    {
                        optionsColor = Color.WHITE;
                        if (Raylib.IsKeyPressed(KeyboardKey.KEY_SPACE) || Raylib.IsKeyPressed(KeyboardKey.KEY_ENTER))
                        {
                            gameState = "options";        
                        }
                    }
                    else
                    {
                        optionsColor = Color.BLACK;
                    }

                    if (menuTarget == 3)
                    {
                        exitColor = Color.WHITE;
                        if (Raylib.IsKeyPressed(KeyboardKey.KEY_SPACE) || Raylib.IsKeyPressed(KeyboardKey.KEY_ENTER))
                        {
                            break;        
                        }
                    }
                    else
                    {
                        exitColor = Color.BLACK;
                    }

                    //MENU GRAPHIC
                    Raylib.BeginDrawing();
                    Raylib.ClearBackground(Color.BLACK);
                    Raylib.DrawText("SPACE INVADERS", 200, 100, 48, Color.WHITE);
                    Raylib.DrawText("START", 200, 200, 32, Color.WHITE);
                    Raylib.DrawText("*", 160, 200, 32, startColor);
                    Raylib.DrawText("OPTIONS", 200, 300, 32, Color.WHITE);
                    Raylib.DrawText("*", 160, 300, 32, optionsColor);
                    Raylib.DrawText("EXIT", 200, 400, 32, Color.WHITE);
                    Raylib.DrawText("*", 160, 400, 32, exitColor);
                    Raylib.EndDrawing();
                }

                while(gameState == "game")
                {
                    //GET ENEMY CLASS
                    Enemy enemy = new Enemy(); 
                    Rectangle enemyRec = new Rectangle(200, 200, 40, 40); //TRIAL

                    //MAKE PLAYER REC
                    Rectangle playerRec = new Rectangle(playerX, playerY, 40, 40);

                    //MAKE BULLET REC
                    Rectangle bulletRec = new Rectangle(bulletX, bulletY, 5, 25);
                    

                    
                    //MOVEMENT METOD
                    (float pX, float pSpeed) returnedMovement = PlayerMovement(playerX, playerSpeed);
                    playerX = returnedMovement.pX;
                    playerSpeed = returnedMovement.pSpeed;

                    //GRAFIK
                    Raylib.BeginDrawing();
                    Raylib.ClearBackground(Color.BLACK);
                    
                    Raylib.DrawRectangleRec(playerRec, Color.RED);
                    
                    //enemy.DrawEnemies(); 

                    //TRIAL TRIAL
                    Raylib.DrawRectangleRec(enemyRec, Color.YELLOW);

                    Raylib.EndDrawing();

                    if (Raylib.IsKeyPressed(KeyboardKey.KEY_W) && readyToShoot == true)
                    {
                        readyToShoot = false;
                        bulletX = playerX + 15;
                    }

                    if (readyToShoot == false && bulletY > -500)
                    {
                        Raylib.DrawRectangleRec(bulletRec, Color.RED);
                        bulletY -= 15;
                    }
                    else
                    {
                        readyToShoot = true;
                        bulletY = 700;
                    }

                    if (Raylib.CheckCollisionRecs(enemyRec, bulletRec))
                    {
                        Raylib.DrawText("HIT!", 100, 100, 32, Color.WHITE);
                    }

                    //ENEMY AND PLAYER COLLISION
                    //for (int i = 0; i < allEnemies; i++)
                    //{ if (CheckCollisionRecs(player, enemy[i],) -> gameOver = true)}
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

        static int MenuTarget(int mnTarget)
        {
            if (Raylib.IsKeyPressed(KeyboardKey.KEY_W) || Raylib.IsKeyPressed(KeyboardKey.KEY_UP))
            {
                if (mnTarget == 1)
                {
                    mnTarget = 1;
                }
                else
                {
                    mnTarget--;
                }
            }
            else if (Raylib.IsKeyPressed(KeyboardKey.KEY_S) || Raylib.IsKeyPressed(KeyboardKey.KEY_DOWN))
            {
                if (mnTarget == 3)
                {
                    mnTarget = 3;
                }
                else 
                {
                    mnTarget++;
                }
            }
            return mnTarget;
        }
    }
}

                