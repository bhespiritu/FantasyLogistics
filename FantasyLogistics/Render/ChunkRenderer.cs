using FantasyLogistics.World;

namespace FantasyLogistics.Render;

public abstract class ChunkRenderer<T>
{
    public abstract byte[] renderChunk(WorldChunk<T> chunk);
}