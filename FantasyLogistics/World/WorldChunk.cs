using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasyLogistics.World
{
    public abstract class WorldChunk
    {
        



    }
    public class WorldChunk<T> : WorldChunk
    {
        public readonly int size;

        public WorldChunk(int size)
        {
            this.size = size;
            _chunkData = new T[size, size];
        }

        public T[,] _chunkData ;



    }
}
