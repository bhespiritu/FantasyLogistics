using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Numerics;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace FantasyLogistics.World
{
    public abstract class WorldLayer
    {

        public abstract WorldChunk RequestChunk(Vector2 worldCoords);

        public abstract WorldChunk RequestChunk(float x, float y);

        public abstract int getChunkResolution();
    }

    public class WorldLayer<T> : WorldLayer
    {
        private IWorldChunkProvider<T> provider;
        private readonly int chunkResolution;

        private Dictionary<Vector2, WorldChunk<T>> cache = new Dictionary<Vector2, WorldChunk<T>>();

        public WorldLayer(int chunkResolution)
        {
            this.chunkResolution=chunkResolution;
        }

        public override WorldChunk<T> RequestChunk(Vector2 worldCoords)
        {
            if (!cache.ContainsKey(worldCoords))
            {
                cache[worldCoords] = provider.RequestChunk(worldCoords);
            }

            return cache[worldCoords];
        }

        public override WorldChunk<T> RequestChunk(float x, float y)
        {
            return RequestChunk(new Vector2(x, y));
        }

        public override int getChunkResolution()
        {
            return chunkResolution;
        }

        public IWorldChunkProvider<T> GetProvider()
        {
            return this.provider;
        }

        public void SetProvider(IWorldChunkProvider<T> provider)
        {
            this.provider = provider;
        }
    }

}
