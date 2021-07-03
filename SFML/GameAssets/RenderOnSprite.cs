using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SFML.GameAssets.GameFuctions;


namespace SFML.GameAssets
{
    // Rework of
    // https://www.gamedev.net/forums/topic/693835-sfml-and-fast-pixel-drawing-in-c/

    static public class RenderOnSprite
    {
        public static bool SmoothImage { get; set; } = false;

        static Color[] buffer = new Color[GameProperties.WindowWidth * GameProperties.WindowHeight];
        static Color[] wipeBuffer = Enumerable.Repeat(
            GameProperties.BackGroundColor,
            GameProperties.WindowWidth * GameProperties.WindowHeight)
            .ToArray();

        static Texture texture = new Texture((uint)GameProperties.WindowWidth, (uint)GameProperties.WindowHeight);
        static Sprite viewport = new Sprite(texture);

        // (WindowWidth x WindowHeight) x 4 bytes per pixel
        static byte[] pixels = new byte[(GameProperties.WindowWidth * GameProperties.WindowHeight) * 4];

        static public void Draw(RenderWindow window)
        {
            //DrawStaticDemo();
            BufferIntoImage();

            // update the texture with the array
            texture.Update(pixels);
            texture.Smooth = SmoothImage;

            // draw the texture over the screen  
            window.Draw(viewport);

            // wipe
            wipeBuffer.CopyTo(buffer, 0);
        }
        static void BufferIntoImage()
        {
            //fill the byte array
            Parallel.For(0, GameProperties.WindowWidth * GameProperties.WindowHeight, i => {

                int index = i * 4;
                pixels[index + 0] = buffer[index / 4].R;
                pixels[index + 1] = buffer[index / 4].G;
                pixels[index + 2] = buffer[index / 4].B;
                pixels[index + 3] = buffer[index / 4].A;

            });
        }
        static void DrawStaticDemo()
        {
            for (int x = 0; x < GameProperties.WindowWidth; x++)
                for (int y = 0; y < GameProperties.WindowHeight; y++)
                {
                    buffer[Fuctions.IX(x, y)] = new Color((byte)(y + x), (byte)(y), (byte)(y - x));
                }
        }
        static void mousdemo()
        {
            Fuctions.GetMousePositionBeta(out int x, out int y);
            buffer[Fuctions.IX(x, y)] = new Color((byte)(y + x), (byte)(y), (byte)(y - x));
        }

        static public void DrawSinglePixel(Pixel pixel)
        {
            buffer[Fuctions.IX(pixel.X, pixel.Y)] = pixel.Color;
        }

        static public void DrawPixelBatch(List<Pixel> batch)
        {


        } 
    }
    public struct Pixel
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Color Color { get; set; }
    }
}
