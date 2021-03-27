using System;
using Raylib_cs;

namespace slutprojektet
{
    public class Enemy
    {
        public int width, height;
        public int x, y;
        public bool isDead = false;

        public Enemy(int tempX, int tempY)
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
               Raylib.DrawRectangle(x, y, width, height, Color.RED);
           }
        }
    }
}
