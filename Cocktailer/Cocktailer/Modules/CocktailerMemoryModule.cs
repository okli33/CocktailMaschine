using Cocktailer.Services;
using Ninject.Modules;

namespace Cocktailer.Modules
{
    public class CocktailerMemoryModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IMemoryService>().To<MemoryService>().InThreadScope();
        }
    }
}
