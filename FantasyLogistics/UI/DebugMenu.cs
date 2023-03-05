using FantasyLogistics.Render;
using ImGuiNET;

namespace FantasyLogistics.UI;

public class DebugMenu
{

    private bool rendererSettingsOpen = false;
    private int chunkRendererIndex = 0;

    private String[] chunkRendererOptions;

    private MapWindow context;

    private FlatColorChunkRendererDebugUI debugUi;

    public DebugMenu(MapWindow context)
    {
        this.context = context;
        
        FlatColorChunkRenderer chunkRenderer = (FlatColorChunkRenderer)context.chunkRenderer;
        debugUi = new FlatColorChunkRendererDebugUI(chunkRenderer);
    }

    public void RenderMenu()
    {
        ImGui.Begin("Debug Menu");

        ImGui.Checkbox("Open Renderer Settings", ref rendererSettingsOpen);
        ImGui.Separator();
        if (rendererSettingsOpen)
        {
            ImGui.Indent();

            ImGui.Combo("Renderer", ref chunkRendererIndex, typeof(FlatColorChunkRenderer).Name);
            
            debugUi.DrawUI();
            if (debugUi.clean())
            {
                context.updateTexture();
            }
            
            ImGui.Unindent();
            ImGui.Separator();
        }
        
        ImGui.End();
    }
}