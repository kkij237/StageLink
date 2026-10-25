using Android.App;
using Android.Content.PM;
using Android.OS;
using Microsoft.Maui.Storage;

namespace StageLink;

// Name fixé pour pouvoir lancer l'app par adb :
//   adb shell am start -n com.stagelink.app/com.stagelink.app.MainActivity --ez demo true
[Activity(
	Name = "com.stagelink.app.MainActivity",
	Theme = "@style/Maui.SplashTheme",
	MainLauncher = true,
	LaunchMode = LaunchMode.SingleTop,
	ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode |
	                       ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
	protected override void OnCreate(Bundle? savedInstanceState)
	{
		base.OnCreate(savedInstanceState);

		// Extra "demo" => la visite guidée de 30 s démarre seule (voir scripts/record-demo.sh)
		if (Intent?.GetBooleanExtra("demo", false) == true)
		{
			Preferences.Default.Set("demo_autostart", true);
		}
	}
}
