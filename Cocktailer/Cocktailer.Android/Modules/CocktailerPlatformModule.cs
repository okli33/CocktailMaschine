using Cocktailer.Droid.Services;
using Cocktailer.Services;
using Ninject.Modules;

namespace Cocktailer.Droid.Modules
{
    class CocktailerPlatformModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IBluetoothCommunicationService>()
                .To<BluetoothCommunicationService>()
                .InSingletonScope();
        }
    }
}