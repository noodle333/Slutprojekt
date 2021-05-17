using System;
using Raylib_cs;
using System.Numerics;

namespace slutprojektet
{
    public class Enemy
    {
        public int width, height;
        public float x, y;
        public int direction = 2;
        public bool isDead = false;

        private static Texture2D spaceInvaderOne = Raylib.LoadTexture("invader.png");
        private static Texture2D spaceInvaderTwo = Raylib.LoadTexture("invader2.png");
        private static Texture2D spaceInvaderThree = Raylib.LoadTexture("invader3.png");
        private int caseSwitch;

        public Enemy(float tempX, float tempY)
        {
            width = 40;
            height = 40;
            x = tempX;
            y = tempY;

            if (y < 100)
            {
                caseSwitch = 1;
            }
            else if (y < 200)
            {
                caseSwitch = 2;                
            } 
            else 
            {
                caseSwitch = 3;
            }
        }
       

        public void DrawEnemy()
        {
            if (isDead == false)
            {   
                switch (caseSwitch)
                {
                    case 1:
                        Raylib.DrawTextureEx(spaceInvaderOne, new Vector2((int)x-9, (int)y-8), 0f, 0.11f, Color.WHITE);
                        break;
                    case 2:
                        Raylib.DrawTextureEx(spaceInvaderTwo, new Vector2((int)x-9, (int)y-8), 0f, 0.11f, Color.WHITE);
                        break;
                    case 3:
                        Raylib.DrawTextureEx(spaceInvaderThree, new Vector2((int)x-9, (int)y-8), 0f, 0.11f, Color.WHITE);
                        break;
                }
            }
        }
        public void UpdateEnemy()
        {
            x += 0.3f * direction;
        }
    }
}
