using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using FantasyLogistics.World;

namespace FantasyLogistics.Terrain
{
    public class ErosionStage : PipelineStage
    {
        
        public override bool process()
        {
            WorldLayer<float> heightLayer = (WorldLayer<float>) worldReference.getWorldLayer(0);
            int iteration = 50000;
            int maxLifetime = 30;
            float inertia = 0.5f;
            float initialWater = 1;
            float initialSpeed = 1;
            float sedimentFactor = 4;
            float minSedimentCap = 0.1f;
            float depositSpeed = .03f;
            float erodeSpeed = .03f;
            float gravity = 4;
            float evaporateSpeed = 0.1f;

            float[,] brush =
                {   { .015625f, .015625f, .015625f, .015625f, .015625f}, 
                    { .015625f, .0625f,   .0625f,   .0625f,   .015625f},
                    { .015625f, .0625f,   .25f,     .0625f,   .015625f},
                    { .015625f, .0625f,   .0625f,   .0625f,   .015625f},
                    { .015625f, .015625f ,.015625f ,.015625f ,.015625f}
                };

            Random random = new Random();

            WorldChunk<float> chunk = heightLayer.RequestChunk(0, 0);

            int size = 255;
            for (int i = 0; i < iteration; i++)
            {
                //Console.WriteLine("NEW ITERATION");
                float px = (float)(size*random.NextDouble()), py = (float)(size*random.NextDouble());
                float dx = 0, dy = 0;
                float speed = initialSpeed;
                float water = initialWater;
                float sediment = 0;


                for (int j = 0; j < maxLifetime; j++)
                {
                    int nx = (int)px;
                    int ny = (int)py;

                    if (nx > 254 || nx < 0 || ny > 254 || ny < 0) break;

                    float xOffset = px - nx;
                    float yOffset = py - ny;


                    float height, gx, gy;
                    calcHeightandGradient(px,py,chunk._chunkData, out height, out gx, out gy);

                    dx = (dx * inertia - gx * (1 - inertia));
                    dy = (dy * inertia - gy * (1 - inertia));

                    float len = MathF.Sqrt(dx * dx + dy * dy);
                    if (len != 0)
                    {
                        dx /= len;
                        dy /= len;
                    }

                    px += dx;
                    py += dy;

                    if ((dx == 0 && dy == 0) || px < 0 || px >= size - 1 || py < 0 || py >= size - 1 || float.IsNaN(px) || float.IsNaN(py)) break;

                    float a, b;
                    float newHeight;
                    calcHeightandGradient(px, py, chunk._chunkData, out newHeight, out a, out b);
                    float dh = newHeight - height;

                    float sedimentCapacity = MathF.Max(-dh * speed * water * sedimentFactor, minSedimentCap);

                    if (sediment > sedimentCapacity || dh > 0)
                    {
                        float amountToDeposit =
                            (dh > 0) ? MathF.Min(dh, sediment) : (sediment - sedimentCapacity) * depositSpeed;
                        sediment -= amountToDeposit;

                        //Console.WriteLine(px + " " + py + " +"+amountToDeposit);

                        chunk._chunkData[nx, ny] += amountToDeposit * (1 - xOffset) * (1 - yOffset);
                        chunk._chunkData[nx+1, ny] += amountToDeposit * (xOffset) * (1 - yOffset);
                        chunk._chunkData[nx, ny+1] += amountToDeposit * (1 - xOffset) * (yOffset);
                        chunk._chunkData[nx+1, ny+1] += amountToDeposit * (xOffset) * (yOffset);
                    }
                    else
                    {
                        float amountToErode = MathF.Min((sedimentCapacity - sediment) * erodeSpeed, -dh);
                        //Console.WriteLine(px + " " + py + " -"+amountToErode);
                        for (int ix = -2; ix <= 2; ix++)
                        {
                            for (int iy = -2; iy <= 2; iy++)
                            {
                                if ((ix * ix + iy * iy) < 4)
                                {
                                    if(nx + ix < 0 || nx + ix >= size || ny + iy < 0 || ny + iy >= size) continue;
                                    float brushWeight = brush[ix + 2, iy + 2];
                                    chunk._chunkData[nx+ix,ny+iy] -= brushWeight * amountToErode;
                                    sediment += brushWeight*amountToErode;



                                }
                            }
                        }
                    }

                    speed = MathF.Sqrt(speed * speed + dh * gravity);
                    water *= (1 - evaporateSpeed);
                    //Console.WriteLine("{0} {1} {2} {3} {4}",px, py, sediment, height, speed);
                    if (chunk._chunkData[ny, nx] > 1)
                    {
                        Console.WriteLine("BREAK1");
                    }

                    //chunk._chunkData[ny, nx] = MathF.Min(chunk._chunkData[ny, nx], 0.9f);
                    //chunk._chunkData[ny, nx] = MathF.Max(chunk._chunkData[ny, nx], 0.1f);
                    if (chunk._chunkData[ny, nx] < 0.25)
                    {
                        Console.WriteLine("BREAK2");
                    }
                }
            }

            return true;
        }

        void calcHeightandGradient(float px, float py, float[,] data, out float height1, out float gx1, out float gy1)
        {
            int nx = (int)px;
            int ny = (int)py;
            float xOffset = px - nx;
            float yOffset = py - ny;

            float ul = data[nx, ny];
            float ur = data[nx + 1, ny];
            float ll = data[nx, ny + 1];
            float lr = data[nx + 1, ny + 1];

            height1 = bilerp(ul, ur, ll, lr, xOffset, yOffset);

            gx1 = (ul - ur) * (1 - yOffset) + (ll - lr) * yOffset;
            gy1 = (ll - ul) * (1 - xOffset) + (lr - ur) * xOffset;
        }

        private static float bilerp(float x1, float x2, float y1, float y2, float t1, float t2)
        {
            float top = lerp(x1, x2, t1);
            float bottom = lerp(y1, y2, t1);

            return lerp(top, bottom, t2);
        }

        private static float lerp(float a, float b, float t)
        {
            return a + (b - a) * t;
        }
    }
}
