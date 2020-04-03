using Cocktailer.Models.DataManagement;
using Cocktailer.Models.Entries;
using Cocktailer.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;

namespace Cocktailer.ViewModels
{
    public class CocktailModeViewModel : BaseViewModel
    {
        ObservableCollection<RecipeEntry> recipeEntries;
        public ObservableCollection<RecipeEntry> RecipeEntries
        {
            get => recipeEntries;
            set
            {
                recipeEntries = value;
                OnPropertyChanged();
            }
        }
        public CocktailModeViewModel(INavService navService) : base(navService)
        {
        }
        public override void Init()
        {
            
            RecipeEntries = new ObservableCollection<RecipeEntry>(new List<RecipeEntry>()
            {
                
                
            });
            var text = JsonConvert.SerializeObject(RecipeEntries[0]);
            Debug.WriteLine(text);
        }
    }
}
