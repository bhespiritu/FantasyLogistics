using System.Runtime.InteropServices;
using FantasyLogistics.Noise;
using FantasyLogistics.World;
using ImGuiNET;
using SFML.Window;

namespace FantasyLogistics.UI;

public class NoiseEditorDebugUI
{
    private PerlinNoiseChunkProvider target;
    
    protected bool autoUpdate = false;

    private bool dirty;//TODO extract into own class/interface
    private bool dirtyErode;

    private List<NoiseLayerSettings> settings;
    

    public NoiseEditorDebugUI(PerlinNoiseChunkProvider target)
    {
        this.target = target;
        settings = new List<NoiseLayerSettings>();
        foreach (NoiseLayer setting in this.target.noiseLayers)
        {
            settings.Add(setting._noiseLayerSettings);
        }
    }

    public void DrawUI()
    {
        var listSpan = CollectionsMarshal.AsSpan(settings);
        for (int i = 0; i < listSpan.Length; i++)
        {
            ImGui.DragInt("Offset X " + i, ref listSpan[i].offsetX,0.1f);
            ImGui.DragInt("Offset Y " + i, ref listSpan[i].offsetY,0.1f);
            ImGui.DragFloat("Scale " + i, ref listSpan[i].scale,0.001f);
            ImGui.DragFloat("Power " + i, ref listSpan[i].power,0.001f);
            ImGui.Separator();
        }
        
        
        if (ImGui.Button("+"))
        {
            settings.Add(new NoiseLayerSettings());
        }
        
        if (ImGui.Button("Erode"))
        {
            flagErodeDirty();
            autoUpdate = false;
        }
        
        ImGui.Checkbox("Auto Update", ref autoUpdate);

        if (autoUpdate)
        {
            Update();
            flagDirty();
        }
        else
        {
            if (ImGui.Button("Update"))
            {
                Update();
                flagDirty();
            }
        }
    }

    public bool clean()
    {
        if (dirty)
        {
            dirty = false;
            return true;
        }

        return false;
    }
    
    public bool cleanErode()
    {
        if (dirtyErode)
        {
            dirtyErode = false;
            return true;
        }

        return false;
    }

    public void flagDirty()
    {
        dirty = true;
    }
    
    public void flagErodeDirty()
    {
        dirtyErode = true;
    }

    protected void Update()
    {
        target.noiseLayers.Clear();
        foreach (NoiseLayerSettings setting in settings)
        {
            target.noiseLayers.Add(new NoiseLayer(setting.offsetX, setting.offsetY, setting.power, setting.scale));
        }
    }
}