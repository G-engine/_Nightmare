using System;
using System.Threading;
using Android.Content.Res;
using Android.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using Color = Microsoft.Xna.Framework.Color;
using Double = Java.Lang.Double;

namespace AndroidGame;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private GameProcess MainGameProcess;
    private Player MainPlayer;
    private Enemy Ghost0;
    private Enemy Ghost1;
    private Button ButtonMoveLeft;
    private Button ButtonMoveRight;
    private Button ButtonLadder;
    private Button ButtonPick;
    private Button ButtonGo;
    private Button ButtonStart;
    private Button ButtonRules;
    private Button ButtonBack;
    private Button ButtonPause;
    private Item Key1;
    private Item Key2;
    private SpriteFont MessageFont;

    private double Tick;
    private double PreviousTick;
    private double AnimTick;
    private double GhostTick;
    private double GhostPreviousTick;
    private double GhostAttackTick;
    private double GhostAttackPreviousTick;
    private double RndTime0;
    private double Time;

    private GameProcess.AnimationType animationType;

    public static double CurrentWidth;
    public static double CurrentHeight;
    private const int NominalWidth = 1920;
    private const int NominalHeight = 1080;
    
    //приближенные размеры рабочей области формата 16:9 к разрешению устройства 
    public static double CountedWidth;
    public static double CountedHeight;
    public static float Scale;
    
    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        
        var metric = new Android.Util.DisplayMetrics();
        Activity.WindowManager.DefaultDisplay.GetMetrics(metric);
        
        CurrentWidth = metric.WidthPixels;
        CurrentHeight = metric.HeightPixels;
        
        _graphics.IsFullScreen = true;
        _graphics.PreferredBackBufferWidth = (int)CurrentWidth;
        _graphics.PreferredBackBufferHeight = (int)CurrentHeight;
        _graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;

        UpdateScreenParams();
    }

    protected override void Initialize()
    {
        MainGameProcess = new GameProcess(-1170, -1100);
        MainPlayer = new Player(250, 420);
        Ghost0 = new Enemy(200,3950, 0);
        Ghost1 = new Enemy(200,2600, 1);
        ButtonMoveLeft = new Button(50, 750);
        ButtonMoveRight = new Button(350, 750);
        ButtonLadder = new Button(1025, 750);
        ButtonPick = new Button(1325, 750);
        ButtonGo = new Button(1625, 750);
        ButtonStart = new Button(700, 440);
        ButtonRules = new Button(700, 740);
        ButtonBack = new Button(70, 70);
        ButtonPause = new Button(1750, 40);
        Key1 = new Item(430, 470, 3, 2);
        Key2 = new Item(300, 880, 0, 1);

        animationType = GameProcess.AnimationType.None;

        base.Initialize();
    }
    private void UpdateScreenParams()
    {
        double dx = CurrentWidth / NominalWidth * 1.0;
        double dy = CurrentHeight / NominalHeight * 1.0;
        
        Scale = (float)Double.Min(dx, dy);
        
        CountedHeight = NominalHeight * Scale;
        CountedWidth = NominalWidth * Scale;
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        
        MainPlayer.TextureStatic = Content.Load<Texture2D>("player_staying");
        MainPlayer.TextureGoing1 = Content.Load<Texture2D>("player_going1");
        MainPlayer.TextureGoing2 = Content.Load<Texture2D>("player_going2");
        MainPlayer.TextureGoing3 = Content.Load<Texture2D>("player_going3");
        MainPlayer.TextureGoing4 = Content.Load<Texture2D>("player_going4");
        MainPlayer.TextureGoing5 = Content.Load<Texture2D>("player_going5");
        MainPlayer.TextureGoing6 = Content.Load<Texture2D>("player_going6");
        MainPlayer.TextureStaticRev = Content.Load<Texture2D>("player_staying_rev");
        MainPlayer.TextureGoing1Rev = Content.Load<Texture2D>("player_going1rev");
        MainPlayer.TextureGoing2Rev = Content.Load<Texture2D>("player_going2rev");
        MainPlayer.TextureGoing3Rev = Content.Load<Texture2D>("player_going3rev");
        MainPlayer.TextureGoing4Rev = Content.Load<Texture2D>("player_going4rev");
        MainPlayer.TextureGoing5Rev = Content.Load<Texture2D>("player_going5rev");
        MainPlayer.TextureGoing6Rev = Content.Load<Texture2D>("player_going6rev");
        MainPlayer.TextureAnimation1 = Content.Load<Texture2D>("player_anim_1");
        MainPlayer.TextureAnimation2 = Content.Load<Texture2D>("player_anim_2");
        MainPlayer.TextureAnimation3 = Content.Load<Texture2D>("player_anim_3");
        MainPlayer.TextureAnimation1rev = Content.Load<Texture2D>("player_anim_1rev");
        MainPlayer.TextureAnimation2rev = Content.Load<Texture2D>("player_anim_2rev");
        MainPlayer.TextureAnimation3rev = Content.Load<Texture2D>("player_anim_3rev");
        MainPlayer.TextureJump1rev = Content.Load<Texture2D>("player_jump1rev");
        MainPlayer.TextureJump2rev = Content.Load<Texture2D>("player_jump2rev");
        MainPlayer.TextureLadder2 = Content.Load<Texture2D>("player_ladder2");
        MainPlayer.TextureLadder3 = Content.Load<Texture2D>("player_ladder3");
        MainPlayer.TextureLife = Content.Load<Texture2D>("life");
        
        Ghost0.TextureStatic = Content.Load<Texture2D>("ghost_static");
        Ghost0.TextureStaticRev = Content.Load<Texture2D>("ghost_static_rev");
        Ghost0.TextureAttack1 = Content.Load<Texture2D>("ghost_attack1");
        Ghost0.TextureAttack2 = Content.Load<Texture2D>("ghost_attack2");
        Ghost0.TextureAttack3 = Content.Load<Texture2D>("ghost_attack3");
        Ghost0.TextureAttack4 = Content.Load<Texture2D>("ghost_attack4");
        Ghost0.TextureAttack1rev = Content.Load<Texture2D>("ghost_attack1_rev");
        Ghost0.TextureAttack2rev = Content.Load<Texture2D>("ghost_attack2_rev");
        Ghost0.TextureAttack3rev = Content.Load<Texture2D>("ghost_attack3_rev");
        Ghost0.TextureAttack4rev = Content.Load<Texture2D>("ghost_attack4_rev");
        
        Ghost1.TextureStatic = Content.Load<Texture2D>("ghost_static");
        Ghost1.TextureStaticRev = Content.Load<Texture2D>("ghost_static_rev");
        Ghost1.TextureAnimation1 = Content.Load<Texture2D>("ghost_anim1");
        Ghost1.TextureAnimation2 = Content.Load<Texture2D>("ghost_anim2");
        Ghost1.TextureAnimation3 = Content.Load<Texture2D>("ghost_anim3");
        Ghost1.TextureAnimation4 = Content.Load<Texture2D>("ghost_anim4");
        Ghost1.TextureAnimation5 = Content.Load<Texture2D>("ghost_anim5");
        Ghost1.TextureAnimation6 = Content.Load<Texture2D>("ghost_anim6");
        Ghost1.TextureAnimation7 = Content.Load<Texture2D>("ghost_anim7");
        Ghost1.TextureAnimation8 = Content.Load<Texture2D>("ghost_anim8");
        Ghost1.TextureAnimation9 = Content.Load<Texture2D>("ghost_anim9");
        Ghost1.TextureAnimation10 = Content.Load<Texture2D>("ghost_anim10");
        Ghost1.TextureAnimation1rev = Content.Load<Texture2D>("ghost_anim1_rev");
        Ghost1.TextureAnimation2rev = Content.Load<Texture2D>("ghost_anim2_rev");
        Ghost1.TextureAnimation3rev = Content.Load<Texture2D>("ghost_anim3_rev");
        Ghost1.TextureAnimation4rev = Content.Load<Texture2D>("ghost_anim4_rev");
        Ghost1.TextureAnimation5rev = Content.Load<Texture2D>("ghost_anim5_rev");
        Ghost1.TextureAnimation6rev = Content.Load<Texture2D>("ghost_anim6_rev");
        Ghost1.TextureAnimation7rev = Content.Load<Texture2D>("ghost_anim7_rev");
        Ghost1.TextureAnimation8rev = Content.Load<Texture2D>("ghost_anim8_rev");
        Ghost1.TextureAnimation9rev = Content.Load<Texture2D>("ghost_anim9_rev");
        Ghost1.TextureAnimation10rev = Content.Load<Texture2D>("ghost_anim10_rev");
        Ghost1.TextureAttack1 = Content.Load<Texture2D>("ghost_attack1");
        Ghost1.TextureAttack2 = Content.Load<Texture2D>("ghost_attack2");
        Ghost1.TextureAttack3 = Content.Load<Texture2D>("ghost_attack3");
        Ghost1.TextureAttack4 = Content.Load<Texture2D>("ghost_attack4");
        Ghost1.TextureAttack1rev = Content.Load<Texture2D>("ghost_attack1_rev");
        Ghost1.TextureAttack2rev = Content.Load<Texture2D>("ghost_attack2_rev");
        Ghost1.TextureAttack3rev = Content.Load<Texture2D>("ghost_attack3_rev");
        Ghost1.TextureAttack4rev = Content.Load<Texture2D>("ghost_attack4_rev");
        
        ButtonMoveLeft.TextureButton = Content.Load<Texture2D>("button_left");
        ButtonMoveLeft.TextureButtonPressed = Content.Load<Texture2D>("button_left_pressed");
        ButtonMoveRight.TextureButton = Content.Load<Texture2D>("button_right");
        ButtonMoveRight.TextureButtonPressed = Content.Load<Texture2D>("button_right_pressed");
        ButtonLadder.TextureButton = Content.Load<Texture2D>("button_ladder");
        ButtonLadder.TextureButtonPressed = Content.Load<Texture2D>("button_ladder_pressed");
        ButtonPick.TextureButton = Content.Load<Texture2D>("button_pick");
        ButtonPick.TextureButtonPressed = Content.Load<Texture2D>("button_pick_pressed");
        ButtonGo.TextureButton = Content.Load<Texture2D>("button_go");
        ButtonGo.TextureButtonPressed = Content.Load<Texture2D>("button_go_pressed");
        ButtonStart.TextureButton = Content.Load<Texture2D>("button_start");
        ButtonStart.TextureButtonPressed = Content.Load<Texture2D>("button_start");
        ButtonRules.TextureButton = Content.Load<Texture2D>("button_rules");
        ButtonRules.TextureButtonPressed = Content.Load<Texture2D>("button_rules");
        ButtonBack.TextureButton = Content.Load<Texture2D>("button_back");
        ButtonBack.TextureButtonPressed = Content.Load<Texture2D>("button_back");
        ButtonPause.TextureButton = Content.Load<Texture2D>("button_pause");
        ButtonPause.TextureButtonPressed = Content.Load<Texture2D>("button_pause");

        MainGameProcess.Map = Content.Load<Texture2D>("map");
        MainGameProcess.Menu = Content.Load<Texture2D>("main_menu");
        MainGameProcess.MenuWin = Content.Load<Texture2D>("main_menu_win");
        MainGameProcess.MenuLose = Content.Load<Texture2D>("main_menu_lose");
        MainGameProcess.MenuRules = Content.Load<Texture2D>("menu_rules");
        MainGameProcess.ItemBack = Content.Load<Texture2D>("item_background");
        MainGameProcess.Blur = Content.Load<Texture2D>("blur");
        MainGameProcess.AudioSteps1 = Content.Load<SoundEffect>("audio_steps1");
        MainGameProcess.AudioSteps2 = Content.Load<SoundEffect>("audio_steps2");
        MainGameProcess.AudioButton = Content.Load<SoundEffect>("audio_button");
        MainGameProcess.AudioAttack = Content.Load<SoundEffect>("audio_attack");
        MainGameProcess.AudioDeath = Content.Load<SoundEffect>("audio_death");
        MainGameProcess.AudioLadder = Content.Load<SoundEffect>("audio_ladder");
        MainGameProcess.AudioLadderInst = MainGameProcess.AudioLadder.CreateInstance();
        MainGameProcess.AudioDoorClosed = Content.Load<SoundEffect>("audio_door_closed");
        MainGameProcess.AudioDoorOpened = Content.Load<SoundEffect>("audio_door_opened");
        
        Key1.Texture = Content.Load<Texture2D>("key1");
        Key2.Texture = Content.Load<Texture2D>("key2");

        MessageFont = Content.Load<SpriteFont>("File");
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed) Exit();

        if (!MainGameProcess.IsPause)
        {
            //обработка кнопок
            if (MainGameProcess.IsGame)
            {
                if (ButtonMoveLeft.IsPressed)
                {
                    if (!MainPlayer.Moving)
                    {
                        MainPlayer.Moving = true;
                        MainPlayer.Pos = 0;
                    }
                    MainPlayer.Direction = 1;
                    if (MainPlayer.X <= 500 & MainPlayer.X >= 80)
                    {
                        switch (MainPlayer.Floor)
                        {
                            case 0:
                                if(MainGameProcess.X >= 0)
                                    MainPlayer.X -= (uint)MainPlayer.Speed;
                                else MainGameProcess.X += MainPlayer.Speed;
                                break;
                            case 1:
                                if (MainPlayer.Room == 1)
                                {
                                    if(MainGameProcess.X >= 0)
                                        MainPlayer.X -= (uint)MainPlayer.Speed;
                                    else MainGameProcess.X += MainPlayer.Speed;
                                }
                                if (MainPlayer.Room == 2)
                                {
                                    if(MainGameProcess.X >= -3020)
                                        MainPlayer.X -= (uint)MainPlayer.Speed;
                                    else MainGameProcess.X += MainPlayer.Speed;
                                }
                                break;
                            case 2:
                                if (MainPlayer.Room == 1)
                                {
                                    if(MainGameProcess.X >= 0)
                                        MainPlayer.X -= (uint)MainPlayer.Speed;
                                    else MainGameProcess.X += MainPlayer.Speed;
                                }
                                if (MainPlayer.Room == 2)
                                {
                                    if(MainGameProcess.X >= -3020)
                                        MainPlayer.X -= (uint)MainPlayer.Speed;
                                    else MainGameProcess.X += MainPlayer.Speed;
                                }
                                break;
                            case 3:
                                MainPlayer.X -= (uint)MainPlayer.Speed;
                                break;
                                
                        }
                    }
                    else if (MainPlayer.X < 80){}
                    else MainPlayer.X -= (uint)MainPlayer.Speed;
                }
                if (ButtonMoveRight.IsPressed)
                {
                    if (!MainPlayer.Moving)
                    {
                        MainPlayer.Moving = true;
                        MainPlayer.Pos = 0;
                    }
                    MainPlayer.Direction = 0;
                    if (MainPlayer.X + MainPlayer.TextureStatic.Width >= 1420 & MainPlayer.X + MainPlayer.TextureStatic.Width <= 1830)
                    {
                        switch (MainPlayer.Floor)
                        {
                            case 0:
                                if(MainGameProcess.X <= -4850)
                                    MainPlayer.X += (uint)MainPlayer.Speed;
                                else MainGameProcess.X -= MainPlayer.Speed;
                                break;
                            
                            case 1:
                                if (MainPlayer.Room == 1)
                                {
                                    if(MainGameProcess.X <= -1170)
                                        MainPlayer.X += (uint)MainPlayer.Speed;
                                    else MainGameProcess.X -= MainPlayer.Speed;
                                }
                                
                                if (MainPlayer.Room == 2)
                                {
                                    if(MainGameProcess.X <= -4850)
                                        MainPlayer.X += (uint)MainPlayer.Speed;
                                    else MainGameProcess.X -= MainPlayer.Speed;
                                }
                                break;
                            
                            case 2:
                                if (MainPlayer.Room == 1)
                                {
                                    if(MainGameProcess.X <= -1170)
                                        MainPlayer.X += (uint)MainPlayer.Speed;
                                    else MainGameProcess.X -= MainPlayer.Speed;
                                }
                                
                                if (MainPlayer.Room == 2)
                                {
                                    if(MainGameProcess.X <= -3570)
                                        MainPlayer.X += (uint)MainPlayer.Speed;
                                    else MainGameProcess.X -= MainPlayer.Speed;
                                }
                                break;
                            case 3:
                                MainPlayer.X += (uint)MainPlayer.Speed;
                                break;
                        }
                    }
                    else if(MainPlayer.X + MainPlayer.TextureStatic.Width > 1830){}
                    else MainPlayer.X += (uint)MainPlayer.Speed;
                }
                if ((!ButtonMoveLeft.IsPressed && !ButtonMoveRight.IsPressed) || (ButtonMoveLeft.IsPressed && ButtonMoveRight.IsPressed))
                {
                    if (MainPlayer.Moving)
                    {
                        MainPlayer.Moving = false;
                        MainGameProcess.AudioSteps2.Play();
                    }
                }
                if (ButtonLadder.IsPressed)
                {
                    if (ButtonLadder.IsReleased)
                    {
                        ButtonLadder.IsReleased = false;
                        if ((MainPlayer.Floor == 1 | MainPlayer.Floor == 2) & ((MainGameProcess.X <= -1160 & MainGameProcess.X >= -1280 & MainPlayer.X <= 1600 & MainPlayer.X >= 1500)
                                                                               || (MainGameProcess.X >= -3020 & MainGameProcess.X <= -2900 & MainPlayer.X >= 245 & MainPlayer.X <= 360))
                                                                               || (MainGameProcess.X >= -50 & MainGameProcess.X <= 10 & MainPlayer.X >= 90 & MainPlayer.X <= 180))
                        {
                            AnimTick = Tick;
                            MainGameProcess.IsGame = false;
                            animationType = GameProcess.AnimationType.Ladder;
                            MainPlayer.Pos = 0;
                        }
                        
                        else if (MainPlayer.Floor == 2 & ((MainGameProcess.X >= -3020 & MainGameProcess.X <= -2900 & MainPlayer.X >= 650 & MainPlayer.X <= 800)
                                     || (MainGameProcess.X >= -3320 & MainGameProcess.X <= -3020 & MainPlayer.X >= 500 & MainPlayer.X <= 700)
                                     || (MainGameProcess.X >= -3420 & MainGameProcess.X <= -3320 & MainPlayer.X >= 300 & MainPlayer.X <= 430)))
                        {
                            AnimTick = Tick;
                            MainGameProcess.IsGame = false;
                            animationType = GameProcess.AnimationType.Ladder;
                            MainPlayer.Pos = 0;
                            MainPlayer.Direction = 2;
                            MainPlayer.Floor = 3;
                        }
                        
                        else if (MainPlayer.Floor == 3 & MainGameProcess.X <= -3340 & MainPlayer.X >= 300 & MainPlayer.X <= 430)
                        {
                            AnimTick = Tick;
                            MainGameProcess.IsGame = false;
                            animationType = GameProcess.AnimationType.Ladder;
                            MainPlayer.Pos = 0;
                            MainPlayer.Direction = 3;
                        }
                    }
                }
                if (ButtonGo.IsPressed)
                {
                    if (ButtonGo.IsReleased)
                    {
                        ButtonGo.IsReleased = false;
                        if (MainPlayer.Floor == 0 & ((MainGameProcess.X >= -200 & MainGameProcess.X <= 0 & MainPlayer.X >= 1000 & MainPlayer.X <= 1150)
                                                     || (MainGameProcess.X >= -500 & MainGameProcess.X <= -200 & MainPlayer.X >= 750 & MainPlayer.X <= 1000)
                                                     || (MainGameProcess.X >= -900 & MainGameProcess.X <= -500 & MainPlayer.X >= 500 & MainPlayer.X <= 700)
                                                     || (MainGameProcess.X >= -4500 & MainGameProcess.X <= -4100 & MainPlayer.X >= 800 & MainPlayer.X <= 1150)
                                                     || (MainGameProcess.X >= -4700 & MainGameProcess.X <= -4500 & MainPlayer.X >= 750 & MainPlayer.X <= 1000)
                                                     || (MainGameProcess.X >= -4850 & MainGameProcess.X <= -4700 & MainPlayer.X >= 500 & MainPlayer.X <= 750)))
                        {
                            MainGameProcess.AudioDoorOpened.Play();
                            animationType = GameProcess.AnimationType.Door;
                            AnimTick = Tick;
                            MainGameProcess.IsGame = false;
                            MainPlayer.Pos = 0;
                        }
                        else if (MainPlayer.Floor == 1 & ((MainGameProcess.X >= -600 & MainGameProcess.X <= -150 & MainPlayer.X >= 800 & MainPlayer.X <= 1150)
                                                          || (MainGameProcess.X >= -800 & MainGameProcess.X <= -600 & MainPlayer.X >= 800 & MainPlayer.X <= 950)
                                                          || (MainGameProcess.X >= -1170 & MainGameProcess.X <= -800 & MainPlayer.X >= 500 & MainPlayer.X <= 600)))
                        {
                            if (Key1.IsVisible)
                            {
                                MainGameProcess.AudioDoorOpened.Play();
                                animationType = GameProcess.AnimationType.Door;
                                AnimTick = Tick;
                                MainGameProcess.IsGame = false;
                                MainPlayer.Pos = 0;
                            }
                            else
                            {
                                MainGameProcess.AudioDoorClosed.Play();
                                MainGameProcess.Message = 1;
                                MainGameProcess.IsMessageShowing = true;
                            }
                        }
                        else if (MainPlayer.Floor == 1 & ((MainGameProcess.X <= -3050 & MainGameProcess.X >= -3400 & MainPlayer.X >= 800 & MainPlayer.X <= 1150)
                                                           || (MainGameProcess.X <= -3400 & MainGameProcess.X >= -3700 & MainPlayer.X >= 700 & MainPlayer.X <= 950)
                                                           || (MainGameProcess.X <= -3700 & MainGameProcess.X >= -4000 & MainPlayer.X >= 400 & MainPlayer.X <= 750)))
                        {
                            if (Key2.IsVisible)
                            {
                                MainGameProcess.WinGame();
                            }
                            else if (Key1.IsVisible)
                            {
                                MainGameProcess.AudioDoorClosed.Play();
                                MainGameProcess.Message = 2;
                                MainGameProcess.IsMessageShowing = true;
                            }
                            else
                            {
                                MainGameProcess.AudioDoorClosed.Play();
                                MainGameProcess.Message = 1;
                                MainGameProcess.IsMessageShowing = true;
                            }
                                
                        }
                        else if (MainPlayer.Floor == 2 & ((MainGameProcess.X <= -1140 & MainGameProcess.X >= -1280 & MainPlayer.X <= 1800 & MainPlayer.X >= 1500)
                                                          || (MainGameProcess.X >= -3020 & MainGameProcess.X <= -2900 & MainPlayer.X >= 70 & MainPlayer.X <= 220)))
                        {
                            animationType = GameProcess.AnimationType.Climb;
                            AnimTick = Tick;
                            MainGameProcess.IsGame = false;
                            MainPlayer.Pos = 0;
                        }
                    }
                }
                if (ButtonPick.IsPressed)
                {
                    if (ButtonPick.IsReleased)
                    {
                        ButtonPick.IsReleased = false;
                        MainPlayer.TakeItem(ref Key1);
                        MainPlayer.TakeItem(ref Key2);
                    }
                }
                if (ButtonPause.IsPressed)
                {
                    if (ButtonPause.IsReleased)
                    {
                        MainGameProcess.AudioButton.Play();
                        ButtonPause.IsReleased = false;
                        MainGameProcess.Pause();
                    }
                }
            }
            //анимации
            else
            {
                if (animationType == GameProcess.AnimationType.Climb) //проход на 2 этаже
                {
                    if (MainPlayer.Room == 1) //во вторую комнату (спуск)
                    {
                        if (Tick - AnimTick >= 1.3)
                        {
                            MainGameProcess.IsGame = true;
                            animationType = GameProcess.AnimationType.None;
                            MainGameProcess.Y = -1395; //калибровка координат
                            MainPlayer.Y = 420;
                            MainPlayer.Room = 2;
                        }
                        else if (Tick - AnimTick < 0.8)
                            MainGameProcess.X -= MainPlayer.Speed - 2;
                        else if (Tick - AnimTick >= 0.8)
                        {
                            MainGameProcess.X -= MainPlayer.Speed - 4;
                            MainGameProcess.Y -= MainPlayer.Speed;
                        }

                        if (MainGameProcess.X >= -2920) //перевод камеры
                        {
                            MainGameProcess.X -= MainPlayer.Speed*2;
                            MainPlayer.X -= (uint)MainPlayer.Speed*2;
                        }
                    }
                    else //в первую комнату (прыжок)
                    {
                        if (Tick - AnimTick >= 1.3)
                        {
                            MainGameProcess.IsGame = true;
                            animationType = GameProcess.AnimationType.None;
                            MainGameProcess.Y = -1100; //калибровка координат
                            MainPlayer.Y = 420;
                            MainPlayer.Room = 1;
                        }
                        else if (Tick - AnimTick < 0.04)
                        {
                            MainGameProcess.Y += MainPlayer.Speed + 12;
                            MainGameProcess.X += MainPlayer.Speed/2;
                        }
                        else if (Tick - AnimTick >= 0.04 & Tick - AnimTick < 0.15)
                        {
                            MainGameProcess.Y += MainPlayer.Speed + 12;
                            MainGameProcess.X += MainPlayer.Speed/2;
                        }
                        else if (Tick - AnimTick >= 0.15 & Tick - AnimTick < 0.3)
                        {
                            MainGameProcess.Y += MainPlayer.Speed + 2;
                            MainGameProcess.X += MainPlayer.Speed/2;
                        }
                        else if (Tick - AnimTick >= 0.3 & Tick - AnimTick < 0.5)
                        {
                            MainGameProcess.Y += MainPlayer.Speed - 8;
                            MainGameProcess.X += MainPlayer.Speed/2;
                        }
                        else if (Tick - AnimTick >= 0.5)
                            MainGameProcess.X += MainPlayer.Speed - 4;

                        if (MainGameProcess.X <= -1220) //перевод камеры
                        {
                            MainGameProcess.X += MainPlayer.Speed*2;
                            MainPlayer.X += (uint)MainPlayer.Speed*2;
                        }
                    }
                }
                else if (animationType == GameProcess.AnimationType.Ladder) //спуск/подъем по лестнице
                {
                    if(MainPlayer.Pos == 0)
                        MainGameProcess.AudioLadderInst.Play();
                    switch (MainPlayer.Floor)
                    {
                        case 1:
                            if (MainPlayer.Room == 1)
                            {
                                if (MainGameProcess.Y <= -1100)
                                    MainGameProcess.Y += MainPlayer.Speed;
                                else
                                {
                                    MainGameProcess.AudioLadderInst.Stop();
                                    MainGameProcess.IsGame = true;
                                    animationType = GameProcess.AnimationType.None;
                                    MainGameProcess.Y = -1100;
                                    MainPlayer.Y = 420;
                                    MainPlayer.Floor = 2;
                                }
                            }
                            else if (MainPlayer.Room == 2)
                            {
                                if (MainGameProcess.Y <= -1400)
                                    MainGameProcess.Y += MainPlayer.Speed;
                                else
                                {
                                    MainGameProcess.AudioLadderInst.Stop();
                                    MainGameProcess.IsGame = true;
                                    animationType = GameProcess.AnimationType.None;
                                    MainGameProcess.Y = -1400;
                                    MainPlayer.Y = 420;
                                    MainPlayer.Floor = 2;
                                }
                            }
                            break;
                        case 2:
                            if (MainGameProcess.Y >= -2450)
                                MainGameProcess.Y -= MainPlayer.Speed;
                            else
                            {
                                MainGameProcess.AudioLadderInst.Stop();
                                MainGameProcess.IsGame = true;
                                animationType = GameProcess.AnimationType.None;
                                MainGameProcess.Y = -2450;
                                MainPlayer.Y = 420;
                                MainPlayer.Floor = 1;
                            }
                            break;
                        case 3:
                            if (MainPlayer.Direction == 2)
                            {
                                if (MainGameProcess.Y <= -595)
                                {
                                    MainGameProcess.Y += MainPlayer.Speed;
                                    if (MainGameProcess.X >= -3340)
                                    {
                                        MainPlayer.X -= (uint)MainPlayer.Speed / 2;
                                        MainGameProcess.X -= MainPlayer.Speed / 2;
                                    }
                                }
                                else
                                {
                                    MainGameProcess.AudioLadderInst.Stop();
                                    MainGameProcess.IsGame = true;
                                    animationType = GameProcess.AnimationType.None;
                                    MainPlayer.Y = 420;
                                    MainGameProcess.Y = -595;
                                    MainPlayer.Floor = 3;
                                    MainPlayer.Direction = 0;
                                }
                            }
                            else if (MainPlayer.Direction == 3)
                            {
                                if (MainGameProcess.Y >= -1400)
                                {
                                    MainGameProcess.Y -= MainPlayer.Speed;
                                }
                                else
                                {
                                    MainGameProcess.AudioLadderInst.Stop();
                                    MainGameProcess.IsGame = true;
                                    animationType = GameProcess.AnimationType.None;
                                    MainPlayer.Y = 420;
                                    MainGameProcess.Y = -1400;
                                    MainPlayer.Floor = 2;
                                    MainPlayer.Direction = 0;
                                }
                            }
                            break;
                    }
                }
                else if (animationType == GameProcess.AnimationType.Door)
                {
                    switch (MainPlayer.Floor)
                    {
                        case 0:
                            if (MainGameProcess.Y >= -2450)
                            {
                                MainGameProcess.IsGame = true;
                                animationType = GameProcess.AnimationType.None;
                                MainGameProcess.X = -400;
                                MainGameProcess.Y = -2450;
                                MainPlayer.X = 1100;
                                MainPlayer.Y = 420;
                                MainPlayer.Floor = 1;
                            }
                            else
                            {
                                MainGameProcess.Y += MainPlayer.Speed;
                                if(MainGameProcess.X < -400)
                                    MainGameProcess.X += MainPlayer.Speed * 4;
                                if(MainGameProcess.X > -400)
                                    MainGameProcess.X -= MainPlayer.Speed / 2;
                            }
                            break;
                        
                        case 1:
                            if (MainGameProcess.Y <= -3590)
                            {
                                MainGameProcess.IsGame = true;
                                animationType = GameProcess.AnimationType.None;
                                MainGameProcess.X = 0;
                                MainGameProcess.Y = -3590;
                                MainPlayer.X = 1100;
                                MainPlayer.Y = 558;
                                MainPlayer.Floor = 0;
                            }
                            else
                            {
                                MainGameProcess.Y -= MainPlayer.Speed;
                                if (MainGameProcess.X < 0)
                                    MainGameProcess.X += MainPlayer.Speed;
                            }
                            break;
                    }
                } //переход в подвал
            }
            
            #region Ghost Movement
            {
                GhostTick += gameTime.GetElapsedSeconds();
                GhostAttackTick += gameTime.GetElapsedSeconds();
                if (!Ghost0.InPursuit)
                {
                    RndTime0 += gameTime.GetElapsedSeconds();

                    if (RndTime0 >= Ghost0.Rnd.Next(2, 5)) // в период с 2 до 4 секунд рандомит направление
                    {
                        Ghost0.Direction = Ghost0.Rnd.Next(0, 2);
                        RndTime0 = 0;
                    }

                    if (Ghost0.Direction == 0)
                    {
                        Ghost0.X -= Ghost0.Speed;
                        if (Ghost0.X <= 100)
                        {
                            Ghost0.Direction = 1;
                            RndTime0 = 0;
                        }
                    }
                    else if (Ghost0.Direction == 1)
                    {
                        Ghost0.X += Ghost0.Speed;
                        if (Ghost0.X + Ghost0.TextureStatic.Width >= 6500)
                        {
                            Ghost0.Direction = 0;
                            RndTime0 = 0;
                        }
                    }

                    Ghost0.InPursuit = Ghost0.Floor == MainPlayer.Floor & animationType == GameProcess.AnimationType.None & Math.Abs(Ghost0.X - MainPlayer.X + MainGameProcess.X) <= 2000;
                }
                else if(Ghost0.InPursuit | Ghost0.InAttack)
                {
                    if (Ghost0.X >= MainPlayer.X - MainGameProcess.X)
                    {
                        Ghost0.X -= Ghost0.Speed;
                        Ghost0.Direction = 0;
                    }
                    else if (Ghost0.X <= MainPlayer.X - MainGameProcess.X)
                    {
                        Ghost0.X += Ghost0.Speed;
                        Ghost0.Direction = 1;
                    }
                    
                    if (Ghost0.Floor == MainPlayer.Floor & animationType == GameProcess.AnimationType.None &
                        (Ghost0.Direction == 0 & Math.Abs(Ghost0.X - MainPlayer.X + MainGameProcess.X) <= 800 |
                         Ghost0.Direction == 1 & Math.Abs(Ghost0.X - MainPlayer.X + MainGameProcess.X) <= 800 + MainPlayer.TextureStatic.Width * Scale))
                    {
                        Ghost0.InAttack = true;
                        if (Ghost0.Direction == 0 & Math.Abs(Ghost0.X - MainPlayer.X + MainGameProcess.X) <= 300 |
                            Ghost0.Direction == 1 & Math.Abs(Ghost0.X - MainPlayer.X + MainGameProcess.X) <= 300 + MainPlayer.TextureStatic.Width * Scale)
                        {
                            Thread.Sleep(400);
                            MainGameProcess.X = -3350;
                            MainGameProcess.Y = -595;
                            MainPlayer.X = 400;
                            MainPlayer.Y = 420;
                            MainPlayer.Floor = 3;
                            MainPlayer.Room = 2;
                            MainPlayer.Direction = 0;
                            MainPlayer.Lives--;
                            Ghost0.InAttack = false;
                            GhostAttackTick = 0;
                            GhostAttackPreviousTick = 0;
                            GhostPreviousTick = 0;
                        }
                    }
                    Ghost0.InPursuit = Ghost0.Floor == MainPlayer.Floor & animationType == GameProcess.AnimationType.None & Math.Abs(Ghost0.X - MainPlayer.X + MainGameProcess.X) <= 2000;
                }
                
                if (!Ghost1.Animation)
                {
                    GhostTick = 0;
                    if (!Ghost1.InPursuit)
                    {
                        switch (Ghost1.Floor)
                        {
                            case 1:
                                if (Ghost1.Direction == 0)
                                {
                                    Ghost1.X -= Ghost1.Speed;
                                    if ((Ghost1.X <= 100 && Ghost1.Room == 1) || (Ghost1.X <= 3100 && Ghost1.Room == 2))
                                    {
                                        Ghost1.Direction = Ghost1.Rnd.Next(1, 3); //вправо или вверх
                                    }
                                }
                                else if (Ghost1.Direction == 1)
                                {
                                    Ghost1.X += Ghost1.Speed;
                                    if (Ghost1.X + Ghost1.TextureStatic.Width >= 3000 && Ghost1.Room == 1)
                                    {
                                        Ghost1.Direction = Ghost1.Rnd.Next(0, 2); //влево или вверх
                                        if (Ghost1.Direction == 1)
                                            Ghost1.Direction = 2;
                                    }
                                    else if (Ghost1.X + Ghost1.TextureStatic.Width >= 6500 && Ghost1.Room == 2)
                                    {
                                        Ghost1.Direction = 0; //влево
                                    }
                                }
                                else if (Ghost1.Direction == 2)
                                {
                                    Ghost1.Y -= Ghost1.Speed;
                                    if (Ghost1.Room == 1 && Ghost1.Y <= 1320)
                                    {
                                        Ghost1.Y = 1320;
                                        Ghost1.Floor = 2;
                                        Ghost1.Direction = 1;
                                    }
                                    else if (Ghost1.Room == 2 && Ghost1.Y <= 1620)
                                    {
                                        Ghost1.Y = 1620;
                                        Ghost1.Floor = 2;
                                        Ghost1.Direction = 0;
                                    }
                                }
                                break;
                            case 2:
                                if (Ghost1.Direction == 0)
                                {
                                    Ghost1.X -= Ghost1.Speed;
                                    if (Ghost1.X <= 100 && Ghost1.Room == 1)
                                    {
                                        Ghost1.Direction = Ghost1.Rnd.Next(1, 3); //вправо или вниз
                                        if (Ghost1.Direction == 2) Ghost1.Direction = 3; 
                                    }
                                    else if (Ghost1.X <= 3100 && Ghost1.Room == 2)
                                    {
                                        Ghost1.Direction = Ghost1.Rnd.Next(1, 4); //вправо или вниз или анимация влево
                                        if (Ghost1.Direction == 2) Ghost1.Animation = true;
                                    }
                                } 
                                else if (Ghost1.Direction == 1)
                                {
                                    Ghost1.X += Ghost1.Speed;
                                    if (Ghost1.X + Ghost1.TextureStatic.Width >= 3000 && Ghost1.Room == 1)
                                    {
                                        Ghost1.Direction = Ghost1.Rnd.Next(0, 3); //влево или вниз или анимация вправо
                                        if (Ghost1.Direction == 1) Ghost1.Direction = 3;
                                        if (Ghost1.Direction == 2) Ghost1.Animation = true;
                                    }
                                    else if (Ghost1.X + Ghost1.TextureStatic.Width >= 5400 && Ghost1.Room == 2)
                                    {
                                        Ghost1.Direction = 0; //влево
                                    }
                                } 
                                else if (Ghost1.Direction == 3)
                                {
                                    Ghost1.Y += Ghost1.Speed;
                                    if (Ghost1.Y >= 2600)
                                    {
                                        Ghost1.Y = 2600;
                                        Ghost1.Floor = 1;
                                        if (Ghost1.Room == 1)
                                        {
                                            Ghost1.Direction = Ghost1.Rnd.Next(0, 2);
                                            if (Ghost1.Direction == 1) Ghost1.Direction = 2;
                                        }
                                        else Ghost1.Direction = Ghost1.Rnd.Next(1, 3);
                                    }
                                }
                                break;
                        }
                        Ghost1.InPursuit = Ghost1.Floor == MainPlayer.Floor & Ghost1.Room == MainPlayer.Room &
                                           (Ghost1.Direction == 0 | Ghost1.Direction == 1) & !Ghost1.Animation &
                                           animationType == GameProcess.AnimationType.None &
                                           Math.Abs(Ghost1.X - MainPlayer.X + MainGameProcess.X) <= 1000;
                    }
                    else if(Ghost1.InPursuit | Ghost1.InAttack)
                    {
                        if (Ghost1.X > MainPlayer.X - MainGameProcess.X)
                        {
                            Ghost1.X -= Ghost1.Speed;
                            Ghost1.Direction = 0;
                        }
                        else if (Ghost1.X < MainPlayer.X - MainGameProcess.X)
                        {
                            Ghost1.X += Ghost1.Speed;
                            Ghost1.Direction = 1;
                        }

                        if (Ghost1.Floor == MainPlayer.Floor & Ghost1.Room == MainPlayer.Room &
                            (Ghost1.Direction == 0 & Math.Abs(Ghost1.X - MainPlayer.X + MainGameProcess.X) <= 800 |
                             Ghost1.Direction == 1 & Math.Abs(Ghost1.X - MainPlayer.X + MainGameProcess.X) <= 800 + MainPlayer.TextureStatic.Width * Scale) &
                            animationType == GameProcess.AnimationType.None)
                        {
                            Ghost1.InAttack = true;
                            if (Ghost1.Direction == 0 & Math.Abs(Ghost1.X - MainPlayer.X + MainGameProcess.X) <= 300 |
                                Ghost1.Direction == 1 & Math.Abs(Ghost1.X - MainPlayer.X + MainGameProcess.X) <= 300 + MainPlayer.TextureStatic.Width * Scale)
                            {
                                MainGameProcess.AudioDeath.Play();
                                Thread.Sleep(400);
                                MainGameProcess.X = -3350;
                                MainGameProcess.Y = -595;
                                MainPlayer.X = 400;
                                MainPlayer.Y = 420;
                                MainPlayer.Floor = 3;
                                MainPlayer.Room = 2;
                                MainPlayer.Direction = 0;
                                MainPlayer.Lives--;
                                Ghost1.InAttack = false;
                                GhostAttackTick = 0;
                                GhostAttackPreviousTick = 0;
                                GhostPreviousTick = 0;
                            }
                        }
                        else
                        {
                            Ghost1.InAttack = false;
                        }
                        Ghost1.InPursuit = Ghost1.Floor == MainPlayer.Floor & Ghost1.Room == MainPlayer.Room & (Ghost1.Direction == 0 | Ghost1.Direction == 1) & animationType == GameProcess.AnimationType.None & Math.Abs(Ghost1.X - MainPlayer.X + MainGameProcess.X) <= 1000;
                    }
                }
                else
                {
                    if (Ghost1.Room == 1)
                    {
                        if (GhostTick >= 0.72)
                        {
                            Ghost1.X = 3200;
                            Ghost1.Y = 1620;
                            Ghost1.Animation = false;
                            Ghost1.Direction = Ghost1.Rnd.Next(1, 3);
                            if (Ghost1.Direction == 2) Ghost1.Direction = 3;
                            Ghost1.Room = 2;
                            Ghost1.Pos = 0;
                            GhostTick = 0;
                            GhostPreviousTick = 0;
                        }
                    }
                    else
                    {
                        if (GhostTick >= 0.72)
                        {
                            Ghost1.X = 2500;
                            Ghost1.Y = 1320;
                            Ghost1.Animation = false;
                            Ghost1.Direction = Ghost1.Rnd.Next(0, 2);
                            if (Ghost1.Direction == 1) Ghost1.Direction = 3;
                            Ghost1.Room = 1;
                            Ghost1.Pos = 0;
                            GhostTick = 0;
                            GhostPreviousTick = 0;
                        }
                    }
                }
            }
            #endregion
            
            if (MainPlayer.Lives == 0) MainGameProcess.LoseGame();
        }
        else
        {
            if (MainGameProcess.IsRules)
            {
                if (ButtonBack.IsPressed)
                {
                    if (ButtonBack.IsReleased)
                    {
                        MainGameProcess.AudioButton.Play();
                        ButtonBack.IsReleased = false;
                        MainGameProcess.IsRules = false;
                    }   
                }
            }
            else
            {
                if (ButtonStart.IsPressed)
                {
                    if (ButtonStart.IsReleased)
                    {
                        MainGameProcess.AudioButton.Play();
                        ButtonStart.IsReleased = false;
                        MainGameProcess.NewGame(-1170, -1100);
                        MainPlayer.ResetParams(250, 420);
                        Ghost0.ResetParams(200,3950, 0);
                        Ghost1.ResetParams(200,2600, 1);
                        Key1.ResetParams(430, 470, 3, 2);
                        Key2.ResetParams(300, 880, 0, 1);
                    }
                }
                else if (ButtonRules.IsPressed)
                {
                    if (ButtonRules.IsReleased)
                    {
                        MainGameProcess.AudioButton.Play();
                        ButtonRules.IsReleased = false;
                        MainGameProcess.IsRules = true;
                    }
                }
                else if (ButtonBack.IsPressed)
                {
                    if (ButtonBack.IsReleased)
                    {
                        MainGameProcess.AudioButton.Play();
                        ButtonBack.IsReleased = false;
                        MainGameProcess.Continue();
                    }
                }
            }
        }
        base.Update(gameTime);
    }

    private void DrawMessage()
    {
        _spriteBatch.DrawString(MessageFont, MainGameProcess.ShowMessage(),
            new Vector2(490.AbsoluteX(), 10.AbsoluteY()), new Color(255, 255, 0),
            0, Vector2.Zero, Scale, SpriteEffects.None, 0);
    }
    private void DrawLives()
    {
        switch (MainPlayer.Lives)
        {
            case 1:
                _spriteBatch.Draw(MainPlayer.TextureLife, new Vector2(30.AbsoluteX(), 30.AbsoluteY()),
                    new Rectangle(0,0, MainPlayer.TextureLife.Width, MainPlayer.TextureLife.Height),
                    Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
                break;
            case 2:
                _spriteBatch.Draw(MainPlayer.TextureLife, new Vector2(30.AbsoluteX(), 30.AbsoluteY()),
                    new Rectangle(0,0, MainPlayer.TextureLife.Width, MainPlayer.TextureLife.Height),
                    Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
                _spriteBatch.Draw(MainPlayer.TextureLife, new Vector2(150.AbsoluteX(), 30.AbsoluteY()),
                    new Rectangle(0,0, MainPlayer.TextureLife.Width, MainPlayer.TextureLife.Height),
                    Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
                break;
            case 3:
                _spriteBatch.Draw(MainPlayer.TextureLife, new Vector2(30.AbsoluteX(), 30.AbsoluteY()),
                    new Rectangle(0,0, MainPlayer.TextureLife.Width, MainPlayer.TextureLife.Height),
                    Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
                _spriteBatch.Draw(MainPlayer.TextureLife, new Vector2(150.AbsoluteX(), 30.AbsoluteY()),
                    new Rectangle(0,0, MainPlayer.TextureLife.Width, MainPlayer.TextureLife.Height),
                    Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
                _spriteBatch.Draw(MainPlayer.TextureLife, new Vector2(270.AbsoluteX(), 30.AbsoluteY()),
                    new Rectangle(0,0, MainPlayer.TextureLife.Width, MainPlayer.TextureLife.Height),
                    Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
                break;
        }
        
    }
    private void DrawMap()
    {
        _spriteBatch.Draw(MainGameProcess.Map, new Vector2(MainGameProcess.X.AbsoluteX(),MainGameProcess.Y.AbsoluteY()),
            new Rectangle(0,0, MainGameProcess.Map.Width, MainGameProcess.Map.Height),
            Color.White, 0, Vector2.Zero, 3f*Scale, SpriteEffects.None, 0);
    }
    private void DrawMenu()
    {
        if (MainGameProcess.IsWin & !MainGameProcess.IsLose)
        {
            _spriteBatch.Draw(MainGameProcess.MenuWin, new Vector2(0.AbsoluteX(), 0.AbsoluteY()),
                new Rectangle(0,0, MainGameProcess.MenuWin.Width, MainGameProcess.MenuWin.Height),
                Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
        }
        else if (MainGameProcess.IsLose & !MainGameProcess.IsWin)
        {
            _spriteBatch.Draw(MainGameProcess.MenuLose, new Vector2(0.AbsoluteX(), 0.AbsoluteY()),
                new Rectangle(0,0, MainGameProcess.MenuLose.Width, MainGameProcess.MenuLose.Height),
                Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
        }
        else
        {
            _spriteBatch.Draw(MainGameProcess.Menu, new Vector2(0.AbsoluteX(), 0.AbsoluteY()),
                new Rectangle(0,0, MainGameProcess.Menu.Width, MainGameProcess.Menu.Height),
                Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
            if(MainGameProcess.IsManual) 
                ButtonBack.Process(_spriteBatch);
        }
    }
    private void DrawMenuRules()
    {
        _spriteBatch.Draw(MainGameProcess.MenuRules,new Vector2(0.AbsoluteX(), 0.AbsoluteY()),
            new Rectangle(0,0, MainGameProcess.MenuRules.Width, MainGameProcess.MenuRules.Height),
            Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
    }
    private void DrawItemBackGround()
    {
        _spriteBatch.Draw(MainGameProcess.ItemBack, new Vector2(1705.AbsoluteX(), 250.AbsoluteY()),
            new Rectangle(0,0, MainGameProcess.ItemBack.Width, MainGameProcess.ItemBack.Height),
            Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
    }
    private void DrawKey1()
    {
        if (Key1.IsVisible)
        {
            if(Key1.Scale <= 0.3)
                Key1.Scale += 0.003f;
            else 
            {
                if (Key1.X <= 1690)
                    Key1.X += 20;

                if (Key1.Y >= 300)
                    Key1.Y -= 3;
            }
                
            _spriteBatch.Draw(Key1.Texture, new Vector2(Key1.X.AbsoluteX(), Key1.Y.AbsoluteY()),
                new Rectangle(0,0, Key1.Texture.Width, Key1.Texture.Height),
                Color.White, 0, Vector2.Zero, (0.8f + Key1.Scale)*Scale, SpriteEffects.None, 0);
        }
    }
    private void DrawKey2()
    {
        if (Key2.IsVisible)
        {
            if(Key2.Scale <= 0.16)
                Key2.Scale += 0.003f;
            else 
            {
                if (Key2.X <= 1700)
                    Key2.X += 10;

                if (Key2.Y >= 400)
                    Key2.Y -= 3;
            }
                
            _spriteBatch.Draw(Key2.Texture, new Vector2(Key2.X.AbsoluteX(), Key2.Y.AbsoluteY()),
                new Rectangle(0,0, Key2.Texture.Width, Key2.Texture.Height),
                Color.White, 0, Vector2.Zero, (0.8f + Key2.Scale)*Scale, SpriteEffects.None, 0);
        }
    }
    private void DrawRectangle(Rectangle coords, Color color)
    {
        var rect = new Texture2D(GraphicsDevice, 1, 1);
        rect.SetData(new[] {color});
        _spriteBatch.Draw(rect, coords, color);
    }
    private void DrawBlur()
    {
        _spriteBatch.Draw(MainGameProcess.Blur,new Vector2(0.AbsoluteX(), 0.AbsoluteY()),
            new Rectangle(0,0, MainGameProcess.Blur.Width, MainGameProcess.Blur.Height),
            Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
    }
    private void DrawLadderAnimation()
    {
        if (Tick - PreviousTick >= 0.4)
        {
            switch (MainPlayer.Pos % 2)
            {
                case 0:
                    _spriteBatch.Draw(MainPlayer.TextureLadder2,
                        new Vector2(MainPlayer.X.AbsoluteX(), MainPlayer.Y.AbsoluteY()),
                        new Rectangle(0, 0, MainPlayer.TextureLadder2.Width, MainPlayer.TextureLadder2.Height),
                        Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
                    ++MainPlayer.Pos;
                    PreviousTick = Tick;
                    break;
                case 1:
                    _spriteBatch.Draw(MainPlayer.TextureLadder3,
                        new Vector2(MainPlayer.X.AbsoluteX(), MainPlayer.Y.AbsoluteY()),
                        new Rectangle(0, 0, MainPlayer.TextureLadder3.Width, MainPlayer.TextureLadder3.Height),
                        Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
                    ++MainPlayer.Pos;
                    PreviousTick = Tick;
                    break;
            }
        }
        else
        {
            switch ((MainPlayer.Pos + 1) % 2)
            {
                case 0: 
                    _spriteBatch.Draw(MainPlayer.TextureLadder2, 
                        new Vector2(MainPlayer.X.AbsoluteX(), MainPlayer.Y.AbsoluteY()), 
                        new Rectangle(0, 0, MainPlayer.TextureLadder2.Width, MainPlayer.TextureLadder2.Height),
                        Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
                    break;
                case 1: 
                    _spriteBatch.Draw(MainPlayer.TextureLadder3, 
                        new Vector2(MainPlayer.X.AbsoluteX(), MainPlayer.Y.AbsoluteY()),
                        new Rectangle(0, 0, MainPlayer.TextureLadder3.Width, MainPlayer.TextureLadder3.Height), 
                        Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
                    break;
            }
        }
    }
    private void DrawCrawlAnimation()
    {
        if (Tick - AnimTick < 0.8)
        {
            if (Tick - PreviousTick >= 0.4)
            {
                switch (MainPlayer.Pos % 4)
                {
                    case 0:
                        _spriteBatch.Draw(MainPlayer.TextureAnimation1, 
                            new Vector2(MainPlayer.X.AbsoluteX(), MainPlayer.Y.AbsoluteY()),
                            new Rectangle(0, 0, MainPlayer.TextureAnimation1.Width, MainPlayer.TextureAnimation1.Height),
                            Color.White, 0, Vector2.Zero, 1.3f*Scale, SpriteEffects.None, 0);
                        ++MainPlayer.Pos;
                        PreviousTick = Tick;
                        break;
                    case 1:
                        _spriteBatch.Draw(MainPlayer.TextureAnimation2,
                            new Vector2(MainPlayer.X.AbsoluteX(), MainPlayer.Y.AbsoluteY()),
                            new Rectangle(0, 0, MainPlayer.TextureAnimation2.Width, MainPlayer.TextureAnimation2.Height),
                            Color.White, 0, Vector2.Zero, 1.3f*Scale, SpriteEffects.None, 0);
                        ++MainPlayer.Pos;
                        PreviousTick = Tick;
                        break;
                    case 2:
                        _spriteBatch.Draw(MainPlayer.TextureAnimation3,
                            new Vector2(MainPlayer.X.AbsoluteX(), MainPlayer.Y.AbsoluteY()),
                            new Rectangle(0, 0, MainPlayer.TextureAnimation3.Width, MainPlayer.TextureAnimation3.Height),
                            Color.White, 0, Vector2.Zero, 1.3f*Scale, SpriteEffects.None, 0);
                        ++MainPlayer.Pos;
                        PreviousTick = Tick;
                        break;
                    case 3:
                        _spriteBatch.Draw(MainPlayer.TextureAnimation2,
                            new Vector2(MainPlayer.X.AbsoluteX(), MainPlayer.Y.AbsoluteY()),
                            new Rectangle(0, 0, MainPlayer.TextureAnimation2.Width, MainPlayer.TextureAnimation2.Height),
                            Color.White, 0, Vector2.Zero, 1.3f*Scale, SpriteEffects.None, 0);
                        ++MainPlayer.Pos;
                        PreviousTick = Tick;
                        break;
                }
            }
            else
            {
                switch ((MainPlayer.Pos + 1) % 4)
                {
                    case 0:
                        _spriteBatch.Draw(MainPlayer.TextureAnimation1,
                            new Vector2(MainPlayer.X.AbsoluteX(), MainPlayer.Y.AbsoluteY()),
                            new Rectangle(0, 0, MainPlayer.TextureAnimation1.Width, MainPlayer.TextureAnimation1.Height),
                            Color.White, 0, Vector2.Zero, 1.3f*Scale, SpriteEffects.None, 0);
                        break;
                    case 1:
                        _spriteBatch.Draw(MainPlayer.TextureAnimation2,
                            new Vector2(MainPlayer.X.AbsoluteX(), MainPlayer.Y.AbsoluteY()),
                            new Rectangle(0, 0, MainPlayer.TextureAnimation2.Width, MainPlayer.TextureAnimation2.Height),
                            Color.White, 0, Vector2.Zero, 1.3f*Scale, SpriteEffects.None, 0);
                        break;
                    case 2:
                        _spriteBatch.Draw(MainPlayer.TextureAnimation3,
                            new Vector2(MainPlayer.X.AbsoluteX(), MainPlayer.Y.AbsoluteY()),
                            new Rectangle(0, 0, MainPlayer.TextureAnimation3.Width, MainPlayer.TextureAnimation3.Height),
                            Color.White, 0, Vector2.Zero, 1.3f*Scale, SpriteEffects.None, 0);
                        break;
                    case 3:
                        _spriteBatch.Draw(MainPlayer.TextureAnimation2,
                            new Vector2(MainPlayer.X.AbsoluteX(), MainPlayer.Y.AbsoluteY()),
                            new Rectangle(0, 0, MainPlayer.TextureAnimation2.Width, MainPlayer.TextureAnimation2.Height),
                            Color.White, 0, Vector2.Zero, 1.3f*Scale, SpriteEffects.None, 0);
                        break;
                }
            }
        }

        if (Tick - AnimTick >= 0.8)
        {
            switch ((MainPlayer.Pos + 1) % 4)
            {
                case 0:
                    _spriteBatch.Draw(MainPlayer.TextureAnimation1,
                        new Vector2(MainPlayer.X.AbsoluteX(), MainPlayer.Y.AbsoluteY()),
                        new Rectangle(0, 0, MainPlayer.TextureAnimation1.Width, MainPlayer.TextureAnimation1.Height),
                        Color.White, 0, Vector2.Zero, 1.3f*Scale, SpriteEffects.None, 0);
                    break;
                case 1:
                    _spriteBatch.Draw(MainPlayer.TextureAnimation2,
                        new Vector2(MainPlayer.X.AbsoluteX(), MainPlayer.Y.AbsoluteY()),
                        new Rectangle(0, 0, MainPlayer.TextureAnimation2.Width, MainPlayer.TextureAnimation2.Height),
                        Color.White, 0, Vector2.Zero, 1.3f*Scale, SpriteEffects.None, 0);
                    break;
                case 2:
                    _spriteBatch.Draw(MainPlayer.TextureAnimation3,
                        new Vector2(MainPlayer.X.AbsoluteX(), MainPlayer.Y.AbsoluteY()),
                        new Rectangle(0, 0, MainPlayer.TextureAnimation3.Width, MainPlayer.TextureAnimation3.Height),
                        Color.White, 0, Vector2.Zero, 1.3f*Scale, SpriteEffects.None, 0);
                    break;
                case 3:
                    _spriteBatch.Draw(MainPlayer.TextureAnimation2,
                        new Vector2(MainPlayer.X.AbsoluteX(), MainPlayer.Y.AbsoluteY()),
                        new Rectangle(0, 0, MainPlayer.TextureAnimation2.Width, MainPlayer.TextureAnimation2.Height),
                        Color.White, 0, Vector2.Zero, 1.3f*Scale, SpriteEffects.None, 0);
                    break;
            }
        }
    }
    private void DrawClimbAnimation()
    {
        if (Tick - AnimTick < 0.5)
        {
            if (Tick - PreviousTick >= 0.3)
            {
                switch (MainPlayer.Pos % 3)
                {
                    case 0:
                        _spriteBatch.Draw(MainPlayer.TextureJump1rev,
                            new Vector2(MainPlayer.X.AbsoluteX(), MainPlayer.Y.AbsoluteY()),
                            new Rectangle(0, 0, MainPlayer.TextureJump1rev.Width, MainPlayer.TextureJump1rev.Height),
                            Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
                        ++MainPlayer.Pos;
                        PreviousTick = Tick;
                        break;
                    case 1:
                        _spriteBatch.Draw(MainPlayer.TextureJump2rev,
                            new Vector2(MainPlayer.X.AbsoluteX(), MainPlayer.Y.AbsoluteY()),
                            new Rectangle(0, 0, MainPlayer.TextureJump2rev.Width, MainPlayer.TextureJump2rev.Height),
                            Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
                        ++MainPlayer.Pos;
                        PreviousTick = Tick;
                        break;
                    case 2:
                        _spriteBatch.Draw(MainPlayer.TextureJump1rev,
                            new Vector2(MainPlayer.X.AbsoluteX(), MainPlayer.Y.AbsoluteY()),
                            new Rectangle(0, 0, MainPlayer.TextureJump1rev.Width, MainPlayer.TextureJump1rev.Height),
                            Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
                        ++MainPlayer.Pos;
                        PreviousTick = Tick;
                        break;
                    
                }
            }
            else
            {
                switch ((MainPlayer.Pos + 1) % 3)
                {
                    case 0:
                        _spriteBatch.Draw(MainPlayer.TextureJump1rev,
                            new Vector2(MainPlayer.X.AbsoluteX(), MainPlayer.Y.AbsoluteY()),
                            new Rectangle(0, 0, MainPlayer.TextureJump1rev.Width, MainPlayer.TextureJump1rev.Height),
                            Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
                        break;
                    case 1:
                        _spriteBatch.Draw(MainPlayer.TextureJump2rev,
                            new Vector2(MainPlayer.X.AbsoluteX(), MainPlayer.Y.AbsoluteY()),
                            new Rectangle(0, 0, MainPlayer.TextureJump2rev.Width, MainPlayer.TextureJump2rev.Height),
                            Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
                        break;
                    case 2:
                        _spriteBatch.Draw(MainPlayer.TextureJump1rev,
                            new Vector2(MainPlayer.X.AbsoluteX(), MainPlayer.Y.AbsoluteY()),
                            new Rectangle(0, 0, MainPlayer.TextureJump1rev.Width, MainPlayer.TextureJump1rev.Height),
                            Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
                        break;
                }
            }
        }

        if (Tick - AnimTick >= 0.5)
        {
            if (Tick - PreviousTick >= 0.4)
            {
                switch (MainPlayer.Pos % 4)
                {
                    case 0:
                        _spriteBatch.Draw(MainPlayer.TextureAnimation1rev,
                            new Vector2(MainPlayer.X.AbsoluteX(), MainPlayer.Y.AbsoluteY()),
                            new Rectangle(0, 0, MainPlayer.TextureAnimation1rev.Width, MainPlayer.TextureAnimation1rev.Height),
                            Color.White, 0, Vector2.Zero, 1.3f*Scale, SpriteEffects.None, 0);
                        ++MainPlayer.Pos;
                        PreviousTick = Tick;
                        break;
                    case 1:
                        _spriteBatch.Draw(MainPlayer.TextureAnimation2rev,
                            new Vector2(MainPlayer.X.AbsoluteX(), MainPlayer.Y.AbsoluteY()),
                            new Rectangle(0, 0, MainPlayer.TextureAnimation2rev.Width, MainPlayer.TextureAnimation2rev.Height),
                            Color.White, 0, Vector2.Zero, 1.3f*Scale, SpriteEffects.None, 0);
                        ++MainPlayer.Pos;
                        PreviousTick = Tick;
                        break;
                    case 2:
                        _spriteBatch.Draw(MainPlayer.TextureAnimation3rev,
                            new Vector2(MainPlayer.X.AbsoluteX(), MainPlayer.Y.AbsoluteY()),
                            new Rectangle(0, 0, MainPlayer.TextureAnimation3rev.Width, MainPlayer.TextureAnimation3rev.Height),
                            Color.White, 0, Vector2.Zero, 1.3f*Scale, SpriteEffects.None, 0);
                        ++MainPlayer.Pos;
                        PreviousTick = Tick;
                        break;
                    case 3:
                        _spriteBatch.Draw(MainPlayer.TextureAnimation2rev,
                            new Vector2(MainPlayer.X.AbsoluteX(), MainPlayer.Y.AbsoluteY()),
                            new Rectangle(0, 0, MainPlayer.TextureAnimation2rev.Width, MainPlayer.TextureAnimation2rev.Height),
                            Color.White, 0, Vector2.Zero, 1.3f*Scale, SpriteEffects.None, 0);
                        ++MainPlayer.Pos;
                        PreviousTick = Tick;
                        break;
                }
            }
            else
            {
                switch ((MainPlayer.Pos + 1) % 4)
                {
                    case 0:
                        _spriteBatch.Draw(MainPlayer.TextureAnimation1rev,
                            new Vector2(MainPlayer.X.AbsoluteX(), MainPlayer.Y.AbsoluteY()),
                            new Rectangle(0, 0, MainPlayer.TextureAnimation1rev.Width, MainPlayer.TextureAnimation1rev.Height),
                            Color.White, 0, Vector2.Zero, 1.3f*Scale, SpriteEffects.None, 0);
                        break;
                    case 1:
                        _spriteBatch.Draw(MainPlayer.TextureAnimation2rev,
                            new Vector2(MainPlayer.X.AbsoluteX(), MainPlayer.Y.AbsoluteY()),
                            new Rectangle(0, 0, MainPlayer.TextureAnimation2rev.Width, MainPlayer.TextureAnimation2rev.Height),
                            Color.White, 0, Vector2.Zero, 1.3f*Scale, SpriteEffects.None, 0);
                        break;
                    case 2:
                        _spriteBatch.Draw(MainPlayer.TextureAnimation3rev,
                            new Vector2(MainPlayer.X.AbsoluteX(), MainPlayer.Y.AbsoluteY()),
                            new Rectangle(0, 0, MainPlayer.TextureAnimation3rev.Width, MainPlayer.TextureAnimation3rev.Height),
                            Color.White, 0, Vector2.Zero, 1.3f*Scale, SpriteEffects.None, 0);
                        break;
                    case 3:
                        _spriteBatch.Draw(MainPlayer.TextureAnimation2rev,
                            new Vector2(MainPlayer.X.AbsoluteX(), MainPlayer.Y.AbsoluteY()),
                            new Rectangle(0, 0, MainPlayer.TextureAnimation2rev.Width, MainPlayer.TextureAnimation2rev.Height),
                            Color.White, 0, Vector2.Zero, 1.3f*Scale, SpriteEffects.None, 0);
                        break;
                }
            }
        }
    }
    private void DrawAnimation()
    {
        if (!MainPlayer.Moving)
        {
            if (MainPlayer.Direction == 0)
                _spriteBatch.Draw(MainPlayer.TextureStatic, new Vector2(MainPlayer.X.AbsoluteX(),MainPlayer.Y.AbsoluteY()),
                    new Rectangle(0,0, MainPlayer.TextureStatic.Width, MainPlayer.TextureStatic.Height),
                    Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
            else if (MainPlayer.Direction == 1)
                _spriteBatch.Draw(MainPlayer.TextureStaticRev, new Vector2(MainPlayer.X.AbsoluteX(),MainPlayer.Y.AbsoluteY()),
                    new Rectangle(0,0, MainPlayer.TextureStaticRev.Width, MainPlayer.TextureStaticRev.Height),
                    Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
            MainPlayer.PosReset();
            Tick = 0;
            PreviousTick = 0;
        }
        else if(Tick - PreviousTick >= 0.2)
        {
            switch (MainPlayer.Pos % 6)
            {
                case 0:
                    MainGameProcess.AudioSteps1.Play();
                    if (MainPlayer.Direction == 0)
                        _spriteBatch.Draw(MainPlayer.TextureGoing1, new Vector2(MainPlayer.X.AbsoluteX(),MainPlayer.Y.AbsoluteY()),
                            new Rectangle(0,0, MainPlayer.TextureGoing1.Width, MainPlayer.TextureGoing1.Height),
                            Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
                    else if (MainPlayer.Direction == 1)
                        _spriteBatch.Draw(MainPlayer.TextureGoing1Rev, new Vector2(MainPlayer.X.AbsoluteX(),MainPlayer.Y.AbsoluteY()),
                            new Rectangle(0,0, MainPlayer.TextureGoing1Rev.Width, MainPlayer.TextureGoing1Rev.Height),
                            Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
                    ++MainPlayer.Pos;
                    PreviousTick = Tick;
                    break;
                case 1:
                    if (MainPlayer.Direction == 0)
                        _spriteBatch.Draw(MainPlayer.TextureGoing2, new Vector2(MainPlayer.X.AbsoluteX(),MainPlayer.Y.AbsoluteY()),
                            new Rectangle(0,0, MainPlayer.TextureGoing2.Width, MainPlayer.TextureGoing2.Height),
                            Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
                    else if (MainPlayer.Direction == 1)
                        _spriteBatch.Draw(MainPlayer.TextureGoing2Rev, new Vector2(MainPlayer.X.AbsoluteX(),MainPlayer.Y.AbsoluteY()),
                            new Rectangle(0,0, MainPlayer.TextureGoing2Rev.Width, MainPlayer.TextureGoing2Rev.Height),
                            Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
                    ++MainPlayer.Pos;
                    PreviousTick = Tick;
                    break;
                case 2:
                    if (MainPlayer.Direction == 0)
                        _spriteBatch.Draw(MainPlayer.TextureGoing3, new Vector2(MainPlayer.X.AbsoluteX(),MainPlayer.Y.AbsoluteY()),
                            new Rectangle(0,0, MainPlayer.TextureGoing3.Width, MainPlayer.TextureGoing3.Height),
                            Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
                    else if (MainPlayer.Direction == 1)
                        _spriteBatch.Draw(MainPlayer.TextureGoing3Rev, new Vector2(MainPlayer.X.AbsoluteX(),MainPlayer.Y.AbsoluteY()),
                            new Rectangle(0,0, MainPlayer.TextureGoing3Rev.Width, MainPlayer.TextureGoing3Rev.Height),
                            Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
                    ++MainPlayer.Pos;
                    PreviousTick = Tick;
                    break;
                case 3:
                    MainGameProcess.AudioSteps2.Play();
                    if (MainPlayer.Direction == 0)
                        _spriteBatch.Draw(MainPlayer.TextureGoing4, new Vector2(MainPlayer.X.AbsoluteX(),MainPlayer.Y.AbsoluteY()),
                            new Rectangle(0,0, MainPlayer.TextureGoing4.Width, MainPlayer.TextureGoing4.Height),
                            Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
                    else if (MainPlayer.Direction == 1)
                        _spriteBatch.Draw(MainPlayer.TextureGoing4Rev, new Vector2(MainPlayer.X.AbsoluteX(),MainPlayer.Y.AbsoluteY()),
                            new Rectangle(0,0, MainPlayer.TextureGoing4Rev.Width, MainPlayer.TextureGoing4Rev.Height),
                            Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
                    ++MainPlayer.Pos;
                    PreviousTick = Tick;
                    break;
                case 4:
                    if (MainPlayer.Direction == 0)
                        _spriteBatch.Draw(MainPlayer.TextureGoing5, new Vector2(MainPlayer.X.AbsoluteX(),MainPlayer.Y.AbsoluteY()),
                            new Rectangle(0,0, MainPlayer.TextureGoing5.Width, MainPlayer.TextureGoing5.Height),
                            Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
                    else if (MainPlayer.Direction == 1)
                        _spriteBatch.Draw(MainPlayer.TextureGoing5Rev, new Vector2(MainPlayer.X.AbsoluteX(),MainPlayer.Y.AbsoluteY()),
                            new Rectangle(0,0, MainPlayer.TextureGoing5Rev.Width, MainPlayer.TextureGoing5Rev.Height),
                            Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
                    ++MainPlayer.Pos;
                    PreviousTick = Tick;
                    break;
                case 5:
                    if (MainPlayer.Direction == 0)
                        _spriteBatch.Draw(MainPlayer.TextureGoing6, new Vector2(MainPlayer.X.AbsoluteX(),MainPlayer.Y.AbsoluteY()),
                            new Rectangle(0,0, MainPlayer.TextureGoing6.Width, MainPlayer.TextureGoing6.Height),
                            Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
                    else if (MainPlayer.Direction == 1)
                        _spriteBatch.Draw(MainPlayer.TextureGoing6Rev, new Vector2(MainPlayer.X.AbsoluteX(),MainPlayer.Y.AbsoluteY()),
                            new Rectangle(0,0, MainPlayer.TextureGoing6Rev.Width, MainPlayer.TextureGoing6Rev.Height),
                            Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
                    ++MainPlayer.Pos;
                    PreviousTick = Tick;
                    break;
            }
        }
        else
        {
            switch ((MainPlayer.Pos + 1) % 6)
            {
                case 0:
                    if (MainPlayer.Direction == 0)
                        _spriteBatch.Draw(MainPlayer.TextureGoing1, new Vector2(MainPlayer.X.AbsoluteX(),MainPlayer.Y.AbsoluteY()),
                            new Rectangle(0,0, MainPlayer.TextureGoing1.Width, MainPlayer.TextureGoing1.Height),
                            Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
                    else if (MainPlayer.Direction == 1)
                        _spriteBatch.Draw(MainPlayer.TextureGoing1Rev, new Vector2(MainPlayer.X.AbsoluteX(),MainPlayer.Y.AbsoluteY()),
                            new Rectangle(0,0, MainPlayer.TextureGoing1Rev.Width, MainPlayer.TextureGoing1Rev.Height),
                            Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
                    break;
                case 1:
                    if (MainPlayer.Direction == 0)
                        _spriteBatch.Draw(MainPlayer.TextureGoing2, new Vector2(MainPlayer.X.AbsoluteX(),MainPlayer.Y.AbsoluteY()),
                            new Rectangle(0,0, MainPlayer.TextureGoing2.Width, MainPlayer.TextureGoing2.Height),
                            Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
                    else if (MainPlayer.Direction == 1)
                        _spriteBatch.Draw(MainPlayer.TextureGoing2Rev, new Vector2(MainPlayer.X.AbsoluteX(),MainPlayer.Y.AbsoluteY()),
                            new Rectangle(0,0, MainPlayer.TextureGoing2Rev.Width, MainPlayer.TextureGoing2Rev.Height),
                            Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
                    break;
                case 2:
                    if (MainPlayer.Direction == 0)
                        _spriteBatch.Draw(MainPlayer.TextureGoing3, new Vector2(MainPlayer.X.AbsoluteX(),MainPlayer.Y.AbsoluteY()),
                            new Rectangle(0,0, MainPlayer.TextureGoing3.Width, MainPlayer.TextureGoing3.Height),
                            Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
                    else if (MainPlayer.Direction == 1)
                        _spriteBatch.Draw(MainPlayer.TextureGoing3Rev, new Vector2(MainPlayer.X.AbsoluteX(),MainPlayer.Y.AbsoluteY()),
                            new Rectangle(0,0, MainPlayer.TextureGoing3Rev.Width, MainPlayer.TextureGoing3Rev.Height),
                            Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
                    break;
                case 3:
                    if (MainPlayer.Direction == 0)
                        _spriteBatch.Draw(MainPlayer.TextureGoing4, new Vector2(MainPlayer.X.AbsoluteX(),MainPlayer.Y.AbsoluteY()),
                            new Rectangle(0,0, MainPlayer.TextureGoing4.Width, MainPlayer.TextureGoing4.Height),
                            Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
                    else if (MainPlayer.Direction == 1)
                        _spriteBatch.Draw(MainPlayer.TextureGoing4Rev, new Vector2(MainPlayer.X.AbsoluteX(),MainPlayer.Y.AbsoluteY()),
                            new Rectangle(0,0, MainPlayer.TextureGoing4Rev.Width, MainPlayer.TextureGoing4Rev.Height),
                            Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
                    break;
                case 4:
                    if (MainPlayer.Direction == 0)
                        _spriteBatch.Draw(MainPlayer.TextureGoing5, new Vector2(MainPlayer.X.AbsoluteX(),MainPlayer.Y.AbsoluteY()),
                            new Rectangle(0,0, MainPlayer.TextureGoing5.Width, MainPlayer.TextureGoing5.Height),
                            Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
                    else if (MainPlayer.Direction == 1)
                        _spriteBatch.Draw(MainPlayer.TextureGoing5Rev, new Vector2(MainPlayer.X.AbsoluteX(),MainPlayer.Y.AbsoluteY()),
                            new Rectangle(0,0, MainPlayer.TextureGoing5Rev.Width, MainPlayer.TextureGoing5Rev.Height),
                            Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
                    break;
                case 5:
                    if (MainPlayer.Direction == 0)
                        _spriteBatch.Draw(MainPlayer.TextureGoing6, new Vector2(MainPlayer.X.AbsoluteX(),MainPlayer.Y.AbsoluteY()),
                            new Rectangle(0,0, MainPlayer.TextureGoing6.Width, MainPlayer.TextureGoing6.Height),
                            Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
                    else if (MainPlayer.Direction == 1)
                        _spriteBatch.Draw(MainPlayer.TextureGoing6Rev, new Vector2(MainPlayer.X.AbsoluteX(),MainPlayer.Y.AbsoluteY()),
                            new Rectangle(0,0, MainPlayer.TextureGoing6Rev.Width, MainPlayer.TextureGoing6Rev.Height),
                            Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
                    break;
            }
        }
    }
    private void DrawGhost(ref Enemy ghost)
    {
        if (ghost.InAttack)
        {
            if (ghost.Direction == 1)
            {
                if (GhostAttackTick - GhostAttackPreviousTick >= 0.3)
                {
                    switch (ghost.Pos % 4)
                    {
                        case 0:
                            _spriteBatch.Draw(ghost.TextureAttack1,
                                new Vector2((ghost.X + MainGameProcess.X).AbsoluteX(),
                                    (ghost.Y + MainGameProcess.Y).AbsoluteY()),
                                new Rectangle(0, 0, ghost.TextureAttack1.Width, ghost.TextureAttack1.Height),
                                Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
                            ++ghost.Pos;
                            GhostAttackPreviousTick = GhostAttackTick;
                            break;
                        case 1:
                            MainGameProcess.AudioAttack.Play();
                            _spriteBatch.Draw(ghost.TextureAttack2,
                                new Vector2((ghost.X + MainGameProcess.X).AbsoluteX(),
                                    (ghost.Y + MainGameProcess.Y).AbsoluteY()),
                                new Rectangle(0, 0, ghost.TextureAttack2.Width, ghost.TextureAttack2.Height),
                                Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
                            ++ghost.Pos;
                            GhostAttackPreviousTick = GhostAttackTick;
                            break;
                        case 2:
                            _spriteBatch.Draw(ghost.TextureAttack3,
                                new Vector2((ghost.X + MainGameProcess.X).AbsoluteX(),
                                    (ghost.Y + MainGameProcess.Y).AbsoluteY()),
                                new Rectangle(0, 0, ghost.TextureAttack3.Width, ghost.TextureAttack3.Height),
                                Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
                            ++ghost.Pos;
                            GhostAttackPreviousTick = GhostAttackTick;
                            break;
                        case 3:
                            _spriteBatch.Draw(ghost.TextureAttack4,
                                new Vector2((ghost.X + MainGameProcess.X).AbsoluteX(),
                                    (ghost.Y + MainGameProcess.Y).AbsoluteY()),
                                new Rectangle(0, 0, ghost.TextureAttack4.Width, ghost.TextureAttack4.Height),
                                Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
                            ++ghost.Pos;
                            GhostAttackPreviousTick = GhostAttackTick;
                            break;
                    }
                }
                else
                {
                    switch ((ghost.Pos - 1) % 4)
                    {
                        case 0:
                            _spriteBatch.Draw(ghost.TextureAttack1,
                                new Vector2((ghost.X + MainGameProcess.X).AbsoluteX(),
                                    (ghost.Y + MainGameProcess.Y).AbsoluteY()),
                                new Rectangle(0, 0, ghost.TextureAttack1.Width, ghost.TextureAttack1.Height),
                                Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
                            break;
                        case 1:
                            _spriteBatch.Draw(ghost.TextureAttack2,
                                new Vector2((ghost.X + MainGameProcess.X).AbsoluteX(),
                                    (ghost.Y + MainGameProcess.Y).AbsoluteY()),
                                new Rectangle(0, 0, ghost.TextureAttack2.Width, ghost.TextureAttack2.Height),
                                Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
                            break;
                        case 2:
                            _spriteBatch.Draw(ghost.TextureAttack3,
                                new Vector2((ghost.X + MainGameProcess.X).AbsoluteX(),
                                    (ghost.Y + MainGameProcess.Y).AbsoluteY()),
                                new Rectangle(0, 0, ghost.TextureAttack3.Width, ghost.TextureAttack3.Height),
                                Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
                            break;
                        case 3:
                            _spriteBatch.Draw(ghost.TextureAttack4,
                                new Vector2((ghost.X + MainGameProcess.X).AbsoluteX(),
                                    (ghost.Y + MainGameProcess.Y).AbsoluteY()),
                                new Rectangle(0, 0, ghost.TextureAttack4.Width, ghost.TextureAttack4.Height),
                                Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
                            break;
                    }
                }
            }
            else
            {
                if (GhostAttackTick - GhostAttackPreviousTick >= 0.3)
                {
                    switch (ghost.Pos % 4)
                    {
                        case 0:
                            _spriteBatch.Draw(ghost.TextureAttack1rev,
                                new Vector2((ghost.X + MainGameProcess.X).AbsoluteX(),
                                    (ghost.Y + MainGameProcess.Y).AbsoluteY()),
                                new Rectangle(0, 0, ghost.TextureAttack1rev.Width, ghost.TextureAttack1rev.Height),
                                Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
                            ++ghost.Pos;
                            GhostAttackPreviousTick = GhostAttackTick;
                            break;
                        case 1:
                            _spriteBatch.Draw(ghost.TextureAttack2rev,
                                new Vector2((ghost.X + MainGameProcess.X).AbsoluteX(),
                                    (ghost.Y + MainGameProcess.Y).AbsoluteY()),
                                new Rectangle(0, 0, ghost.TextureAttack2rev.Width, ghost.TextureAttack2rev.Height),
                                Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
                            ++ghost.Pos;
                            GhostAttackPreviousTick = GhostAttackTick;
                            break;
                        case 2:
                            _spriteBatch.Draw(ghost.TextureAttack3rev,
                                new Vector2((ghost.X + MainGameProcess.X).AbsoluteX(),
                                    (ghost.Y + MainGameProcess.Y).AbsoluteY()),
                                new Rectangle(0, 0, ghost.TextureAttack3rev.Width, ghost.TextureAttack3rev.Height),
                                Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
                            ++ghost.Pos;
                            GhostAttackPreviousTick = GhostAttackTick;
                            break;
                        case 3:
                            _spriteBatch.Draw(ghost.TextureAttack4rev,
                                new Vector2((ghost.X + MainGameProcess.X).AbsoluteX(),
                                    (ghost.Y + MainGameProcess.Y).AbsoluteY()),
                                new Rectangle(0, 0, ghost.TextureAttack4rev.Width, ghost.TextureAttack4rev.Height),
                                Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
                            ++ghost.Pos;
                            GhostAttackPreviousTick = GhostAttackTick;
                            break;
                    }
                }
                else
                {
                    switch ((ghost.Pos - 1) % 4)
                    {
                        case 0:
                            _spriteBatch.Draw(ghost.TextureAttack1rev,
                                new Vector2((ghost.X + MainGameProcess.X).AbsoluteX(),
                                    (ghost.Y + MainGameProcess.Y).AbsoluteY()),
                                new Rectangle(0, 0, ghost.TextureAttack1rev.Width, ghost.TextureAttack1rev.Height),
                                Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
                            break;
                        case 1:
                            _spriteBatch.Draw(ghost.TextureAttack2rev,
                                new Vector2((ghost.X + MainGameProcess.X).AbsoluteX(),
                                    (ghost.Y + MainGameProcess.Y).AbsoluteY()),
                                new Rectangle(0, 0, ghost.TextureAttack2rev.Width, ghost.TextureAttack2rev.Height),
                                Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
                            break;
                        case 2:
                            _spriteBatch.Draw(ghost.TextureAttack3rev,
                                new Vector2((ghost.X + MainGameProcess.X).AbsoluteX(),
                                    (ghost.Y + MainGameProcess.Y).AbsoluteY()),
                                new Rectangle(0, 0, ghost.TextureAttack3rev.Width, ghost.TextureAttack3rev.Height),
                                Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
                            break;
                        case 3:
                            _spriteBatch.Draw(ghost.TextureAttack4rev,
                                new Vector2((ghost.X + MainGameProcess.X).AbsoluteX(),
                                    (ghost.Y + MainGameProcess.Y).AbsoluteY()),
                                new Rectangle(0, 0, ghost.TextureAttack4rev.Width, ghost.TextureAttack4rev.Height),
                                Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
                            break;
                    }
                }
            }
        }
        else
        {
            if (ghost.Direction == 0 | ghost.Direction == 2 | ghost.Direction == 3)
                _spriteBatch.Draw(ghost.TextureStaticRev, new Vector2((ghost.X + MainGameProcess.X).AbsoluteX(),(ghost.Y + MainGameProcess.Y).AbsoluteY()),
                    new Rectangle(0,0, ghost.TextureStatic.Width, ghost.TextureStatic.Height),
                    Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
            else if (ghost.Direction == 1)
                _spriteBatch.Draw(ghost.TextureStatic, new Vector2((ghost.X + MainGameProcess.X).AbsoluteX(),(ghost.Y + MainGameProcess.Y).AbsoluteY()),
                    new Rectangle(0,0, ghost.TextureStatic.Width, ghost.TextureStatic.Height),
                    Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
        }
    }
    private void DrawGhostTpAnimation(ref Enemy ghost)
    {
        if (ghost.Room == 1)
        {
            if (GhostTick - GhostPreviousTick >= 0.08)
            {
                switch (ghost.Pos % 10)
                {
                    case 0:
                        _spriteBatch.Draw(ghost.TextureAnimation1, new Vector2((ghost.X + MainGameProcess.X).AbsoluteX(),(ghost.Y + MainGameProcess.Y).AbsoluteY()),
                            new Rectangle(0,0, ghost.TextureAnimation1.Width, ghost.TextureAnimation1.Height),
                            Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
                        ++ghost.Pos;
                        GhostPreviousTick = GhostTick;
                        break;
                    case 1:
                        _spriteBatch.Draw(ghost.TextureAnimation2, new Vector2((ghost.X + MainGameProcess.X).AbsoluteX(),(ghost.Y + MainGameProcess.Y).AbsoluteY()),
                            new Rectangle(0,0, ghost.TextureAnimation2.Width, ghost.TextureAnimation2.Height),
                            Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
                        ++ghost.Pos;
                        GhostPreviousTick = GhostTick;
                        break;
                    case 2:
                        _spriteBatch.Draw(ghost.TextureAnimation3, new Vector2((ghost.X + MainGameProcess.X).AbsoluteX(),(ghost.Y + MainGameProcess.Y).AbsoluteY()),
                            new Rectangle(0,0, ghost.TextureAnimation3.Width, ghost.TextureAnimation3.Height),
                            Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
                        ++ghost.Pos;
                        GhostPreviousTick = GhostTick;
                        break;
                    case 3:
                        _spriteBatch.Draw(ghost.TextureAnimation4, new Vector2((ghost.X + MainGameProcess.X).AbsoluteX(),(ghost.Y + MainGameProcess.Y).AbsoluteY()),
                            new Rectangle(0,0, ghost.TextureAnimation4.Width, ghost.TextureAnimation4.Height),
                            Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
                        ++ghost.Pos;
                        GhostPreviousTick = GhostTick;
                        break;
                    case 4:
                        _spriteBatch.Draw(ghost.TextureAnimation5, new Vector2((ghost.X + MainGameProcess.X).AbsoluteX(),(ghost.Y + MainGameProcess.Y).AbsoluteY()),
                            new Rectangle(0,0, ghost.TextureAnimation5.Width, ghost.TextureAnimation5.Height),
                            Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
                        ++ghost.Pos;
                        GhostPreviousTick = GhostTick;
                        break;
                    case 5:
                        _spriteBatch.Draw(ghost.TextureAnimation6, new Vector2((ghost.X + MainGameProcess.X).AbsoluteX(),(ghost.Y + MainGameProcess.Y).AbsoluteY()),
                            new Rectangle(0,0, ghost.TextureAnimation6.Width, ghost.TextureAnimation6.Height),
                            Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
                        ++ghost.Pos;
                        GhostPreviousTick = GhostTick;
                        break;
                    case 6:
                        _spriteBatch.Draw(ghost.TextureAnimation7, new Vector2((ghost.X + MainGameProcess.X).AbsoluteX(),(ghost.Y + MainGameProcess.Y).AbsoluteY()),
                            new Rectangle(0,0, ghost.TextureAnimation7.Width, ghost.TextureAnimation7.Height),
                            Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
                        ++ghost.Pos;
                        GhostPreviousTick = GhostTick;
                        break;
                    case 7:
                        _spriteBatch.Draw(ghost.TextureAnimation8, new Vector2((ghost.X + MainGameProcess.X).AbsoluteX(),(ghost.Y + MainGameProcess.Y).AbsoluteY()),
                            new Rectangle(0,0, ghost.TextureAnimation8.Width, ghost.TextureAnimation8.Height),
                            Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
                        ++ghost.Pos;
                        GhostPreviousTick = GhostTick;
                        break;
                    case 8:
                        _spriteBatch.Draw(ghost.TextureAnimation9, new Vector2((ghost.X + MainGameProcess.X).AbsoluteX(),(ghost.Y + MainGameProcess.Y).AbsoluteY()),
                            new Rectangle(0,0, ghost.TextureAnimation9.Width, ghost.TextureAnimation9.Height),
                            Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
                        ++ghost.Pos;
                        GhostPreviousTick = GhostTick;
                        break;
                    case 9:
                        _spriteBatch.Draw(ghost.TextureAnimation10, new Vector2((ghost.X + MainGameProcess.X).AbsoluteX(),(ghost.Y + MainGameProcess.Y).AbsoluteY()),
                            new Rectangle(0,0, ghost.TextureAnimation10.Width, ghost.TextureAnimation10.Height),
                            Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
                        ++ghost.Pos;
                        GhostPreviousTick = GhostTick;
                        break;
                }
            }
            else
            {
                switch ((ghost.Pos - 1) % 10)
                {
                    case 0:
                        _spriteBatch.Draw(ghost.TextureAnimation1,
                            new Vector2((ghost.X + MainGameProcess.X).AbsoluteX(), (ghost.Y + MainGameProcess.Y).AbsoluteY()),
                            new Rectangle(0, 0, ghost.TextureAnimation1.Width, ghost.TextureAnimation1.Height),
                            Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
                        break;
                    case 1:
                        _spriteBatch.Draw(ghost.TextureAnimation2,
                            new Vector2((ghost.X + MainGameProcess.X).AbsoluteX(), (ghost.Y + MainGameProcess.Y).AbsoluteY()),
                            new Rectangle(0, 0, ghost.TextureAnimation2.Width, ghost.TextureAnimation2.Height),
                            Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
                        break;
                    case 2:
                        _spriteBatch.Draw(ghost.TextureAnimation3,
                            new Vector2((ghost.X + MainGameProcess.X).AbsoluteX(), (ghost.Y + MainGameProcess.Y).AbsoluteY()),
                            new Rectangle(0, 0, ghost.TextureAnimation3.Width, ghost.TextureAnimation3.Height),
                            Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
                        break;
                    case 3:
                        _spriteBatch.Draw(ghost.TextureAnimation4,
                            new Vector2((ghost.X + MainGameProcess.X).AbsoluteX(), (ghost.Y + MainGameProcess.Y).AbsoluteY()),
                            new Rectangle(0, 0, ghost.TextureAnimation4.Width, ghost.TextureAnimation4.Height),
                            Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
                        break;
                    case 4:
                        _spriteBatch.Draw(ghost.TextureAnimation5,
                            new Vector2((ghost.X + MainGameProcess.X).AbsoluteX(), (ghost.Y + MainGameProcess.Y).AbsoluteY()),
                            new Rectangle(0, 0, ghost.TextureAnimation5.Width, ghost.TextureAnimation5.Height),
                            Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
                        break;
                    case 5:
                        _spriteBatch.Draw(ghost.TextureAnimation6,
                            new Vector2((ghost.X + MainGameProcess.X).AbsoluteX(), (ghost.Y + MainGameProcess.Y).AbsoluteY()),
                            new Rectangle(0, 0, ghost.TextureAnimation6.Width, ghost.TextureAnimation6.Height),
                            Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
                        break;
                    case 6:
                        _spriteBatch.Draw(ghost.TextureAnimation7,
                            new Vector2((ghost.X + MainGameProcess.X).AbsoluteX(), (ghost.Y + MainGameProcess.Y).AbsoluteY()),
                            new Rectangle(0, 0, ghost.TextureAnimation7.Width, ghost.TextureAnimation7.Height),
                            Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
                        break;
                    case 7:
                        _spriteBatch.Draw(ghost.TextureAnimation8,
                            new Vector2((ghost.X + MainGameProcess.X).AbsoluteX(), (ghost.Y + MainGameProcess.Y).AbsoluteY()),
                            new Rectangle(0, 0, ghost.TextureAnimation8.Width, ghost.TextureAnimation8.Height),
                            Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
                        break;
                    case 8:
                        _spriteBatch.Draw(ghost.TextureAnimation9,
                            new Vector2((ghost.X + MainGameProcess.X).AbsoluteX(), (ghost.Y + MainGameProcess.Y).AbsoluteY()),
                            new Rectangle(0, 0, ghost.TextureAnimation9.Width, ghost.TextureAnimation9.Height),
                            Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
                        break;
                    case 9:
                        _spriteBatch.Draw(ghost.TextureAnimation10,
                            new Vector2((ghost.X + MainGameProcess.X).AbsoluteX(), (ghost.Y + MainGameProcess.Y).AbsoluteY()),
                            new Rectangle(0, 0, ghost.TextureAnimation10.Width, ghost.TextureAnimation10.Height),
                            Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
                        break;
                }
            }
        }
        else
        {
            if (GhostTick - GhostPreviousTick >= 0.08)
            {
                switch (ghost.Pos % 10)
                {
                    case 0:
                        _spriteBatch.Draw(ghost.TextureAnimation1rev, new Vector2((ghost.X + MainGameProcess.X).AbsoluteX(),(ghost.Y + MainGameProcess.Y).AbsoluteY()),
                            new Rectangle(0,0, ghost.TextureAnimation1rev.Width, ghost.TextureAnimation1rev.Height),
                            Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
                        ++ghost.Pos;
                        GhostPreviousTick = GhostTick;
                        break;
                    case 1:
                        _spriteBatch.Draw(ghost.TextureAnimation2rev, new Vector2((ghost.X + MainGameProcess.X).AbsoluteX(),(ghost.Y + MainGameProcess.Y).AbsoluteY()),
                            new Rectangle(0,0, ghost.TextureAnimation2rev.Width, ghost.TextureAnimation2rev.Height),
                            Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
                        ++ghost.Pos;
                        GhostPreviousTick = GhostTick;
                        break;
                    case 2:
                        _spriteBatch.Draw(ghost.TextureAnimation3rev, new Vector2((ghost.X + MainGameProcess.X).AbsoluteX(),(ghost.Y + MainGameProcess.Y).AbsoluteY()),
                            new Rectangle(0,0, ghost.TextureAnimation3rev.Width, ghost.TextureAnimation3rev.Height),
                            Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
                        ++ghost.Pos;
                        GhostPreviousTick = GhostTick;
                        break;
                    case 3:
                        _spriteBatch.Draw(ghost.TextureAnimation4rev, new Vector2((ghost.X + MainGameProcess.X).AbsoluteX(),(ghost.Y + MainGameProcess.Y).AbsoluteY()),
                            new Rectangle(0,0, ghost.TextureAnimation4rev.Width, ghost.TextureAnimation4rev.Height),
                            Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
                        ++ghost.Pos;
                        GhostPreviousTick = GhostTick;
                        break;
                    case 4:
                        _spriteBatch.Draw(ghost.TextureAnimation5rev, new Vector2((ghost.X + MainGameProcess.X).AbsoluteX(),(ghost.Y + MainGameProcess.Y).AbsoluteY()),
                            new Rectangle(0,0, ghost.TextureAnimation5rev.Width, ghost.TextureAnimation5rev.Height),
                            Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
                        ++ghost.Pos;
                        GhostPreviousTick = GhostTick;
                        break;
                    case 5:
                        _spriteBatch.Draw(ghost.TextureAnimation6rev, new Vector2((ghost.X + MainGameProcess.X).AbsoluteX(),(ghost.Y + MainGameProcess.Y).AbsoluteY()),
                            new Rectangle(0,0, ghost.TextureAnimation6rev.Width, ghost.TextureAnimation6rev.Height),
                            Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
                        ++ghost.Pos;
                        GhostPreviousTick = GhostTick;
                        break;
                    case 6:
                        _spriteBatch.Draw(ghost.TextureAnimation7rev, new Vector2((ghost.X + MainGameProcess.X).AbsoluteX(),(ghost.Y + MainGameProcess.Y).AbsoluteY()),
                            new Rectangle(0,0, ghost.TextureAnimation7rev.Width, ghost.TextureAnimation7rev.Height),
                            Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
                        ++ghost.Pos;
                        GhostPreviousTick = GhostTick;
                        break;
                    case 7:
                        _spriteBatch.Draw(ghost.TextureAnimation8rev, new Vector2((ghost.X + MainGameProcess.X).AbsoluteX(),(ghost.Y + MainGameProcess.Y).AbsoluteY()),
                            new Rectangle(0,0, ghost.TextureAnimation8rev.Width, ghost.TextureAnimation8rev.Height),
                            Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
                        ++ghost.Pos;
                        GhostPreviousTick = GhostTick;
                        break;
                    case 8:
                        _spriteBatch.Draw(ghost.TextureAnimation9rev, new Vector2((ghost.X + MainGameProcess.X).AbsoluteX(),(ghost.Y + MainGameProcess.Y).AbsoluteY()),
                            new Rectangle(0,0, ghost.TextureAnimation9rev.Width, ghost.TextureAnimation9rev.Height),
                            Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
                        ++ghost.Pos;
                        GhostPreviousTick = GhostTick;
                        break;
                    case 9:
                        _spriteBatch.Draw(ghost.TextureAnimation10rev, new Vector2((ghost.X + MainGameProcess.X).AbsoluteX(),(ghost.Y + MainGameProcess.Y).AbsoluteY()),
                            new Rectangle(0,0, ghost.TextureAnimation10rev.Width, ghost.TextureAnimation10rev.Height),
                            Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
                        ++ghost.Pos;
                        GhostPreviousTick = GhostTick;
                        break;
                }
            }
            else
            {
                switch ((ghost.Pos - 1) % 10)
                {
                    case 0:
                        _spriteBatch.Draw(ghost.TextureAnimation1rev, new Vector2((ghost.X + MainGameProcess.X).AbsoluteX(), (ghost.Y + MainGameProcess.Y).AbsoluteY()),
                            new Rectangle(0, 0, ghost.TextureAnimation1rev.Width, ghost.TextureAnimation1rev.Height),
                            Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
                        break;
                    case 1:
                        _spriteBatch.Draw(ghost.TextureAnimation2rev, new Vector2((ghost.X + MainGameProcess.X).AbsoluteX(), (ghost.Y + MainGameProcess.Y).AbsoluteY()),
                            new Rectangle(0, 0, ghost.TextureAnimation2rev.Width, ghost.TextureAnimation2rev.Height),
                            Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
                        break;
                    case 2:
                        _spriteBatch.Draw(ghost.TextureAnimation3rev, new Vector2((ghost.X + MainGameProcess.X).AbsoluteX(), (ghost.Y + MainGameProcess.Y).AbsoluteY()),
                            new Rectangle(0, 0, ghost.TextureAnimation3rev.Width, ghost.TextureAnimation3rev.Height),
                            Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
                        break;
                    case 3:
                        _spriteBatch.Draw(ghost.TextureAnimation4rev, new Vector2((ghost.X + MainGameProcess.X).AbsoluteX(), (ghost.Y + MainGameProcess.Y).AbsoluteY()),
                            new Rectangle(0, 0, ghost.TextureAnimation4rev.Width, ghost.TextureAnimation4rev.Height),
                            Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
                        break;
                    case 4:
                        _spriteBatch.Draw(ghost.TextureAnimation5rev, new Vector2((ghost.X + MainGameProcess.X).AbsoluteX(), (ghost.Y + MainGameProcess.Y).AbsoluteY()),
                            new Rectangle(0, 0, ghost.TextureAnimation5rev.Width, ghost.TextureAnimation5rev.Height),
                            Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
                        break;
                    case 5:
                        _spriteBatch.Draw(ghost.TextureAnimation6rev, new Vector2((ghost.X + MainGameProcess.X).AbsoluteX(), (ghost.Y + MainGameProcess.Y).AbsoluteY()),
                            new Rectangle(0, 0, ghost.TextureAnimation6rev.Width, ghost.TextureAnimation6rev.Height),
                            Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
                        break;
                    case 6:
                        _spriteBatch.Draw(ghost.TextureAnimation7rev, new Vector2((ghost.X + MainGameProcess.X).AbsoluteX(), (ghost.Y + MainGameProcess.Y).AbsoluteY()),
                            new Rectangle(0, 0, ghost.TextureAnimation7rev.Width, ghost.TextureAnimation7rev.Height),
                            Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
                        break;
                    case 7:
                        _spriteBatch.Draw(ghost.TextureAnimation8rev, new Vector2((ghost.X + MainGameProcess.X).AbsoluteX(), (ghost.Y + MainGameProcess.Y).AbsoluteY()),
                            new Rectangle(0, 0, ghost.TextureAnimation8rev.Width, ghost.TextureAnimation8rev.Height),
                            Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
                        break;
                    case 8:
                        _spriteBatch.Draw(ghost.TextureAnimation9rev, new Vector2((ghost.X + MainGameProcess.X).AbsoluteX(), (ghost.Y + MainGameProcess.Y).AbsoluteY()),
                            new Rectangle(0, 0, ghost.TextureAnimation9rev.Width, ghost.TextureAnimation9rev.Height),
                            Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
                        break;
                    case 9:
                        _spriteBatch.Draw(ghost.TextureAnimation10rev, new Vector2((ghost.X + MainGameProcess.X).AbsoluteX(), (ghost.Y + MainGameProcess.Y).AbsoluteY()),
                            new Rectangle(0, 0, ghost.TextureAnimation10rev.Width, ghost.TextureAnimation10rev.Height),
                            Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
                        break;
                }
            }
        }
    }
    
    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);
        _spriteBatch.Begin();

        if (!MainGameProcess.IsPause)
        {
            Tick += gameTime.GetElapsedSeconds();
            DrawMap();

            if(MainGameProcess.IsGame) DrawAnimation(); //отрисовка анимации ходьбы
            else if (animationType == GameProcess.AnimationType.Climb) //отрисовка прохода на 2 этаже
            {
                if (MainPlayer.Room == 1)
                    DrawCrawlAnimation();
                else
                    DrawClimbAnimation();
            }
            else if (animationType == GameProcess.AnimationType.Ladder) //отрисовка спуска/подъема по лестнице
            {
                DrawLadderAnimation();
            }

            if (Ghost1.Animation) DrawGhostTpAnimation(ref Ghost1);
            else DrawGhost(ref Ghost1);
            DrawGhost(ref Ghost0);
        
            DrawBlur();
            DrawItemBackGround();
            DrawKey1();
            DrawKey2();
        
            ButtonMoveLeft.Process(_spriteBatch);
            ButtonMoveRight.Process(_spriteBatch);
            ButtonLadder.Process(_spriteBatch);
            ButtonGo.Process(_spriteBatch);
            ButtonPick.Process(_spriteBatch);
            ButtonPause.Process(_spriteBatch);
        
            DrawLives();
            
            if (MainGameProcess.IsMessageShowing)
            {
                Time += gameTime.GetElapsedSeconds();
                if (Time <= 4)
                    DrawMessage();
                else
                {
                    MainGameProcess.IsMessageShowing = false;
                    Time = 0;
                }
            }
        }
        else
        {
            if (MainGameProcess.IsRules)
            {
                DrawMenuRules();
                ButtonBack.Process(_spriteBatch);
            }
            else
            {
                DrawMenu();
                ButtonStart.Process(_spriteBatch);
                ButtonRules.Process(_spriteBatch);
            }
        }
        
        DrawRectangle (new Rectangle (0, 0, (int)((CurrentWidth-CountedWidth)/2), (int)CurrentHeight), Color.Black);
        DrawRectangle (new Rectangle ((int)(CurrentWidth - (CurrentWidth-CountedWidth)/2), 0, (int)((CurrentWidth-CountedWidth)/2) + 2, (int)CurrentHeight), Color.Black);
        DrawRectangle (new Rectangle(0, 0, (int)CurrentWidth, (int)((CurrentHeight-CountedHeight)/2)), Color.Black);
        DrawRectangle (new Rectangle(0, (int)(CurrentHeight - (CurrentHeight-CountedHeight)/2), (int)CurrentWidth, (int)((CurrentHeight-CountedHeight)/2) + 2), Color.Black);
        
        _spriteBatch.End();
        base.Draw(gameTime);
    }
}