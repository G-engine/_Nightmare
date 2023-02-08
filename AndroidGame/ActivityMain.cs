using Android.App;
using Android.Content.PM;
using Android.Icu.Util;
using Android.OS;
using Android.Views;
using Android.Widget;
using Microsoft.Xna.Framework;

namespace AndroidGame
{
    [Activity(
        Label = "Nightmare",
        MainLauncher = true,
        Icon = "@drawable/icon",
        AlwaysRetainTaskState = true,
        LaunchMode = LaunchMode.SingleInstance,
        ScreenOrientation = ScreenOrientation.Landscape,
        ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden |
                               ConfigChanges.ScreenSize | ConfigChanges.Navigation
    )]
    public class Activity1 : AndroidGameActivity
    {
        private Game1 _game;
        private View _view;
        
        public override void OnWindowFocusChanged(bool hasFocus)
        {
            base.OnWindowFocusChanged(hasFocus);

            if (hasFocus)
            {
                var uiOptions =
                    SystemUiFlags.HideNavigation |
                    SystemUiFlags.LayoutHideNavigation |
                    SystemUiFlags.LayoutFullscreen |
                    SystemUiFlags.Fullscreen |
                    SystemUiFlags.LayoutStable |
                    SystemUiFlags.ImmersiveSticky;

                Window.DecorView.SystemUiVisibility = (StatusBarVisibility)uiOptions;
            }
        } //чтобы не было видно навигацию
        
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            _game = new Game1();
            _view = _game.Services.GetService(typeof(View)) as View;

            SetContentView(_view);
            _game.Run();

        }
    }
}