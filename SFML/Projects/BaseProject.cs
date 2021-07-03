using SFML.GameAssets;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace SFML.Projects
{
    public class BaseProject
    {
        public virtual void GameLoop(RenderWindow window) { }
        public override string ToString()
        {
            var naming = base.ToString().Split(".");
            return $"Project: {naming[naming.Length - 1]}";
        }
    }
}
