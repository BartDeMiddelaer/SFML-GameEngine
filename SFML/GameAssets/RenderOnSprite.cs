using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFML.GameAssets
{
    // Rework of
    // https://www.gamedev.net/forums/topic/693835-sfml-and-fast-pixel-drawing-in-c/

    static public class RenderOnSprite
    {
        public static bool SmoothImage { get; set; } = false; 

        static Color[] ScreenBuffer = new Color[GameProperties.WindowWidth * GameProperties.WindowHeight];
        static Color[] ScreenWhiper = Enumerable.Repeat(
            GameProperties.BackGroundColor, 
            GameProperties.WindowWidth * GameProperties.WindowHeight)
            .ToArray();

        static Texture mainTexture = new Texture((uint)GameProperties.WindowWidth, (uint)GameProperties.WindowHeight);
        static Sprite mainViewport = new Sprite(mainTexture);

        //GameProperties.WindowWidth x GameProperties.WindowHeight pixels x 4 bytes per pixel

        static byte[] pixels = new byte[GameProperties.WindowWidth * GameProperties.WindowHeight * 4];

        static public void Draw(RenderWindow window)
        {
            // DO STUFF
            //DrawStaticDemo();
            BufferIntoImage();

            mainTexture.Update(pixels);//update the texture with the array
            mainTexture.Smooth = SmoothImage;
            window.Draw(mainViewport);//draw the texture over the screen  

            ResetBuffer();
            
        }                
        static void BufferIntoImage()
        {    
            //fill the byte array]
            Parallel.For(0, GameProperties.WindowWidth * GameProperties.WindowHeight, i => {

                int index = i * 4;
                pixels[index + 0] = ScreenBuffer[index / 4].R;
                pixels[index + 1] = ScreenBuffer[index / 4].G;
                pixels[index + 2] = ScreenBuffer[index / 4].B;
                pixels[index + 3] = ScreenBuffer[index / 4].A;

            });       
        }
        static void DrawStaticDemo()
        {
            for (int x = 0; x < GameProperties.WindowWidth; x++)
                for (int y = 0; y < GameProperties.WindowHeight; y++)
                {                 
                    ScreenBuffer[IX(x, y)] = new Color((byte)(y + x), (byte)(y), (byte)(y - x));
                }
        }

        static public void DrawSinglePixel(Pixel pixel)
        {        
            ScreenBuffer[IX(pixel.X, pixel.Y)] = pixel.Color;
        }

        static public void DrawPixelBatch(List<Pixel> batch)
        { 
        

        }

        static void ResetBuffer() =>              
            ScreenBuffer = ScreenWhiper;

        static public int IX(int x, int y) =>
            x + (y * GameProperties.WindowWidth);
        
    }
    public struct Pixel {

        public int X { get; set; }
        public int Y { get; set; }
        public Color Color { get; set; }
    }

}
