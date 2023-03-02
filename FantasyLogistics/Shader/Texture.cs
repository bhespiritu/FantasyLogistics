namespace FantasyLogistics.Shader;

using System;
using OpenTK.Graphics.OpenGL;
using StbImageSharp;

public class Texture
{
    protected int Handle;
    public Texture(int glHandle)
    {
        Handle = glHandle;
        Use();
    }

    public static Texture LoadFromSFMLImage(SFML.Graphics.Image image)
    {
        int handle = GL.GenTexture();
        
        GL.ActiveTexture(TextureUnit.Texture0);
        GL.BindTexture(TextureTarget.Texture2D, handle);

        byte[] data = image.Pixels;
        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, (int) image.Size.X, (int) image.Size.Y, 0, PixelFormat.Rgba, PixelType.UnsignedByte, data);
        
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

        // Now, set the wrapping mode. S is for the X axis, and T is for the Y axis.
        // We set this to Repeat so that textures will repeat when wrapped. Not demonstrated here since the texture coordinates exactly match
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Clamp);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Clamp);

        return new Texture(handle);
    }
    
    public void Use(TextureUnit unit = TextureUnit.Texture0)
    {
        GL.ActiveTexture(unit);
        GL.BindTexture(TextureTarget.Texture2D, Handle);
    }
    
}