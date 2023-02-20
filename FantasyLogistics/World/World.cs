using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace FantasyLogistics.World
{
    public class World
    {
        List<WorldLayer> layers;

        public World()
        {
            layers = new List<WorldLayer>();
        }

        public void addWorldLayer(WorldLayer layer)
        {
            layers.Add(layer);
        }

    }
}
