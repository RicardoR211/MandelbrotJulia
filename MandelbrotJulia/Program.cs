using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace MandelbrotJulia
{
    internal class Program
    {
        public static void Main(String[] args)
        {
            Raylib.InitWindow(800, 480, "MandelbrotJulia");
            Raylib.SetTargetFPS(60);

            //Carregando shader
            Shader shader = Raylib.LoadShader(null, "mandelbrot.frag");

            //Propriedades de resolução
            int resolutionLoc = Raylib.GetShaderLocation(shader, "resolution");
            float[] resolution = { 800f, 480f };
            Raylib.SetShaderValue(shader, resolutionLoc, resolution, ShaderUniformDataType.Vec2);

            //Iterar com base no zoom
            int maxIterLoc = Raylib.GetShaderLocation(shader, "maxIter");

            //Propriedades do zoom
            float zoom = 1.0f;
            Vector2 offset = new Vector2(0.0f, 0.0f);

            int zoomLoc = Raylib.GetShaderLocation(shader, "zoom");
            int offsetLoc = Raylib.GetShaderLocation(shader, "offset");

            float zoomSpeed = 1.5f;

            //Definindo modos
            int modeLoc = Raylib.GetShaderLocation(shader, "mode");
            int juliaCLoc = Raylib.GetShaderLocation(shader, "juliaC");
            int mode = 0;
            Vector2 juliaC = new Vector2(-0.7f, 0.27f);

            //Loop principal do jogo
            while (!Raylib.WindowShouldClose())
            {

                //Pegando o giro do mouse
                float scroll = Raylib.GetMouseWheelMove();
                if (scroll != 0) zoom *= (scroll > 0 ? 0.9f / zoomSpeed : 1.1f * zoomSpeed);

                Raylib.SetShaderValue(shader, zoomLoc, zoom, ShaderUniformDataType.Float);
                float[] off = { offset.X, offset.Y };
                Raylib.SetShaderValue(shader, offsetLoc, off, ShaderUniformDataType.Vec2);

                //Setando o maximo de iteração
                int maxIter = (int)(100 / zoom);
                maxIter = Math.Clamp(maxIter, 100, 1000);
                Raylib.SetShaderValue(shader, maxIterLoc, maxIter, ShaderUniformDataType.Int);

                //Dando um boost de velocidade
                if (Raylib.IsKeyDown(KeyboardKey.LeftShift))
                {
                    zoomSpeed = 1.5f;
                }
                else zoomSpeed = 1.0f;

                //Mover a câmera
                if (Raylib.IsMouseButtonDown(MouseButton.Left))
                {
                    Vector2 delta = Raylib.GetMouseDelta();
                    offset.X -= delta.X * zoom * 0.003f;
                    offset.Y += delta.Y * zoom * 0.003f;
                }

                if (Raylib.IsMouseButtonPressed(MouseButton.Right))
                {
                    Vector2 mouse = Raylib.GetMousePosition();
                    Vector2 uv = new Vector2(mouse.X / 800f, mouse.Y / 480f);
                    juliaC = new Vector2(
                        (uv.X - 0.5f) * 3.5f * zoom + offset.X,
                        -(uv.Y - 0.5f) * 2.0f * zoom + offset.Y
                    );
                }

                //Trocando modo
                if (Raylib.IsKeyPressed(KeyboardKey.Tab))
                {
                    Console.WriteLine("Ai");
                    if (mode == 0) mode = 1;
                    else mode = 0;
                }
                float[] jc = { juliaC.X, juliaC.Y };
                Raylib.SetShaderValue(shader, juliaCLoc, jc, ShaderUniformDataType.Vec2);
                Raylib.SetShaderValue(shader, modeLoc, mode, ShaderUniformDataType.Int);

                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.Black);

                Raylib.BeginShaderMode(shader);
                  

                //Desenhando o retângulo de teste
                Raylib.DrawRectangle(0, 0, 800, 480, Color.White);

                

                Raylib.EndShaderMode();
                Raylib.DrawText($"FPS: {Raylib.GetFPS()}", 10, 10, 20, Color.Green);

                Raylib.EndDrawing();
            }

            Raylib.CloseWindow();

        }
    }
}
