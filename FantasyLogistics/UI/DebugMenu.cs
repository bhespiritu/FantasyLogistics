using FantasyLogistics.Render;
using FantasyLogistics.World;
using ImGuiNET;

namespace FantasyLogistics.UI;

public class DebugMenu
{

    private bool rendererSettingsOpen = false;
    private int chunkRendererIndex = 0;

    private bool noiseEditorOpen = false;

    private String[] chunkRendererOptions;

    private MapWindow context;

    private FlatColorChunkRendererDebugUI chunkDebugUI;
    private NoiseEditorDebugUI noiseDebugUI;

    public DebugMenu(MapWindow context)
    {
        this.context = context;
        
        FlatColorChunkRenderer chunkRenderer = (FlatColorChunkRenderer)context.chunkRenderer;
        chunkDebugUI = new FlatColorChunkRendererDebugUI(chunkRenderer);

        noiseDebugUI = new NoiseEditorDebugUI((PerlinNoiseChunkProvider)((WorldLayer<float>) context.world.getWorldLayer(0)).GetProvider());
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
            
            chunkDebugUI.DrawUI();
            if (chunkDebugUI.clean())
            {
                context.updateTexture();
            }

            ImGui.Unindent();
            ImGui.Separator();
        }
        
        ImGui.Checkbox("Open Noise Editor", ref noiseEditorOpen);
        ImGui.Separator();
        if (noiseEditorOpen)
        {
            ImGui.Indent();
            
            noiseDebugUI.DrawUI();
            if (noiseDebugUI.clean())
            {
                context.updateNoise();
                context.updateTexture();
            }

            if (noiseDebugUI.cleanErode())
            {
                context.updateNoise();
                context.doErosion();
                context.updateTexture();
            }

            ImGui.Unindent();
            ImGui.Separator();
        }
        
        ImGui.End();
    }
}