// See https://aka.ms/new-console-template for more information

using FantasyLogistics.JSON;
using FantasyLogistics.Noise;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Schema.Generation;
using SFML.Graphics;
using SFML.Window;
using Color = SFML.Graphics.Color;

namespace FantasyLogistics;

class Program
{
    const int WIDTH = 640;
    const int HEIGHT = 480;
    const string TITLE = "SHMUP";

    

    static void Main(string[] args)
    {
        JSchemaGenerator schemaGenerator = new JSchemaGenerator();

        JSchema schema = schemaGenerator.Generate(typeof(WorldConfig));
        Console.WriteLine(schema);

        VideoMode mode = new VideoMode(WIDTH, HEIGHT);
        RenderWindow window = new RenderWindow(mode, TITLE);

        window.SetVerticalSyncEnabled(true);

        window.Closed += (sender, args) => window.Close();

        Image screenImage = new Image(WIDTH, HEIGHT);

        



        Texture t = new Texture(screenImage);

        Sprite background = new Sprite(t);

        while (window.IsOpen)
        {
            window.DispatchEvents();
            window.Clear(Color.Blue);
            window.Draw(background);
            window.Display();
        }

        
    }
}