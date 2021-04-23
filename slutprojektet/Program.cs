﻿using System;
using Raylib_cs;
using System.Numerics;
using System.Timers;
using System.Collections.Generic;

namespace slutprojektet
{
    class Program   //TODO: ENEMY SHOOTING, PLAYER COLLISION, GAMEOVER, NEXT LEVEL, RANDOM ENEMY SKINS (CASE SWITCH?), UNLOCKABLE PLAYER SKINS
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

            //ENEMY VALUES
            List<Enemy> enemies = new List<Enemy>();
            //ADD ENEMIES FOR EACH ROW AND FOREACH COLUMN
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    enemies.Add(new Enemy(j*80+20, i*80+40));
                }
            }
            //PLAY VALUES
            float playerX = 492;
            float playerY = 708;
            float playerSpeed = 5; 

            //SHOOT VALUES
            bool readyToShoot = true;
            float bulletX = 0;
            float bulletY = 700;

            //ENEMY SHOOTING
            int enemyBulletX = 0;
            int enemyBulletY = 0;
            bool enemyShooting = false;


            //SCREEN VALUES
            Raylib.InitWindow(screenWidth, screenHeight, "Space Invaders");
            Raylib.SetTargetFPS(60);               

            while (!Raylib.WindowShouldClose())
            {
                while (gameState == "start")
                {
                    //GET MENUTARGET METHOD
                    menuTarget = MenuTarget(menuTarget);

                    //MENU LOGIC (SÄTT IN I EN METOD SENARE)
                    if (menuTarget == 1)
                    {
                        startColor = Color.WHITE;
                        if (EnterOrSpaceCheck() == true)
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
                        if (EnterOrSpaceCheck() == true)
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
                        if (EnterOrSpaceCheck() == true)
                        {
                            break;        
                        }
                    }
                    else
                    {
                        exitColor = Color.BLACK;
                    }

                    //MENU GRAPHIC //SÄTT IN I EN LOOP SENARE
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
                    //MAKE PLAYER REC
                    Rectangle playerRec = new Rectangle(playerX, playerY, 40, 40);
                    
                    //MAKE BULLET REC
                    Rectangle bulletRec = new Rectangle(bulletX, bulletY, 5, 25);    
                    //MOVEMENT METOD
                    (float pX, float pSpeed) returnedMovement = PlayerMovement(playerX, playerSpeed);
                    playerX = returnedMovement.pX;
                    playerSpeed = returnedMovement.pSpeed;

                    for (int i = 0; i < enemies.Count; i++)
                    {
                        enemies[i].DrawEnemy();
                    }
                    //MOVE ALL ENEMIES 
                    for (int i = 0; i < enemies.Count; i++)
                    {
                        enemies[i].UpdateEnemy(); 
                    }  

                    //BULLET LOGIC (LÄGG I EN METOD SENARE)
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

                    //COLLISION BETWEEN BULLET AND ENEMY
                    for (int i = 0; i < enemies.Count; i++)
                    {
                        Rectangle enemyRec = new Rectangle(enemies[i].x, enemies[i].y, enemies[i].width, enemies[i].height);
                        if (Raylib.CheckCollisionRecs(bulletRec, enemyRec) && enemies[i].isDead == false)
                        {
                            enemies[i].isDead = true;
                            bulletY = -40;
                        }
                    }

                    //GET MAX AND MIN X FROM LIVING ENEMIES
                    float maxX = 0;
                    for (int i = 0; i < enemies.Count; i++)
                    {
                        if (enemies[i].x > maxX && enemies[i].isDead == false)
                        {
                            maxX = enemies[i].x;
                        }
                    }
                    float minX = 1024;
                    for (int i = 0; i < enemies.Count; i++)
                    {
                        if (enemies[i].x < minX && enemies[i].isDead == false)
                        {
                            minX = enemies[i].x;
                        }
                        
                    }
                    //MOVE ENEMY BLOCK DOWN AND CHANGE DIR
                    if (maxX > 974 || minX < 10)
                    {
                        for (int i = 0; i < enemies.Count; i++)
                        {
                            enemies[i].y += 80;
                            enemies[i].direction = -enemies[i].direction;
                        }
                    }

                    //ENEMY RANDOM SHOOTING
                    int count = enemies.Count;
                    Random generator = new Random();

                    int enemy = generator.Next(count);

                    //THIS ONE RIGHT HERE CRASHES THE ENTIRE PROGRAM IF YOU WIN MAYBE YOU SHOULD WORK ON FIXING THAT BEFORE TURNING THIS STUFF IN
                    while (enemies[enemy].isDead == true)
                    {
                        enemy = generator.Next(count);
                    }
                
                    if (enemyShooting == false)
                    {
                        enemyBulletX = (int)enemies[enemy].x + 20;
                        enemyBulletY = (int)enemies[enemy].y + 40;
                        enemyShooting = true;
                    }

                    if (enemyShooting == true)
                    {
                        enemyBulletY += 5;
                    }

                    if (enemyBulletY > 1800)
                    {
                        enemyShooting = false;
                    }
                    

        

                    //GRAFIK
                    Raylib.BeginDrawing();
                    Raylib.ClearBackground(Color.BLACK);
                    //DRAW PLAYER
                    Raylib.DrawRectangleRec(playerRec, Color.RED);
                    //DRAW ENEMY BULLET
                    Raylib.DrawRectangle(enemyBulletX, enemyBulletY, 5, 20, Color.GREEN);
                    Raylib.EndDrawing();

                }
            }
        }
        static (float, float) PlayerMovement(float pX, float pSpeed)
        {
            //KEY MOVEMENT && COLLISION CHECK AGAINST WINDOW
            if (Raylib.IsKeyDown(KeyboardKey.KEY_A) && pX > 2)
            {
                pX -= pSpeed;
            }
            if (Raylib.IsKeyDown(KeyboardKey.KEY_D) && pX < 982)
            {
                pX += pSpeed;
            }
            return (pX, pSpeed);
        }

        static int MenuTarget(int mnTarget)
        {
            //MENU MOVEMENT LOGIC
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
        
        static bool EnterOrSpaceCheck()
        {
            if (Raylib.IsKeyPressed(KeyboardKey.KEY_ENTER) || Raylib.IsKeyPressed(KeyboardKey.KEY_SPACE))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

                