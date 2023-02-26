using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasyLogistics.World
{
    public class WorldFactory
    {
        public static World BuildDefaultWorld()
        {
            World world = new World();

            WorldLayer<float> ground = new WorldLayer<float>(256);
            IWorldChunkProvider<float> chunkProvider = new PerlinNoiseChunkProvider(256);
            ground.SetProvider(chunkProvider);
            world.addWorldLayer(ground);

            return world;
        }
    }
}
