using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Text;

namespace MandelbrotJulia
{
    internal class Program
    {
        public static void Main(String[] args)
        {
            Raylib.InitWindow(800, 480, "MandelbrotJulia");
            Raylib.SetTargetFPS(60);

            //Loop principal do jogo
            while (!Raylib.WindowShouldClose())
            {
                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.Black);

                Raylib.DrawText("Primeiro Commit", 190, 200, 20, Color.LightGray);

                Raylib.EndDrawing();
            }

            Raylib.CloseWindow();

        }
    }
}
