using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
namespace AndroidGame;

public class GameProcess
{
    public int X; 
    public int Y; 
    public int Message; 
    public bool IsMessageShowing; 
    public Texture2D Map; 
    public Texture2D Menu; 
    public Texture2D MenuWin;
    public Texture2D MenuLose;
    public Texture2D MenuRules;
    public Texture2D ItemBack;
    public Texture2D Blur;
    public SoundEffect AudioSteps1;
    public SoundEffect AudioSteps2;
    public SoundEffect AudioButton;
    public SoundEffect AudioAttack;
    public SoundEffect AudioDeath;
    public SoundEffect AudioLadder;
    public SoundEffectInstance AudioLadderInst;
    public SoundEffect AudioDoorClosed;
    public SoundEffect AudioDoorOpened;

    public bool IsWin; 
    public bool IsLose; 
    public bool IsGame; 
    public bool IsPause;
    public bool IsManual;
    public bool IsRules;
    public enum AnimationType 
    { 
        Climb, 
        Ladder, 
        Door, 
        None
    }
    public GameProcess(int x, int y) 
    { 
        X = x; 
        Y = y; 
        Message = 0; 
        IsMessageShowing = false; 
        IsWin = false; 
        IsLose = false; 
        IsGame = false;
        IsPause = true;
        IsRules = false;
        IsManual = false;
    }
    
    public string ShowMessage() 
    { 
        switch (Message) 
        { 
            case 1: 
                return "You need a key to open this door";
            case 2:
                return "This key doesn't fit here";
        } 
        return "Something wrong";
    }

    public void Pause()
    {
        IsManual = true;
        IsPause = true;
        IsGame = false;
    }

    public void Continue()
    {
        IsPause = false;
        IsGame = true;
        IsManual = false;
    }
    public void NewGame(int x, int y) 
    { 
        X = x; 
        Y = y; 
        Message = 0; 
        IsMessageShowing = false; 
        IsPause = false;
        IsWin = false; 
        IsLose = false;
        IsGame = true;
        IsRules = false;
        IsManual = false;
    }

    public void WinGame() 
    { 
        IsPause = true; 
        IsWin = true; 
        IsLose = false; 
        IsGame = false;
        IsRules = false;
        IsManual = false;
    }
    public void LoseGame() 
    { 
        IsPause = true; 
        IsWin = false; 
        IsLose = true; 
        IsGame = false;
        IsRules = false;
        IsManual = false;
    }
}
