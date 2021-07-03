using static SFML.GameAssets.GameFuctions;
using SFML.GameAssets;
using SFML.Graphics;
using SFML.Projects;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SFML
{
    static class MainWindow
    {
        [STAThread]
        static void Main()
        {
            GameProperties.Window = new RenderWindow( new VideoMode {

                Height = (uint)GameProperties.WindowHeight,
                Width = (uint)GameProperties.WindowWidth,
                BitsPerPixel = GameProperties.WindowBitsPerPixel

            },$"{GameProperties.Titel} <--> {GameProperties.Project}" , GameProperties.WindowStyle);

            GameProperties.Window.SetVerticalSyncEnabled(GameProperties.EnableVsync);
            GameProperties.Window.Closed += ( sender, e) => GameProperties.Window.Close();
            GameStatus.fpsStatusInterval.Start();


            while (GameProperties.Window.IsOpen)
            {
                GameStatus.UpdateFps();              
                GameProperties.Window.Clear(GameProperties.BackGroundColor);
                GameProperties.Window.DispatchEvents();

                //Events ------------------------
                GameProperties.Window.MouseMoved += Fuctions.MousePosition;
                // ------------------------------

                GameProperties.Project.GameLoop(GameProperties.Window);
                GameStatus.ShowFps();
                
                GameProperties.Window.Display();
            }          
        }
       
    } 
}
