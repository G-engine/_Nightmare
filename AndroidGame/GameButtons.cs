using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;

namespace AndroidGame;

public class Button
{
    private uint X;
    private uint Y;
    public bool IsPressed;
    public bool IsReleased;
    private int Id;
    private TouchCollection Touches;
    
    public Texture2D TextureButton;
    public Texture2D TextureButtonPressed;
    public Button(uint x, uint y)
    {
        X = x;
        Y = y;
        IsPressed = false;
        IsReleased = false;
    }
    public void Process(SpriteBatch spriteBatch)
    {
        Touches = TouchPanel.GetState();

        if (!IsPressed)
            spriteBatch.Draw(TextureButton, new Vector2(X.AbsoluteX(), Y.AbsoluteY()),
                new Rectangle(0,0, TextureButton.Width, TextureButton.Height), Color.White, 
                0, Vector2.Zero, Game1.Scale, SpriteEffects.None, 0);

        if (Touches.Count >= 1) 
            foreach (var touch in Touches) 
            { 
                if ((touch.Position.X >= X.AbsoluteX()) &&
                    (touch.Position.X <= X.AbsoluteX() + TextureButton.Width*Game1.Scale) && 
                    (touch.Position.Y >= Y.AbsoluteY()) &&
                    (touch.Position.Y <= Y.AbsoluteY() + TextureButton.Height*Game1.Scale))
                { 
                    Id = touch.Id; 
                    IsPressed = true; 
                    spriteBatch.Draw(TextureButtonPressed, new Vector2(X.AbsoluteX(), Y.AbsoluteY()),
                        new Rectangle(0,0, TextureButtonPressed.Width, TextureButtonPressed.Height), Color.White, 
                        0, Vector2.Zero, Game1.Scale, SpriteEffects.None, 0);
                }
            }
        
        if (!Touches.FindById(Id, out TouchLocation touchlocation) || (touchlocation.Position.X < X.AbsoluteX()) ||
            (touchlocation.Position.X > X.AbsoluteX() + TextureButton.Width*Game1.Scale) ||
            (touchlocation.Position.Y < Y.AbsoluteY()) ||
            (touchlocation.Position.Y > Y.AbsoluteY() + TextureButton.Height*Game1.Scale))
        { 
            IsPressed = false;
            IsReleased = true;
        }
    }
}