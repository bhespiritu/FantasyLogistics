using ImGuiNET;

namespace FantasyLogistics.UI;

public abstract class ChunkRendererDebugUI<T>
{
    protected T target;
    protected bool autoUpdate = false;

    private bool dirty;//TODO extract into own class/interface

    public ChunkRendererDebugUI(T target)
    {
        this.target = target;
    }

    public void DrawUI()
    {
        Draw();
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

    public void flagDirty()
    {
        dirty = true;
    }
    
    protected abstract void Draw();
    protected abstract void Update();
}