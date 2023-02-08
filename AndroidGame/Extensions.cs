using Microsoft.Xna.Framework;

namespace AndroidGame;

public static class Extensions
{
    public static int AbsoluteX(this uint x)
    {
        return (int)(x * Game1.Scale + (Game1.CurrentWidth - Game1.CountedWidth)/2);
    }
    
    public static int AbsoluteY(this uint y)
    {
        return (int)(y * Game1.Scale + (Game1.CurrentHeight - Game1.CountedHeight)/2);
    } 
    
    public static int AbsoluteX(this int x)
    {
        return (int)(x * Game1.Scale + (Game1.CurrentWidth - Game1.CountedWidth)/2);
    }
    
    public static int AbsoluteY(this int y)
    {
        return (int)(y * Game1.Scale + (Game1.CurrentHeight - Game1.CountedHeight)/2);
    } 
}