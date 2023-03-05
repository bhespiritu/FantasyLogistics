using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using SFML.Graphics;

namespace FantasyLogistics.Shader;

public class UpdateTexture : Texture
{
    public int Width { get; }
    public int Height { get; }

    public byte[] data;

    public UpdateTexture(int glHandle, int width, int height) : base(glHandle)
    {
        Width = width;
        Height = height;
        data = new byte[width* height* 4];
    }

    public void Update()
    {
        GL.TexSubImage2D(TextureTarget.Texture2D, 0, 0, 0,(int) Width,(int) Height, PixelFormat.Rgba, PixelType.UnsignedByte, data);
    }

    public void SetPixel(Color val, int x, int y)
    {
        data[I(x, y, 0)] = val.R;
        data[I(x, y, 1)] = val.G;
        data[I(x, y, 2)] = val.B;
        data[I(x, y, 3)] = val.A;
        
    }

    private int I(int x, int y, int c)
    {
        return c + x * 4 + y * Width*4;
    }

    public static UpdateTexture generateNew(int width, int height)
    {
        int handle = GL.GenTexture();
        
        GL.ActiveTexture(TextureUnit.Texture0);
        GL.BindTexture(TextureTarget.Texture2D, handle);
        UpdateTexture texture = new UpdateTexture(handle, width, height);

        byte[] data = texture.data;
        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, (int) width, (int) height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, data);
        
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