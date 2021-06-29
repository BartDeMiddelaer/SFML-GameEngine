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

        static Random rand = new Random();
        static Color[,] ScreenBuffer = new Color[GameProperties.WindowWidth, GameProperties.WindowHeight];
        static Texture mainTexture = new Texture((uint)GameProperties.WindowWidth, (uint)GameProperties.WindowHeight);
        static Sprite mainViewport = new Sprite(mainTexture);

        //GameProperties.WindowWidth x GameProperties.WindowHeight pixels x 4 bytes per pixel
        static byte[] pixels = new byte[GameProperties.WindowWidth * GameProperties.WindowHeight * 4];

        //intermediary step to keep everything clear
        static Color[] colorsForpixels = new Color[GameProperties.WindowWidth * GameProperties.WindowHeight];

        // keeps track of alle the pixils in use
        static List<Vector2f> PixelDrawnTo = new List<Vector2f>();

        static public void Draw(RenderWindow window)
        {
            // DO STUFF
            // DrawStaticDemo();
            BufferIntoImage();

            mainTexture.Update(pixels);//update the texture with the array
            mainTexture.Smooth = SmoothImage;
            window.Draw(mainViewport);//draw the texture over the screen  

            ResetBuffer();
            PixelDrawnTo.Clear();
        }                
        static void BufferIntoImage()
        {
            PixelDrawnTo.ForEach(pixel =>
                colorsForpixels[(int)pixel.X + (GameProperties.WindowWidth * (int)pixel.Y)] = ScreenBuffer[(int)pixel.X, (int)pixel.Y]);

            for (int i = 0; i < GameProperties.WindowWidth * GameProperties.WindowHeight * 4; i += 4)//fill the byte array
            {
                pixels[i + 0] = colorsForpixels[i / 4].R;
                pixels[i + 1] = colorsForpixels[i / 4].G;
                pixels[i + 2] = colorsForpixels[i / 4].B;
                pixels[i + 3] = colorsForpixels[i / 4].A;
            }

        }
        static void DrawStaticDemo()
        {
            for (int x = 0; x < GameProperties.WindowWidth; x++)
                for (int y = 0; y < GameProperties.WindowHeight; y++)
                {                 
                    ScreenBuffer[x, y] = new Color((byte)rand.Next(0, 255), (byte)rand.Next(0, 255), (byte)rand.Next(0, 255));
                    PixelDrawnTo.Add(new Vector2f(x, y));
                }
        }
        static public void DrawToPixel(int x, int y, Color col)
        {        
            ScreenBuffer[x, y] = col;
            PixelDrawnTo.Add(new Vector2f(x, y));
        }
        static void ResetBuffer() =>              
            PixelDrawnTo.ForEach(pixel => ScreenBuffer[(int)pixel.X, (int)pixel.Y] = GameProperties.BackGroundColor);
    }

}
