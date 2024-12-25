namespace TagsCloudContainerCore.Models.Graphics;

public class Color
{
    private readonly uint _color;
    
    public Color(byte r, byte g, byte b)
    {
        _color = (0xff000000u | (uint)(r << 16) | (uint)(g << 8) | b);
    }
    
    public uint ToUint() => _color;
    
    public byte Red => (byte)((_color >> 16) & 0xff);
    public byte Green => (byte)((_color >> 8) & 0xff);
    public byte Blue => (byte)((_color) & 0xff);
    
}