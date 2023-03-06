using System.Numerics;
using System.Runtime.InteropServices;
using FantasyLogistics.UI;
using FantasyLogistics.World;
using ImGuiNET;
using SFML.Graphics;

namespace FantasyLogistics.Render;

public class FlatColorChunkRenderer : ChunkRenderer<float>
{

    public SortedList<float, Color> ranges = new SortedList<float, Color>();

    public override byte[] renderChunk(WorldChunk<float> chunk)
    {
        int width = chunk.size;
        int height = chunk.size;

        byte[] data = new byte[4 * width * height];

        for (int x = 1; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int index = x * 4 + y * 4 * width;

                float slope = chunk._chunkData[x, y] - chunk._chunkData[x - 1, y];
                float value = chunk._chunkData[x, y];
                Color color = getColor(value);
                
                data[index + 0] = (byte)(color.R*value);
                data[index + 1] = (byte)(color.G*value);
                data[index + 2] = (byte)(color.B*value);
                data[index + 3] = (byte)(color.A*value);
            }
        }

        return data;
    }

    private Color getColor(float threshold)
    {
        foreach(KeyValuePair<float, Color> range in ranges)
        {
            if (threshold <= range.Key) return range.Value;
        }
        return Color.Black;
    }
        
}

public class DefaultFlatColorChunkRenderer : FlatColorChunkRenderer
{
    public DefaultFlatColorChunkRenderer()
    {
        ranges.Add(0.75f, Color.Blue);
        ranges.Add(0.76f, Color.Yellow);
        ranges.Add(0.85f, new Color(0,0x88,0));
        ranges.Add(0.93f, new Color(0x44, 0x44, 0x44));
        ranges.Add(1.00f, Color.White);
    }
}

public class FlatColorChunkRendererDebugUI : ChunkRendererDebugUI<FlatColorChunkRenderer>
{
    private List<ColorSetting> settings;

    public FlatColorChunkRendererDebugUI(FlatColorChunkRenderer target) : base(target)
    {
        settings = new List<ColorSetting>();
        foreach (KeyValuePair<float, Color> entry in target.ranges)
        {
            Vector3 colorAsVector = new Vector3();
            colorAsVector.X = entry.Value.R / 255f;
            colorAsVector.Y = entry.Value.G / 255f;
            colorAsVector.Z = entry.Value.B / 255f;
            
            settings.Add(new ColorSetting(entry.Key, colorAsVector));
        }
    }

    protected override void Draw()
    {
        var listSpan = CollectionsMarshal.AsSpan(settings);
        int? toRemove = null;
        for (int i = 0; i < settings.Count; i++)
        {
                ImGui.Separator();
                ImGui.DragFloat("Threshold " + i, ref listSpan[i].threshold, 0.001f, 0, 1);
                ImGui.ColorEdit3("Color " + i, ref listSpan[i].color);
                if(ImGui.Button("Remove " + i))
                {
                    toRemove = i;
                }
                ImGui.Separator();
        }

        if (toRemove.HasValue)
        {
            settings.RemoveAt(toRemove.Value);
        }


        if (ImGui.Button("+"))
        {
            settings.Add(new ColorSetting(0, Vector3.One));
        }
    }

    protected override void Update()
    {
        target.ranges.Clear();
        foreach (ColorSetting setting in settings)
        {
            if(!target.ranges.ContainsKey(setting.threshold))
            {
                Color c = new Color();
                c.R = (byte)(255 * setting.color.X);
                c.G = (byte)(255 * setting.color.Y);
                c.B = (byte)(255 * setting.color.Z);
                c.A = 255;

                target.ranges.Add(setting.threshold, c);
            }
        }
    }


    private struct ColorSetting
    {
        public float threshold;
        public Vector3 color;

        public ColorSetting(float threshold, Vector3 color)
        {
            this.threshold = threshold;
            this.color = color;
        }
    }
    
} 