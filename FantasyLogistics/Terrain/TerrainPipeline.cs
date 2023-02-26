using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FantasyLogistics.World;

namespace FantasyLogistics.Terrain
{
    public class TerrainPipeline
    {
        private List<PipelineStage> stages;

        
    }

    public abstract class PipelineStage
    {
        public World.World worldReference;

        public abstract bool process();
    }

    public enum StageResult
    {
        FAILURE, PARTIAL_FAILURE, SUCCESS
    }


}
