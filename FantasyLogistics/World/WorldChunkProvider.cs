using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace FantasyLogistics.World
{
    public interface IWorldChunkProvider<T>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public WorldChunk<T> RequestChunk(float x, float y);

        public WorldChunk<T> RequestChunk(Vector2 worldCoord)
        {
            return RequestChunk(worldCoord.X, worldCoord.Y);
        }

        public Vector2 globalToChunk(Vector2 worldCoord)
        {
            return worldCoord;
        }

        public Vector2 globalToChunk(float x, float y)
        {
            return globalToChunk(new Vector2(x, y));
        }
    }
}
