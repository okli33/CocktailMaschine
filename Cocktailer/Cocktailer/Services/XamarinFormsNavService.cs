using Cocktailer.Services;
using Cocktailer.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

[assembly: Dependency(typeof(XamarinFormsNavService))]
namespace Cocktailer.Services
{
    public class XamarinFormsNavService : INavService
    {
        public INavigation XamarinFormsNav { get; set; }

        public bool CanGoBack => XamarinFormsNav.NavigationStack != null 
            && XamarinFormsNav.NavigationStack.Count > 0;

        readonly IDictionary<Type, Type> map = new Dictionary<Type, Type>();

        public event PropertyChangedEventHandler CanGoBackChanged;

        public void RegisterViewMapping(Type viewModel, Type view)
        {
            map.Add(viewModel, view);
        }

        public async Task GoBack()
        {
            await XamarinFormsNav.PopAsync(true);
            OnCanGoBackChanged();
        }

        public async Task NavigateTo<TVM>() where TVM : BaseViewModel
        {
            await NavigateToView(typeof(TVM));
            if (XamarinFormsNav.NavigationStack.Last().BindingContext is BaseViewModel)
            {
                ((BaseViewModel)XamarinFormsNav.NavigationStack.Last().BindingContext).Init();
            }
        }

        public async Task NavigateTo<TVM, TParameter>(TParameter parameter) where TVM : BaseViewModel
        {
            await NavigateToView(typeof(TVM));
            if (XamarinFormsNav.NavigationStack.Last().BindingContext is BaseViewModel)
            {
                ((BaseViewModel<TParameter>)XamarinFormsNav.NavigationStack
                    .Last().BindingContext).Init(parameter);
            }
        }

        public void RemoveLastView()
        {
            if (XamarinFormsNav.NavigationStack.Count < 2)
                return;
            var lastView = XamarinFormsNav.NavigationStack[XamarinFormsNav.NavigationStack.Count - 2];
            XamarinFormsNav.RemovePage(lastView);
        }

        public void ClearBackStack()
        {
            if (XamarinFormsNav.NavigationStack.Count < 2)
                return;
            for (var i = 0; i < XamarinFormsNav.NavigationStack.Count; i++)
            {
                XamarinFormsNav.RemovePage(XamarinFormsNav.NavigationStack[i]);
            }
        }

        public void NavigateToUri(Uri uri)
        {
            if (uri == null)
                throw new ArgumentException("Invalid URI");
            Launcher.TryOpenAsync(uri);
        }

        async Task NavigateToView(Type viewModelType)
        {
            if (!map.TryGetValue(viewModelType, out Type viewType))
                throw new ArgumentException("No view found in view mapping for " + viewModelType.FullName);
            var constructor = viewType.GetTypeInfo().DeclaredConstructors.FirstOrDefault(
                dc => !dc.GetParameters().Any());
            var view = constructor.Invoke(null) as Page;

            await XamarinFormsNav.PushAsync(view, true);
        }

        void OnCanGoBackChanged() => CanGoBackChanged?
            .Invoke(this, new PropertyChangedEventArgs("CanGoBack"));
    }
}
