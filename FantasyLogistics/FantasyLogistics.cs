using System.Text.RegularExpressions;
using FantasyLogistics.Render;
using FantasyLogistics.Shader;
using FantasyLogistics.Terrain;
using FantasyLogistics.UI;
using FantasyLogistics.World;
using ImGuiNET;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using SFML.Graphics;
using Buffer = System.Buffer;
using Image = SFML.Graphics.Image;
using PrimitiveType = OpenTK.Graphics.OpenGL.PrimitiveType;
using Texture = FantasyLogistics.Shader.Texture;

namespace FantasyLogistics
{
    
    public class FantasyLogistics
    {
        public static void Main(string[] args)
        {
            // uint height = 21;
            // uint width = 10;
            // Image testImage = new Image(width, height);
            // int i = 0;
            // for (int x = 0; x < width; x++)
            // {
            //     for (int y = 0; y < height; y++)
            //     {
            //         float fraction = (float)(i++) / (float)(width * height);
            //         byte val = (byte)(255 * fraction);
            //         //Console.WriteLine(i + " f "+ fraction + " " + val);
            //         testImage.SetPixel((uint)x,(uint)y, new Color(val,val,val,255));
            //     }
            // }
            //
            // byte[] data = testImage.Pixels;
            // //Console.WriteLine(Convert.ToHexString(data));
            //
            // String hex = Convert.ToHexString(data);
            // Console.WriteLine(data.Length);
            // Console.WriteLine(SpliceText(hex, (int)width*4));
            //
            //
            // for (int x = 0; x < width; x++)
            // {
            //     for (int y = 0; x < height; x++)
            //     {
            //         for (int c = 0; c < 4; c++)
            //         {
            //             int index = c + x * 4 + y * (int)width;
            //            // Console.WriteLine(x + " " + y + " " + c + " " + data[index]);
            //         }
            //     }
            // }
            
            GameWindowSettings gameWindowSettings = GameWindowSettings.Default;
            NativeWindowSettings nativeWindowSettings = NativeWindowSettings.Default;
            
            using (MapWindow window = new MapWindow(gameWindowSettings, nativeWindowSettings))
            {
                window.Run();
            }
        }
        
        public static string SpliceText(string text, int lineLength) {
            return Regex.Replace(text, "(.{" + lineLength + "})", "$1" + Environment.NewLine);
        } 
    }
    
    public class MapWindow : GameWindow
    {
        // Because we're adding a texture, we modify the vertex array to include texture coordinates.
        // Texture coordinates range from 0.0 to 1.0, with (0.0, 0.0) representing the bottom left, and (1.0, 1.0) representing the top right.
        // The new layout is three floats to create a vertex, then two floats to create the coordinates.
        private readonly float[] _vertices =
        {
            // Position         Texture coordinates
             1f,  -1f, 0.0f, 1.0f, 1.0f, // bottom right
             1f, 1f, 0.0f, 1.0f, 0.0f, // top right
            -1f, 1f, 0.0f, 0.0f, 0.0f, // top left
            -1f,  -1f, 0.0f, 0.0f, 1.0f  // bottom left
        };

        private readonly uint[] _indices =
        {
            0, 1, 3,
            1, 2, 3
        };

        private int _elementBufferObject;

        private int _vertexBufferObject;

        private int _vertexArrayObject;

        private Shader.Shader _shader;

        public UpdateTexture texture;

        private ImGuiController _controller;

        private DebugMenu _debugMenu;

        public ChunkRenderer<float> chunkRenderer;
        public WorldChunk<float> chunk;

        public World.World world;

        public MapWindow(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings)
        {
            chunkRenderer = new DefaultFlatColorChunkRenderer();
            
            
            world = WorldFactory.BuildDefaultWorld();

            updateNoise();
            
            _debugMenu = new DebugMenu(this);
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            _controller = new ImGuiController(ClientSize.X, ClientSize.Y);

            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

            _vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayObject);

            _vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

            _elementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);

            // The shaders have been modified to include the texture coordinates, check them out after finishing the OnLoad function.
            _shader = new Shader.Shader("Shader/base.vert", "Shader/base.frag");
            _shader.Use();

            // Because there's now 5 floats between the start of the first vertex and the start of the second,
            // we modify the stride from 3 * sizeof(float) to 5 * sizeof(float).
            // This will now pass the new vertex array to the buffer.
            var vertexLocation = _shader.GetAttribLocation("aPosition");
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

            // Next, we also setup texture coordinates. It works in much the same way.
            // We add an offset of 3, since the texture coordinates comes after the position data.
            // We also change the amount of data to 2 because there's only 2 floats for texture coordinates.
            var texCoordLocation = _shader.GetAttribLocation("aTexCoord");
            GL.EnableVertexAttribArray(texCoordLocation);
            GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));

            
            

            
            
            texture = UpdateTexture.generateNew(chunk.size, chunk.size);

            updateNoise();
            doErosion();
            updateTexture();
        }

        public void updateTexture()
        {
            byte[] colors = chunkRenderer.renderChunk(chunk);

            texture.data = colors;
            
            texture.Use(TextureUnit.Texture0);
        }
        
        public void updateNoise()
        {
            chunk = ((WorldLayer<float>) world.getWorldLayer(0)).RequestChunk(0, 0, true);
        }
        
        public void doErosion()
        {
            ErosionStage stage = new ErosionStage();
            stage.worldReference = world;

            stage.process();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit);

            GL.BindVertexArray(_vertexArrayObject);

            texture.Use(TextureUnit.Texture0);
            _shader.Use();

            GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);

            _controller.Update(this, (float)e.Time);
            
            _debugMenu.RenderMenu();

            _controller.Render();

            ImGuiController.CheckGLError("End of frame");
            
            SwapBuffers();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            var input = KeyboardState;

            if (input.IsKeyDown(Keys.Escape))
            {
                Close();
            }
            
            if (input.IsKeyDown(Keys.Space))
            {
                for (int x = 30; x < 35; x++)
                {
                    for (int y = 30; y < 35; y++)
                    {
                        texture.SetPixel(Color.Magenta, x,y);
                    }
                }
                texture.Update();
            }
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0, 0, Size.X, Size.Y);
            
            _controller.WindowResized(ClientSize.X, ClientSize.Y);
        }
        
        protected override void OnTextInput(TextInputEventArgs e)
        {
            base.OnTextInput(e);
            
            
            _controller.PressChar((char)e.Unicode);
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);
            
            _controller.MouseScroll(e.Offset);
        }
    }
}
