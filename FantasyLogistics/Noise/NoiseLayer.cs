using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasyLogistics.Noise
{
    public struct NoiseLayerSettings
    {
        public int offsetX;
        public int offsetY;
        public float scale;
        public float power;

        public NoiseLayerSettings(int offsetX, int offsetY, float power, float scale)
        {
            this.offsetX = offsetX;
            this.offsetY = offsetY;
            this.power = power;
            this.scale = scale;
        }
    }

    public class NoiseLayer
    {
        private static FastNoiseLite noise;
        public NoiseLayerSettings _noiseLayerSettings;

        public NoiseLayer(int offsetX = 0, int offsetY = 0, float power = 1, float scale = 1)
        {
            _noiseLayerSettings = new NoiseLayerSettings(offsetX, offsetY, power, scale);
            noise = new FastNoiseLite();
            noise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
        }
        
        public NoiseLayer(NoiseLayerSettings settings)
        {
            _noiseLayerSettings = settings;
            noise = new FastNoiseLite();
            noise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
        }

        public float sampleNoise(float x, float y)
        {
            x = (x * _noiseLayerSettings.scale) + _noiseLayerSettings.offsetX;
            y = (y * _noiseLayerSettings.scale) + _noiseLayerSettings.offsetY;
            return  noise.GetNoise(x,y)* _noiseLayerSettings.power;
        }

    }
}
