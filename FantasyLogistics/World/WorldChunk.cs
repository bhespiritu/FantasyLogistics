using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasyLogistics.World
{
    public abstract class WorldChunk
    {
        public static readonly int CHUNK_RESOLUTION = 64;
        



    }
    public class WorldChunk<T> : WorldChunk
    {
        private T[] _chunkData = new T[CHUNK_RESOLUTION];



    }
}
