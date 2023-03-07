namespace FantasyLogistics;

public class ConfigLoader
{
    private static ConfigLoader INSTANCE;

    public static ConfigLoader getInstance()
    {
        if (INSTANCE == null)
        {
            INSTANCE = new ConfigLoader();
        }

        return INSTANCE;
    }
    
    
    public String getWorldBasePath()
    {
        return "/dev/null";
    }
}