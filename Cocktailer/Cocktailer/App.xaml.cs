using Cocktailer.Modules;
using Cocktailer.Services;
using Cocktailer.ViewModels;
using Cocktailer.Views;
using Ninject;
using Ninject.Modules;
using Xamarin.Forms;

namespace Cocktailer
{
    public partial class App : Application
    {
        public IKernel Kernel { get; set; }
        public App(params INinjectModule[] platformModules)
        {
            InitializeComponent();
            Kernel = new StandardKernel();
            Kernel.Load(new CocktailerCoreModule());
            Kernel.Load(new CocktailerNavModule());
            Kernel.Load(new CocktailerMemoryModule());
            Kernel.Load(platformModules);
            SetMainPage();
        }

        void SetMainPage()
        {
            var mainPage = new NavigationPage(new MainPage())
            {
                BindingContext = Kernel.Get<MainViewModel>()
            };
            var navService = Kernel.Get<INavService>() as XamarinFormsNavService;
            navService.XamarinFormsNav = mainPage.Navigation;
            var memService = Kernel.Get<IMemoryService>() as MemoryService;
            MainPage = mainPage;
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
