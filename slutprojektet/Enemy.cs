using System;
using Raylib_cs;

namespace slutprojektet
{
    public class Enemy
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
}
