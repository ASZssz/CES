using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Runtime;
using Microsoft.Xna.Framework;
using Celeste.Core;

namespace Celeste.Android
{
    [Activity(
        Label = "@string/app_name",
        MainLauncher = true,
        AlwaysRetainTaskState = true,
        LaunchMode = LaunchMode.SingleInstance,
        ScreenOrientation = ScreenOrientation.FullUser,
        ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden | ConfigChanges.ScreenSize
    )]
    public class Activity1 : Activity
	{
		private Game1 _game;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Initialize platform and logging
			ServiceLocator.RegisterPlatformService(new AndroidPlatformService(this));
			ServiceLocator.RegisterLogSystem(new AndroidLogSystem(this));

			ServiceLocator.LogSystem.Log("=== CELESTE ANDROID START ===");

			_game = new Game1();
			SetContentView(_game.Services.GetService(typeof(View)) as View);
			_game.Run();
		}

		protected override void OnPause()
		{
			base.OnPause();
			_game?.Dispose();
			ServiceLocator.LogSystem?.Log("Activity paused");
		}

		protected override void OnResume()
		{
			base.OnResume();
			
			// Reaplicar fullscreen e modo imersivo
			ApplyFullscreenMode();
			
			ServiceLocator.LogSystem?.Log("Activity resumed");
		}

		protected override void OnWindowFocusChanged(bool hasFocus)
		{
			base.OnWindowFocusChanged(hasFocus);
			
			if (hasFocus)
			{
				ApplyFullscreenMode();
			}
		}

		private void ApplyFullscreenMode()
		{
			// Fullscreen imersivo sticky (hide system UI even on interaction)
			if (Build.VERSION.SdkInt >= BuildVersionCodes.Kitkat)
			{
				var immersiveFlags = (SystemUiFlags)0x00001000 | // FLAG_IMMERSIVE_STICKY
					SystemUiFlags.HideNavigation | 
					SystemUiFlags.HideSystemUi |
					SystemUiFlags.LayoutHideNavigation |
					SystemUiFlags.LayoutFullscreen |
					SystemUiFlags.Fullscreen;

				Window.DecorView.SystemUiVisibility = (StatusBarVisibility)immersiveFlags;
			}
			else
			{
				// Fallback para older Android
				var flags = SystemUiFlags.HideNavigation | SystemUiFlags.HideSystemUi;
				Window.DecorView.SystemUiVisibility = (StatusBarVisibility)flags;
			}
		}
	}
}
