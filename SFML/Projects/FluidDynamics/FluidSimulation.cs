using static SFML.GameAssets.GameFuctions;
using SFML.GameAssets;
using SFML.System;
using SFML.Window;
using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFML.Projects.FluidDynamics
{
    class FluidSimulation : BaseProject
    {
        // https://mikeash.com/pyblog/fluid-simulation-for-dummies.html

        Fluid fluid = new Fluid(GameProperties.WindowWidth, 
            0.2f, 
            0.00000001f, // How fast the smoke disepares diffusion 
            0.000000015f);

        Random rand = new Random();


        int oldX = 1, oldY = 1;
        int diferentsInX;
        int diferentsInY;
 
        public override void GameLoop(RenderWindow window)
        {
            Demo();

            fluid.Step();
            fluid.Render();  

            RenderOnSprite.Draw(window);             
        }
        void Demo() {

            MouseDispursion(0.5f);
            BurningCandel(rand.Next(10, 255), 10);
        }
        void BurningCandel(float densitty, int maxFlimeHight)
        {

            fluid.AddDensity(fluid.N / 2, fluid.N - 2, densitty);
            fluid.AddVelocity(fluid.N / 2, fluid.N - 2, 0, rand.Next(-maxFlimeHight, 0));

            fluid.AddDensity((fluid.N / 2) + 1, (fluid.N - 2), densitty);
            fluid.AddVelocity((fluid.N / 2) + 1, (fluid.N - 2), 0, rand.Next(-maxFlimeHight, 0));
        }

        void MouseDispursion(float divideIntensetyBy)
        {
            Fuctions.GetMousePosition(out int mX, out int mY);

            diferentsInX = mX - oldX;
            diferentsInY = mY - oldY;

            oldX = mX;
            oldY = mY;

            fluid.AddVelocity(oldX, oldY, diferentsInX / divideIntensetyBy, diferentsInY/ divideIntensetyBy);
            fluid.AddVelocity(oldX + 1, oldY, diferentsInX/ divideIntensetyBy, diferentsInY/ divideIntensetyBy);
            fluid.AddVelocity(oldX - 1, oldY, diferentsInX/ divideIntensetyBy, diferentsInY/ divideIntensetyBy);
            fluid.AddVelocity(oldX, oldY + 1, diferentsInX/ divideIntensetyBy, diferentsInY/ divideIntensetyBy);
            fluid.AddVelocity(oldX, oldY - 1, diferentsInX/ divideIntensetyBy, diferentsInY/ divideIntensetyBy);
        }       
    }
}
