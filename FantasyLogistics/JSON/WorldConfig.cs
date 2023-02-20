using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FantasyLogistics.World;

namespace FantasyLogistics.JSON
{
    public class WorldConfig
    {
        public string name;
        public string description;

        public int sizeX, sizeY;

        private List<WorldLayer> layers;
    }
}
