using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using FantasyLogistics.World;

namespace FantasyLogistics.Terrain
{
    public class ErosionStage : PipelineStage
    {
        
        public override bool process()
        {
            WorldLayer heightLayer = worldReference.getWorldLayer(0);
            int iteration = 1000;
            int maxLifetime = 30;
            float inertia = 0.5f;
            float initialWater = 1;

            for (int i = 0; i < iteration; i++)
            {
                float px, py;
                float dx, dy;
                float speed;
                float water = initialWater;
                float sediment = 0;

                for (int j = 0; j < maxLifetime; j++)
                {

                }
            }

            return true;
        }
    }
}
