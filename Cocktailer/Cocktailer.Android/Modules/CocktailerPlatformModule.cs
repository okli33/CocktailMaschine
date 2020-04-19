using Cocktailer.Droid.Services;
using Cocktailer.Services;
using Ninject.Modules;

namespace Cocktailer.Droid.Modules
{
    class CocktailerPlatformModule : NinjectModule
    {
        public override void Load()
        {
            IBluetoothCommunicationService btService = new BluetoothCommunicationService();
            Bind<IBluetoothCommunicationService>().ToMethod(x => btService).InSingletonScope();
        }
    }
}