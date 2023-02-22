using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasyLogistics.Noise
{
    internal class NoiseLayer
    {
        public int offsetX, offsetY;
        public float scale;
        public float power;
        private FastNoiseLite noise;

        public NoiseLayer(int offsetX = 0, int offsetY = 0, float power = 1, float scale = 1)
        {
            this.offsetX = offsetX;
            this.offsetY = offsetY;
            this.power = power;
            this.scale = scale;

            noise = new FastNoiseLite();
            noise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
        }

        public float sampleNoise(float x, float y)
        {
            x = (x * scale) + offsetX;
            y = (y * scale) + offsetY;
            return  noise.GetNoise(x,y)*power;
        }

        private static float lerp(float a, float b, float t)
        {
            return a + (b - a) * t;
        }

    }
}
