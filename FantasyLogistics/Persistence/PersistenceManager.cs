using FantasyLogistics.World;

namespace FantasyLogistics.File;

public abstract class PersistenceManager
{
    public abstract bool storeWorld(World.World w);

    public abstract World.World loadWorld();
}