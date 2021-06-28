using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SFML.GameAssets
{
    static public class ExtensionMethods
    {
        public static Vector2f GetOffset(this Vector2f value, float length, float angle)
        {
            value.X = (float)(value.X + Math.Cos((Math.PI / 180.0) * angle) * length);
            value.Y = (float)(value.Y + Math.Sin((Math.PI / 180.0) * angle) * length);

            return value;
        }      
        public static float AddValuePerSec(this float value, float increment)
        {
            float speed = increment / (float)GameStatus.GetFps();
            return value = float.IsInfinity(speed) ? 0 : speed;            
        }
    }
}
