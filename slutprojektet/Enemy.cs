using System;
using Raylib_cs;

namespace slutprojektet
{
    public class Enemy
    {
        public int width, height;
        public float x, y;
        public int direction = 1;
        public bool isDead = false;

        public Enemy(float tempX, float tempY)
        {
            width = 40;
            height = 40;
            x = tempX;
            y = tempY;
        }
        public void DrawEnemy()
        {
           if (isDead == false)
           {
               Raylib.DrawRectangle((int)x, (int)y, width, height, Color.RED);
           }
        }
        public void UpdateEnemy()
        {
            x += 0.5f * direction;
        }
    }
}
