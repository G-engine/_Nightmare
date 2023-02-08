using Microsoft.Xna.Framework.Graphics;
namespace AndroidGame;

public class Item
{
    public uint X;
    public uint Y;
    public uint Floor;
    public uint Room;
    public float Scale;
    public bool IsVisible;
    
    public Texture2D Texture;

    public Item(uint x, uint y, uint floor, uint room)
    {
        X = x;
        Y = y;
        Floor = floor;
        Room = room;
        Scale = 0;
        IsVisible = false;
    }
    
    public void ResetParams(uint x, uint y, uint floor, uint room)
    {
        X = x;
        Y = y;
        Floor = floor;
        Room = room;
        Scale = 0;
        IsVisible = false;
    }
}