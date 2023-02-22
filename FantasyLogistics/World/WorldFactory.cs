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

            WorldLayer<float> ground = new WorldLayer<float>(64);
            ground.SetProvider(new PerlinNoiseChunkProvider(64));
            world.addWorldLayer(ground);

            return world;
        }
    }
}
