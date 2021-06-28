using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SFML.GameAssets
{
    static class GameStatus
    {
        // Fps counter  -------------------
        static public Stopwatch fpsStatusInterval { get; set; } = new Stopwatch();

        private static DateTime lastCheckTime = DateTime.Now;
        private static long frameCount = 0;
        private static double fps;
        private static readonly Text fpsStatus = new Text {
            
            Font = new SFML.Graphics.Font("C:/Windows/Fonts/arial.ttf"),
            Position = new Vector2f(3,3),
            CharacterSize = 10,
            FillColor = SFML.Graphics.Color.Yellow
        };


        public static void UpdateFps()
        {                    
            double secondsElapsed = (DateTime.Now - lastCheckTime).TotalSeconds;
            long count = Interlocked.Exchange(ref frameCount, 0);

            fps = count / secondsElapsed;                
            lastCheckTime = DateTime.Now;

            Interlocked.Increment(ref frameCount);

            if (fpsStatusInterval.Elapsed.TotalMilliseconds > 100)
            {
                fpsStatus.DisplayedString = $"{Math.Round(fps, 0)}";
                fpsStatusInterval.Restart();
            }
        }  
        public static void ShowFps() => GameProperties.Window.Draw(fpsStatus);
        public static double GetFps() => fps;
        // --------------------------------


        // Application Idle ---------------

        [StructLayout(LayoutKind.Sequential)]
        public struct NativeMessage
        {
            public IntPtr Handle;
            public uint Message;
            public IntPtr WParameter;
            public IntPtr LParameter;
            public uint Time;
            public Point Location;
        }

        [DllImport("user32.dll")]
        internal static extern int PeekMessage(NativeMessage message, IntPtr window, uint filterMin, uint filterMax, uint remove);
        public static bool IsApplicationIdle() => PeekMessage(new NativeMessage(), IntPtr.Zero, (uint)0, (uint)0, (uint)0) == 0;
        // --------------------------------
    }

}