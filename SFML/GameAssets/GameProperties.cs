using SFML.Graphics;
using SFML.Projects;
using SFML.Projects.FluidDynamics;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Text;

namespace SFML.GameAssets
{
    public class GameProperties
    {
        static public int WindowHeight { get; set; } = 1080;
        static public int WindowWidth { get; set; } = 1920;

        // bitsPerPixel represents the bit depth, also know as the color depth
        // Usually you would use a value of 32 here to have good rendering
        static public uint WindowBitsPerPixel { get; set; } = 32;
        static public string Titel { get; set; } = "GameEngine: SFMLOpenGl";
        static public bool EnableVsync { get; set; } = true;
        static public RenderWindow Window { get; set; }
        static public Styles WindowStyle { get; set; } = Styles.Default;
        static public BaseProject Project { get; set; } = new FluidSimulation();
        static public Color BackGroundColor { get; set; } = Color.Black; //new Color(238, 238, 238);
    }
}
