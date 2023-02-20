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

    public abstract class WorldLayer<T> : WorldLayer
    {
        private IWorldChunkProvider<T> provider;
        private readonly int chunkResolution;

        public WorldLayer(int chunkResolution)
        {
            this.chunkResolution=chunkResolution;
        }

        public override WorldChunk RequestChunk(Vector2 worldCoords)
        {
            return provider.RequestChunk(worldCoords);
        }

        public override WorldChunk RequestChunk(float x, float y)
        {
            return provider.RequestChunk(x, y);
        }

        public abstract int 
    }

}
