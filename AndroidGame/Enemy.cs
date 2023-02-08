using System;
using Microsoft.Xna.Framework.Graphics;

namespace AndroidGame;


public class Enemy
{
    public int X;
    public int Y;
    public int Direction; //0 - влево, 1 - вправо, 2 - вверх, 3 - вниз
    public int Speed;
    public uint Floor;
    public uint Room;
    public int Pos;
    public bool InPursuit;
    public bool InAttack;
    public Random Rnd;
    public bool Animation;
    
    public Texture2D TextureStatic;
    public Texture2D TextureStaticRev;
    
    public Texture2D TextureAnimation1;
    public Texture2D TextureAnimation2;
    public Texture2D TextureAnimation3;
    public Texture2D TextureAnimation4;
    public Texture2D TextureAnimation5;
    public Texture2D TextureAnimation6;
    public Texture2D TextureAnimation7;
    public Texture2D TextureAnimation8;
    public Texture2D TextureAnimation9;
    public Texture2D TextureAnimation10;
    public Texture2D TextureAnimation1rev;
    public Texture2D TextureAnimation2rev;
    public Texture2D TextureAnimation3rev;
    public Texture2D TextureAnimation4rev;
    public Texture2D TextureAnimation5rev;
    public Texture2D TextureAnimation6rev;
    public Texture2D TextureAnimation7rev;
    public Texture2D TextureAnimation8rev;
    public Texture2D TextureAnimation9rev;
    public Texture2D TextureAnimation10rev;
    
    public Texture2D TextureAttack1;
    public Texture2D TextureAttack2;
    public Texture2D TextureAttack3;
    public Texture2D TextureAttack4;
    public Texture2D TextureAttack1rev;
    public Texture2D TextureAttack2rev;
    public Texture2D TextureAttack3rev;
    public Texture2D TextureAttack4rev;
    

    public Enemy(int x, int y, uint floor)
    {
        X = x;
        Y = y;
        Floor = floor;
        Room = 1;
        Direction = 0;
        Pos = 0;
        Speed = 11;
        InPursuit = false;
        InAttack = false;
        Animation = false;
        Rnd = new Random();
    }

    public void ResetParams(int x, int y, uint floor)
    {
        X = x;
        Y = y;
        Floor = floor;
        Room = 1;
        Direction = 0;
        Pos = 0;
        Speed = 11;
        InPursuit = false;
        InAttack = false;
        Animation = false;
    }
}