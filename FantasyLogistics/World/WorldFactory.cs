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

            //world.addWorldLayer(new TerrainHeightWorldLayer());

            return world;
        }
    }
}
