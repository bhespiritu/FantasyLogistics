using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using FantasyLogistics.Noise;

namespace FantasyLogistics.World
{
    internal class PerlinNoiseChunkProvider : IWorldChunkProvider<float>
    {
        private readonly int size;

        public PerlinNoiseChunkProvider(int size)
        {
            this.size = size;
        }

        public WorldChunk<float> RequestChunk(float x, float y)
        {
            WorldChunk<float> output = new WorldChunk<float>(size);

            List<NoiseLayer> noiseLayers = new List<NoiseLayer>();
            noiseLayers.Add(new NoiseLayer(0, 0, 0.75f, 0.5f));
            for (int i = 2; i <= 4; i++)
            {
                noiseLayers.Add(new NoiseLayer(0, 0, (float)Math.Pow(0.5, i), (float)Math.Pow(1.75, i)));
            }
            NoiseLayer noise = new NoiseLayer();
            float max = 0;

            for (uint x1 = 0; x1 < size; x1++)
            {
                for (uint y1 = 0; y1 < size; y1++)
                {
                    float value = 0;
                    for (int i = 0; i < noiseLayers.Count; i++)
                    {
                        float noiseValFloat = (noiseLayers[i].sampleNoise(x1, y1)+1f)/2f;
                        value += noiseValFloat;

                        value = Math.Max(0, value);
                        if (value > max) max = value;
                    }

                    output._chunkData[x1,y1] = value;

                }
            }

            for (uint x1 = 0; x1 < size; x1++)
            {
                for (uint y1 = 0; y1 < size; y1++)
                {
                    output._chunkData[x1, y1] /= max;

                }
            }

            return output;
        }
    }
}
