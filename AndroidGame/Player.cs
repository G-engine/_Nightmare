using System;
using Microsoft.Xna.Framework.Graphics;

namespace AndroidGame;
public class Player
{
    public uint X;
    public uint Y;
    public int Direction; //0 - влево, 1 - вправо, 2 - вверх, 3 -вниз
    public int Speed;
    public int Lives;
    public bool Moving;
    public int Pos;
    public uint Floor;
    public uint Room;
    
    public Texture2D TextureStatic;
    public Texture2D TextureGoing1;
    public Texture2D TextureGoing2;
    public Texture2D TextureGoing3;
    public Texture2D TextureGoing4;
    public Texture2D TextureGoing5;
    public Texture2D TextureGoing6;
    public Texture2D TextureStaticRev;
    public Texture2D TextureGoing1Rev;
    public Texture2D TextureGoing2Rev;
    public Texture2D TextureGoing3Rev;
    public Texture2D TextureGoing4Rev;
    public Texture2D TextureGoing5Rev;
    public Texture2D TextureGoing6Rev;
    public Texture2D TextureAnimation1;
    public Texture2D TextureAnimation2;
    public Texture2D TextureAnimation3;
    public Texture2D TextureAnimation1rev;
    public Texture2D TextureAnimation2rev;
    public Texture2D TextureAnimation3rev;
    public Texture2D TextureJump1rev;
    public Texture2D TextureJump2rev;
    public Texture2D TextureLadder2;
    public Texture2D TextureLadder3;
    public Texture2D TextureLife;
    
    public Player(uint x, uint y)
    {
        X = x;
        Y = y;
        Speed = 10;
        Lives = 3;
        Moving = false;
        Pos = 0;
        Direction = 0;
        Floor = 2;
        Room = 1;
    }
    public void PosReset()
    {
        Moving = false;
        Pos = 6;
    }
    
    public void TakeItem(ref Item item)
    {
        if (Floor == item.Floor & Room == item.Room & Math.Abs(X + 30 - item.X) <= 150)
        {
            item.IsVisible = true;
        }
    }

    public void ResetParams(uint x, uint y)
    {
        X = x;
        Y = y;
        Speed = 10;
        Lives = 3;
        Moving = false;
        Pos = 0;
        Direction = 0;
        Floor = 2;
        Room = 1;
    }
}

