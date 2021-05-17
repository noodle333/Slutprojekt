using System;
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

            //PLAYER VALUES
            float playerX = 492;
            float playerY = 708;
            float playerSpeed = 5; 

            //SHOOTING VALUES
            bool readyToShoot = true;
            float bulletX = 0;
            float bulletY = 700;
            Rectangle bulletRec = new Rectangle(bulletX, bulletY, 5, 25); 

            //ENEMY VALUES
            List<Enemy> enemies = new List<Enemy>();
            //CREATE ALL ENEMIES METHOD
            CreateEnemies(enemies);

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

                    //CHANGE GAMESTATE DEPENDING ON PLAYER INPUT METHOD
                    (int mnTarget, string gState) returnedMenu = MenuTargetLogic(menuTarget, gameState);
                    gameState = returnedMenu.gState;

                    //CHANGE TEXT COLOR DEPENDING ON MENUTARGET METHOD
                    (int mnTarget, Color start, Color opt, Color exit) returnedColor = MenuColor(menuTarget, startColor, optionsColor, exitColor);
                    startColor = returnedColor.start;
                    optionsColor = returnedColor.opt;
                    exitColor = returnedColor.exit;

                    //MENU GRAPHIC 
                    Raylib.BeginDrawing();
                    Raylib.ClearBackground(Color.BLACK);
                    Raylib.DrawText("SPACE INVADERS", 200, 100, 48, Color.WHITE);
                    Raylib.DrawText("START", 200, 200, 32, Color.WHITE);
                    Raylib.DrawText("OPTIONS", 200, 300, 32, Color.WHITE);
                    Raylib.DrawText("EXIT", 200, 400, 32, Color.WHITE);
                    Raylib.DrawText("*", 160, 200, 32, startColor);
                    Raylib.DrawText("*", 160, 300, 32, optionsColor);
                    Raylib.DrawText("*", 160, 400, 32, exitColor);
                    Raylib.EndDrawing();
                }

                while(gameState == "game")
                {
                    //MAKE PLAYER REC
                    Rectangle playerRec = new Rectangle(playerX, playerY, 40, 40);

                    //MOVEMENT METOD
                    (float pX, float pSpeed) returnedMovement = PlayerMovement(playerX, playerSpeed);
                    playerX = returnedMovement.pX;
                    playerSpeed = returnedMovement.pSpeed;

                    //PLAYER SHOOTING METHOD
                    (bool rdyShoot, float pX, Rectangle bltRec) returnedBullet = PlayerShooting(readyToShoot, playerX, bulletRec);
                    readyToShoot = returnedBullet.rdyShoot;
                    bulletRec = returnedBullet.bltRec;
                    
                    //DRAW ALL ENEMIES METHOD
                    for (int i = 0; i < enemies.Count; i++)
                    {
                        enemies[i].DrawEnemy();
                    }

                    //MOVE ALL ENEMIES METHOD
                    for (int i = 0; i < enemies.Count; i++)
                    {
                        enemies[i].UpdateEnemy(); 
                    }  

                    //BULLETxENEMY COLLISION METHOD
                    (List<Enemy> enemyList, Rectangle bltRec) returnedCollision = BulletCollision(enemies, bulletRec);
                    enemies = returnedCollision.enemyList;
                    bulletRec = returnedCollision.bltRec;

                    //RESETS MIN MAX EACH FRAME
                    float maxEnemyX = 0;
                    float minEnemyX = 1024;

                    //GET MAX AND MIN X FROM LIVING ENEMIES METHOD
                    (List<Enemy> enemyList, float min, float max) returnedEnemyMinMax = EnemyMinMaxPosition(enemies, minEnemyX, maxEnemyX);
                    minEnemyX = returnedEnemyMinMax.min;
                    maxEnemyX = returnedEnemyMinMax.max;
                    
                    //MOVE ENEMY BLOCK DOWN AND CHANGE DIR METHOD
                    (List<Enemy> enemyList, float min, float max) returnedEnemyVertMove = EnemyVerticalMove(enemies, minEnemyX, maxEnemyX);
                    enemies = returnedEnemyVertMove.enemyList;

                    //ENEMY RANDOM SHOOTING
                    Random generator = new Random();
                    int enemy = generator.Next(enemies.Count);

                    //RESET COUNT EVERY FRAME
                    int count = 0;

                    //COUNT DEAD ENEMIES METHOD
                    (List<Enemy> enemyList, int c) returnedDeadEnemies = DeadEnemyCount(enemies, count);
                    count = returnedDeadEnemies.c;

                    //RE-RANDOMIZE IF ENEMY IS DEAD 
                    while (enemies[enemy].isDead == true && count < enemies.Count - 1)
                    {
                        enemy = generator.Next(enemies.Count);
                    }
                
                    //ENEMY SHOOTING METHOD
                    (List<Enemy> enemyList, bool eShoot, int c, int x, int y, int e) returnedEnemyShooting = EnemyShooting(enemies, enemyShooting, count, enemyBulletX, enemyBulletY, enemy);
                    enemies = returnedEnemyShooting.enemyList;
                    enemyShooting = returnedEnemyShooting.eShoot;
                    count = returnedEnemyShooting.c;
                    enemyBulletX = returnedEnemyShooting.x;
                    enemyBulletY = returnedEnemyShooting.y;
                    enemy = returnedEnemyShooting.e;
                    
                    //WHEN ONLY ONE ENEMY LEFT INCReASE ITS SPEED
                    (List<Enemy> enemyList, int c) returnedLastEnemy = LastEnemySpeedBoost(enemies, count);
                    enemies = returnedLastEnemy.enemyList;
                    count = returnedLastEnemy.c;
                    
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

        static void CreateEnemies(List<Enemy> list)
        {
            //ADD ENEMIES FOR EACH ROW AND FOREACH COLUMN
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    list.Add(new Enemy(j*80+20, i*80+40));
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

        static (int, string) MenuTargetLogic(int mnTarget, string gState)
        {
            if (mnTarget == 1)
            {
                if (EnterOrSpaceCheck() == true)
                {
                    gState = "game";        
                }
            }
            if (mnTarget == 2)
            {
                if (EnterOrSpaceCheck() == true)
                {
                    gState = "options";        
                }
            }
            if (mnTarget == 3)
            {
               if (EnterOrSpaceCheck() == true)
                {
                    Raylib.CloseWindow();        
                } 
            }
            return (mnTarget, gState);
        }

        static (int, Color, Color, Color) MenuColor(int mnTarget, Color start, Color opt, Color exit)
        {
            if (mnTarget == 1)
            {
                start = Color.WHITE;
            }
            else
            {
                start = Color.BLACK;
            }
            if (mnTarget == 2)
            {
                opt = Color.WHITE;
            }
            else
            {
                opt = Color.BLACK;
            }
            if (mnTarget == 3)
            {
                exit = Color.WHITE;
            }
            else
            {
                exit = Color.BLACK;
            }
            return (mnTarget, start, opt, exit);
        }

        static (bool, float, Rectangle) PlayerShooting(bool rdyShoot, float pX, Rectangle bltRec)
        {
            //IF SHOOTING IS READY, SET BULLET X POSITION AND TURN OFF ABILITY TO SHOOT AGAIN
            if (Raylib.IsKeyPressed(KeyboardKey.KEY_W) && rdyShoot == true)
            {
                bltRec.x = pX + 15;
                rdyShoot = false;
            }
            //IF SHOOTING IN PROGRESS DRAW BULLET AND MOVE UPWARDS 
            else if (rdyShoot == false && bltRec.y > -500)
            {
                Raylib.DrawRectangleRec(bltRec, Color.RED);
                bltRec.y -= 15;
            }
            //IF BULLET OUT OF BOUNDS, RESET BULLET AND GIVE PLAYER ABILITY TO SHOOT
            else
            {
                rdyShoot = true;
                bltRec.y = 700;
            }
            return (rdyShoot, pX, bltRec);
        }

        static (List<Enemy>, Rectangle) BulletCollision(List<Enemy> enemyList, Rectangle bltRec)
        {
            //COLLISION BETWEEN BULLET AND ENEMY
            for (int i = 0; i < enemyList.Count; i++)
            {
                Rectangle enemyRec = new Rectangle(enemyList[i].x, enemyList[i].y, enemyList[i].width, enemyList[i].height);
                if (Raylib.CheckCollisionRecs(bltRec, enemyRec) && enemyList[i].isDead == false)
                {
                    enemyList[i].isDead = true;
                    bltRec.y = -40;
                }
            }
            return (enemyList, bltRec);
        }
        
        static (List<Enemy>, float, float) EnemyMinMaxPosition(List<Enemy> enemyList, float min, float max)
        {
            for (int i = 0; i < enemyList.Count; i++)
            {
                if (enemyList[i].x > max && enemyList[i].isDead == false)
                {
                    max = enemyList[i].x;
                }
                if (enemyList[i].x < min && enemyList[i].isDead == false)
                {
                    min = enemyList[i].x;
                }
            }
            return (enemyList, min, max);
        }

        static (List<Enemy>, float, float) EnemyVerticalMove(List<Enemy> enemyList, float min, float max)
        {
            if (max > 974 || min < 10)
            {
                for (int i = 0; i < enemyList.Count; i++)
                {
                    enemyList[i].y += 80;
                    enemyList[i].direction = -enemyList[i].direction;
                }
            }
            return (enemyList, min, max);
        }

        static (List<Enemy>, int) DeadEnemyCount(List<Enemy> enemyList, int c)
        {
            for (int i = 0; i < enemyList.Count; i++)
            {
                if (enemyList[i].isDead == true)
                {
                    c++;
                }
            }
            return (enemyList, c);
        }

        static (List<Enemy>, bool, int, int, int, int) EnemyShooting(List<Enemy> enemyList, bool eShoot, int c, int x, int y, int e)
        {
            //CHECK SO ENEMIES ARE NOT CURRENTLY SHOOTING
            if (eShoot == false && c < enemyList.Count - 1)
            {
                x = (int)enemyList[e].x + 20;
                y = (int)enemyList[e].y + 40;
                eShoot = true;
            }
            //MOVE BULLET
            if (eShoot == true)
            {
                y += 10;
            }
            //CHECK WHEN BULLET OUT OF BOUNDS AND RESET
            if (y > 1800)
            {
                eShoot = false;
            }
            return (enemyList, eShoot, c, x, y, e);
        }

        static (List<Enemy>, int) LastEnemySpeedBoost(List<Enemy> enemyList, int c)
        {
            if (c == enemyList.Count - 1)
            {
                //FIND THE LIVING ENEMY
                for (int i = 0; i < enemyList.Count; i++)
                {
                    if (enemyList[i].isDead == false)
                    {
                        //INCReASE ITS SPEED IN APPROPRIATE DIRECTIon
                        enemyList[i].direction += 2;
                    }
                }
            }
            return(enemyList, c);
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

                