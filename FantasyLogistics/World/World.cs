using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace FantasyLogistics.World
{
    public class World
    {
        private List<WorldLayer> layers;
        private Dictionary<String, WeakReference<WorldLayer>> labels;

        public World()
        {
            layers = new List<WorldLayer>();
        }

        public void addWorldLayer(WorldLayer layer, string label = null)
        {
            layers.Add(layer);
            if (label != null)
            {
                labels[label] = new WeakReference<WorldLayer>(layer);
            }
        }

        public WorldLayer getWorldLayer(int i)
        {
            return layers[i];
        }

        public WorldLayer getWorldLayer(string label)
        {
            WeakReference<WorldLayer> layerRef = labels[label];
            WorldLayer layer = null;
            if (!layerRef.TryGetTarget(out layer))
            {

            }
            return layer;
        }

        public void removeWorldLayer(int i)
        {
            layers.RemoveAt(i);
        }

    }
}
