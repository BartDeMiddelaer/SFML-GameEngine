using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFML.GameAssets
{
    public sealed class GameFuctions
    {
        public static GameFuctions Fuctions
        {
            get {

                lock (padlock)  
                    if (instance == null) instance = new GameFuctions();
                    return instance;               
            }
        }
        private static GameFuctions instance = null;
        private static readonly object padlock = new object();
       
        int mousposX = 1;     
        int mousposY = 1;

        internal void MousePosition(object sender, MouseMoveEventArgs e)
        {
            mousposX = e.X;
            mousposY = e.Y;             
        }
        public void GetMousePosition(out int mX, out int mY)
        {
            var wSize = GameProperties.Window.Size;

            double xProcent = (mousposX * 100) / wSize.X;
            double xPixel = Math.Round((GameProperties.WindowWidth * xProcent) / 100, 0);

            double yProcent = (mousposY * 100) / wSize.Y;
            double yPixel = Math.Round((GameProperties.WindowWidth * yProcent) / 100, 0);

            mY = (int)yPixel;
            mX = (int)xPixel;
        }
        public void GetMousePositionBeta(out int mX, out int mY)
        {
            mY = mousposY;
            mX = mousposX;
        }

    }
}



