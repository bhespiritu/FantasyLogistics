// See https://aka.ms/new-console-template for more information

using FantasyLogistics.JSON;
using FantasyLogistics.Noise;
using FantasyLogistics.Terrain;
using FantasyLogistics.World;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Schema.Generation;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using Color = SFML.Graphics.Color;

namespace FantasyLogistics;

class Program
{
    const int WIDTH = 512+256;
    const int HEIGHT = 256;
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

        Image screenImage = new Image(256, 256);
        Image screenImage2 = new Image(256, 256);
        Image screenImage3 = new Image(256, 256);


        World.World w = WorldFactory.BuildDefaultWorld();

        ErosionStage stage = new ErosionStage();
        stage.worldReference = w;

        float[,] delta;

        {
            WorldLayer<float> layer = (WorldLayer<float>)w.getWorldLayer(0);
            WorldChunk<float> chunk = layer.RequestChunk(0, 0);

            delta = (float[,])chunk._chunkData.Clone();
            for (uint x = 0; x < 256; x++)
            {
                for (uint y = 0; y < 256; y++)
                {
                    float layerVal = chunk._chunkData[x, y];
                    if(layerVal > 0.75f)
                    {
                        byte lum = (byte)(255 * MathF.Log10(layerVal * 50));
                        screenImage.SetPixel(x, y, new Color(lum, lum, lum));
                    }
                    else
                    {
                        byte lum = (byte)(255 * MathF.Log10(layerVal * 50));
                        screenImage.SetPixel(x, y, new Color(0, 0, lum));
                    }
                }
            }
        }

        

        stage.process();


        

        {
            WorldLayer<float> layer = (WorldLayer<float>)w.getWorldLayer(0);
            WorldChunk<float> chunk = layer.RequestChunk(0, 0);

            for (uint x = 0; x < 256; x++)
            {
                for (uint y = 0; y < 256; y++)
                {
                    delta[x,y] -= chunk._chunkData[x, y];
                }
            }

            for (uint x = 0; x < 256; x++)
            {
                for (uint y = 0; y < 256; y++)
                {
                    float layerVal = chunk._chunkData[x, y];
                    float deltaVal = MathF.Max(MathF.Min(delta[x, y],1),-1);
                    
                    if (layerVal > 0.75f)
                    {
                        byte lum = (byte)(255 * MathF.Log10(layerVal * 50));
                        screenImage2.SetPixel(x, y, new Color(lum, lum, lum));
                        screenImage3.SetPixel(x, y, new Color(lum, lum, lum));
                    }
                    else
                    {
                        byte lum = (byte)(255 * MathF.Log10(layerVal * 50));
                        screenImage2.SetPixel(x, y, new Color(0, 0, lum));
                        screenImage3.SetPixel(x, y, new Color(0, 0, lum));
                    }
                    if(deltaVal*deltaVal > 0)
                    {
                        if (layerVal > 0)
                        {
                            byte lum = (byte)(255 * deltaVal);
                            screenImage2.SetPixel(x, y, new Color(0, lum, 0));
                        }
                        else
                        {
                            byte lum = (byte)(255 * -deltaVal);
                            screenImage2.SetPixel(x, y, new Color(lum, 0, 0));
                        }
                    }
                }
            }
        }



        Texture t = new Texture(screenImage);

        Sprite background = new Sprite(t);

        Texture t2 = new Texture(screenImage2);

        Sprite background2 = new Sprite(t2);

        background2.Position = new Vector2f(256, 0);

        Texture t3 = new Texture(screenImage3);

        Sprite background3 = new Sprite(t3);

        background3.Position = new Vector2f(512, 0);

        while (window.IsOpen)
        {
            window.DispatchEvents();
            window.Clear(Color.Blue);
            window.Draw(background);
            window.Draw(background2);
            window.Draw(background3);
            window.Display();
        }

        
    }
}