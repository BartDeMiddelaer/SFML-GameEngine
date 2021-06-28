using SFML.GameAssets;
using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFML.Projects.FluidDynamics
{
    public class Fluid
    {
        public int Size { get; set; }
        public int N { get; set; }
        public int Iter { get; set; }

        public float Dt { get; set; }
        public float Diffusion { get; set; }
        public float Viscosity { get; set; }

        public float[] S { get; set; }
        public float[] Density { get; set; }

        public float[] Vx { get; set; }
        public float[] Vy { get; set; }

        public float[] Vx0 { get; set; }
        public float[] Vy0 { get; set; }

        public Fluid(int n, float dt, float diffusion, float viscosity) {
       
            N           = n;
            Diffusion   = diffusion;
            Viscosity   = viscosity;
            Dt          = dt;
            S           = new float[n * n];
            Density     = new float[n * n];
            Vx          = new float[n * n];
            Vy          = new float[n * n];
            Vx0         = new float[n * n]; 
            Vy0         = new float[n * n];
            Iter        = 1;

            RenderOnSprite.SmoothImage = true;
        } 
     
        public void AddDensity(int x, int y, float amount)
        {
            if(x < N && x > 0 && y < N && y > 0)
            Density[IX(x,y)] += amount;
        }
        public void AddVelocity(int x, int y, float amountX, float amountY)
        {
            if (x < N && x > 0 && y < N && y > 0)
            {          
                Vx[IX(x, y)] += amountX; 
                Vy[IX(x, y)] += amountY;        
            }
        }
        
        public int IX(int x, int y) {

            return x + (y * N);
        }

        void diffuse(int b, float[] x, float[] x0, float diff, float dt)
        {
            float a = dt * diff * (N - 2) * (N - 2);
            lin_solve(b, x, x0, a, 1 + 6 * a);
        }

        void lin_solve(int b, float[] x, float[] x0, float a, float c)
        {
            var cRecip = 1.0f / c;
    

            // make paralel
            for (int i = 0; i < Iter; i++)
            {

                for (int yD = 1; yD < N - 1; yD++)
                {
                    for (int xD = 1; xD < N - 1; xD++)
                    {
                        x[IX(xD, yD)] =
                          (x0[IX(xD, yD)] +
                            a *
                              (x[IX(xD + 1, yD)] +
                                x[IX(xD - 1, yD)] +
                                x[IX(xD, yD + 1)] +
                                x[IX(xD, yD - 1)])) *
                          cRecip;
                    }
                }
                set_bnd(b, x);
            }
        }

        void project(float[] velocX, float[] velocY, float[] p, float[] div)
        {                  
            for (int yD = 1; yD < N - 1; yD++)
            {
                for (int xD = 1; xD < N - 1; xD++)
                {
                    div[IX(xD, yD)] = -0.5f * (
                                velocX[IX(xD + 1, yD)]
                            - velocX[IX(xD - 1, yD)]
                            + velocY[IX(xD, yD + 1)]
                            - velocY[IX(xD, yD - 1)]

                        ) / N;
                    p[IX(xD, yD)] = 0;
                }
            }
            
            set_bnd(0, div);
            set_bnd(0, p);
            lin_solve(0, p, div, 1, 6);
                     
            for (int yD = 1; yD < N - 1; yD++)
            {
                for (int xD = 1; xD < N - 1; xD++)
                {
                    velocX[IX(xD, yD)] -= 0.5f * (p[IX(xD + 1, yD)] - p[IX(xD - 1, yD)]) * N;
                    velocY[IX(xD, yD)] -= 0.5f * (p[IX(xD, yD + 1)] - p[IX(xD, yD - 1)]) * N;
                }
            }
            
            set_bnd(1, velocX);
            set_bnd(2, velocY);
        }

        void advect(int b, float[] d, float[] d0, float[] velocX, float[] velocY, float dt)
        {
            float i0, i1, j0, j1;

            float dtx = dt * (N - 2);
            float dty = dt * (N - 2);

            float s0, s1, t0, t1;
            float tmp1, tmp2, x, y;

            float Nfloat = N - 2;
            float ifloat, jfloat;
            int xD, yD;

            for (yD = 1, jfloat = 1; yD < N - 1; yD++, jfloat++)
            {
                for (xD = 1, ifloat = 1; xD < N - 1; xD++, ifloat++)
                {
                    tmp1 = dtx * velocX[IX(xD, yD)];
                    tmp2 = dty * velocY[IX(xD, yD)];
                    x = ifloat - tmp1;
                    y = jfloat - tmp2;

                    if (x < 0.5f) x = 0.5f;
                    if (x > Nfloat + 0.5f) x = Nfloat + 0.5f;
                    i0 = (float)Math.Floor(x);
                    i1 = i0 + 1.0f;
                    if (y < 0.5) y = 0.5f;
                    if (y > Nfloat + 0.5f) y = Nfloat + 0.5f;
                    j0 = (float)Math.Floor(y);
                    j1 = j0 + 1.0f;

                    s1 = x - i0;
                    s0 = 1.0f - s1;
                    t1 = y - j0;
                    t0 = 1.0f - t1;

                    int i0i = (int)i0;
                    int i1i = (int)i1;
                    int j0i = (int)j0;
                    int j1i = (int)j1;

                    d[IX(xD, yD)] =
                      s0 * (t0 * d0[IX(i0i, j0i)] + t1 * d0[IX(i0i, j1i)]) +
                      s1 * (t0 * d0[IX(i1i, j0i)] + t1 * d0[IX(i1i, j1i)]);
                }
            }

            set_bnd(b, d);
        }

        void set_bnd(int b, float[] x)
        {

            for (int xD = 1; xD < N - 1; xD++)
            {
                x[IX(xD, 0)] = b == 2 ? -x[IX(xD, 1)] : x[IX(xD, 1)];
                x[IX(xD, N - 1)] = b == 2 ? -x[IX(xD, N - 2)] : x[IX(xD, N - 2)];
            }
            for (int yD = 1; yD < N - 1; yD++)
            {
                x[IX(0, yD)] = b == 1 ? -x[IX(1, yD)] : x[IX(1, yD)];
                x[IX(N - 1, yD)] = b == 1 ? -x[IX(N - 2, yD)] : x[IX(N - 2, yD)];
            }

            x[IX(0, 0)] = 0.5f * (x[IX(1, 0)] + x[IX(0, 1)]);
            x[IX(0, N - 1)] = 0.5f * (x[IX(1, N - 1)] + x[IX(0, N - 2)]);
            x[IX(N - 1, 0)] = 0.5f * (x[IX(N - 2, 0)] + x[IX(N - 1, 1)]);
            x[IX(N - 1, N - 1)] = 0.5f * (x[IX(N - 2, N - 1)] + x[IX(N - 1, N - 2)]);
        }

        public void Step()
        {
            var N = Size;
            var visc = Viscosity;
            var diff = Diffusion;
            var dt = Dt;
            var Vx = this.Vx;
            var Vy = this.Vy;
            var Vx0 = this.Vx0;
            var Vy0 = this.Vy0;
            var s = S;
            var density = Density;

            diffuse(1, Vx0, Vx, visc, dt);
            diffuse(2, Vy0, Vy, visc, dt);

            project(Vx0, Vy0, Vx, Vy);

            advect(1, Vx, Vx0, Vx0, Vy0, dt);
            advect(2, Vy, Vy0, Vx0, Vy0, dt);

            project(Vx, Vy, Vx0, Vy0);
            diffuse(0, s, density, diff, dt);
            advect(0, density, s, Vx, Vy, dt);
        }

        public void render()
        {
            for (int xD = 0; xD < N; xD++)
            {
                for (int yD = 0; yD < N; yD++)
                {
                    var x = xD;
                    var y = yD;

                    var d = Density[IX(xD, yD)] > 254 ? 255 : Density[IX(xD, yD)];            
                    RenderOnSprite.DrawToPixel(x, y, new Color(255, 0, 255,(byte)d));
                }
            }
        }   
    }
}
