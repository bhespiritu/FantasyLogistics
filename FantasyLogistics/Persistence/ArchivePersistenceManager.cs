using FantasyLogistics.World;
using Newtonsoft.Json;

namespace FantasyLogistics.File;

public class FilePersistenceManager : PersistenceManager
{
    private String basePath;
    
    public FilePersistenceManager()
    {
        basePath = ConfigLoader.getInstance().getWorldBasePath();
        prepare();
    }

    public void prepare()
    {
        Directory.CreateDirectory(basePath);
        
    }
    
    public override bool storeWorld(World.World w)
    {
        WorldInfo worldInfo = w.worldInfo;
        String worldPath = Path.Combine(basePath, worldInfo.name);
        
        Directory.CreateDirectory(worldPath);

        String json = JsonConvert.SerializeObject(worldInfo, Formatting.Indented);

        System.IO.File.WriteAllText(Path.Combine(worldPath, "world.json"),json);

        throw new NotImplementedException();
    }
    

    public override World.World loadWorld()
    {
        throw new NotImplementedException();
    }
}