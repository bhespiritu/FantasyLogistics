using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace FantasyLogistics.Shader;

public class UpdateTexture : Texture
{
    public uint Width { get; }
    public uint Height { get; }

    private float[,,] data;

    public UpdateTexture(int glHandle, int width, int height) : base(glHandle)
    {
        Width = (uint)width;
        Height = (uint)height;
        data = new float[width, height, 4];
    }

    public void Update()
    {
        GL.TexSubImage2D(TextureTarget.Texture2D, 0, 0, 0,(int) Width,(int) Height, PixelFormat.Rgba, PixelType.Float, data);
    }

    public void SetPixel(Color4 val, int x, int y)
    {
        data[y, x, 0] = val.R;
        data[y, x, 1] = val.G;
        data[y, x, 2] = val.B;
        data[y, x, 3] = val.A;
        
    }

    public static UpdateTexture generateNew(int width, int height)
    {
        int handle = GL.GenTexture();
        
        GL.ActiveTexture(TextureUnit.Texture0);
        GL.BindTexture(TextureTarget.Texture2D, handle);
        UpdateTexture texture = new UpdateTexture(handle, width, height);

        float[,,] data = texture.data;
        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, (int) width, (int) height, 0, PixelFormat.Rgba, PixelType.Float, data);
        
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

        // Now, set the wrapping mode. S is for the X axis, and T is for the Y axis.
        // We set this to Repeat so that textures will repeat when wrapped. Not demonstrated here since the texture coordinates exactly match
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Clamp);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Clamp);

        return texture;
    }

    public void Use(TextureUnit unit = TextureUnit.Texture0)
    {
        Update();
        base.Use(unit);
    }
    
}