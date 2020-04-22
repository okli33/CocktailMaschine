
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Cocktailer.Droid.Modules;
using Cocktailer.Views;
using Xamarin.Forms;

namespace Cocktailer.Droid
{
    [Activity(Label = "Cocktailer", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            Forms.SetFlags("SwipeView_Experimental");
            Forms.SetFlags("CarouselView_Experimental");
            Forms.SetFlags("IndicatorView_Experimental");
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App(new CocktailerPlatformModule()));
            MessagingCenter.Subscribe<CocktailmodePage>(this, "setLandscapeMode", sender =>
                {
                    RequestedOrientation = ScreenOrientation.Landscape;
                });
            MessagingCenter.Subscribe<CocktailmodePage>(this, "setPortraitMode", sender =>
                {
                    RequestedOrientation = ScreenOrientation.Portrait;
                });
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}