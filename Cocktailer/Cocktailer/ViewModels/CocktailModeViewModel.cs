using Cocktailer.Exceptions;
using Cocktailer.Models.Entries;
using Cocktailer.Services;
using Microsoft.Runtime.CompilerServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Pipes;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Cocktailer.ViewModels
{
    public class CocktailModeViewModel : BaseViewModel<ConfigurationEntry>
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

        private RecipeEntry selectedEntry;
        public RecipeEntry SelectedEntry
        {
            get => selectedEntry;
            set
            {
                selectedEntry = value;
                OnPropertyChanged();
            }
        }

        IMemoryService memService;
        IBluetoothCommunicationService btService;
        IAlertMessageService alertService;
        ConfigurationEntry Config;
        ILogService logService;
        public CocktailModeViewModel(INavService navService, IMemoryService memService,
            IBluetoothCommunicationService btService, IAlertMessageService alertService,
            ILogService logService) : base(navService)
        {
            this.memService = memService;
            this.btService = btService;
            this.alertService = alertService;
            this.logService = logService;
        }
        public void CloseBluetoothConnection()
        {
            btService.TryCloseConnection();
        }
        public override async void Init(ConfigurationEntry config)
        {
            Config = config;
            try
            {
                if (!await btService.Init())
                {
                    throw new Exception();
                }
            }
            catch (Exception)
            {
                await alertService.ShowErrorMessage("Konnte keine Bluetooth Verbindung aufbauen");
                await NavService.NavigateTo<MainViewModel>();
                NavService.ClearBackStack();
            }

            RecipeEntries = await CompareConfigurationToRecipes();
            if (RecipeEntries.Count == 0)
            {
                await alertService.ShowAlertMessage("Keine verfügbaren Cocktails gefunden");
            }
            else
            {
                SelectedEntry = RecipeEntries[0];
            }
            IsBusy = false;
        }
        private async Task<ObservableCollection<RecipeEntry>> CompareConfigurationToRecipes()
        {
            List<RecipeEntry> possibleRecipes = new List<RecipeEntry>();
            List<RecipeEntry> recipes = null;
            int counter = 0;
            while (recipes == null && counter < 3)
            {
                try
                {
                    recipes = await memService.GetAvailable<RecipeEntry>();
                }
                catch
                {
                    counter++;
                }
            }
            var configurationDrinks = Config.Spots.Where(x => x.Drink != null)
                .Select(x => x.Drink).ToList();
            foreach (var recipe in recipes)
            {
                var possible = true;
                foreach (var ingredient in recipe.Ingredients.Select(x => x.Drink))
                {
                    try
                    {
                        if (!configurationDrinks.Where(x => (x.Brand == "" || x.Brand == ingredient.Brand)
                            && x.Name == ingredient.Name).Any())
                        {
                            possible = false;
                            break;
                        }
                    }
                    catch (Exception)
                    {
                        await alertService.ShowErrorMessage("Es ist ein Fehler"
                            + " mit der Konfiguration aufgetreten."
                            + "\n\n Du wirst zurück ins Haupmenü geleitet.");
                        await NavService.NavigateTo<MainViewModel>();
                        NavService.ClearBackStack();
                        return new ObservableCollection<RecipeEntry>();
                    }
                }
                if (possible)
                    possibleRecipes.Add(recipe);
            }
            return new ObservableCollection<RecipeEntry>(possibleRecipes);
        }

        private Command sendRecipeCommand;
        public Command SendRecipeCommand => sendRecipeCommand ?? (sendRecipeCommand =
            new Command(async () => await SendRecipe()));

        private async Task SendRecipe()
        {
            IsBusy = true;
            if (SelectedEntry == null)
            {
                await alertService.ShowErrorMessage("Wähle zuerst einen Cocktail aus du Dunkop");
                return;
            }

            try
            {
                string recipe = "";
                try
                {
                    recipe = ConvertRecipeToSendString();
                }
                catch (Exception ex)
                {
                    await alertService.ShowErrorMessage(ex.Message + "\n\nVorgang wird abgebrochen");
                    return;
                }
                string answer = "";
                try
                {
                    answer = await btService.Write(recipe);
                }
                catch (TimeoutException)
                {
                    //await alertService
                    //    .ShowErrorMessage("Timeout!\n\nDrücke beim nächsten Mal den Knopf schneller");
                    return;
                }
                catch (SendMessageException)
                {
                    await alertService.ShowAlertMessage("Fehler beim übertragen der Daten\n\nVorgang wird abgebrochen");
                    Init();
                    return;
                }
                catch (ReceiveMessageException)
                {
                    await alertService.ShowAlertMessage("Keine Antwort empfangen, überprüfe ob alles"
                            + " richtig geklappt hat");
                    await logService.AddToLogFile(new LogEntry()
                    { Cocktail = SelectedEntry, Date = DateTime.Now });
                    return;
                }
                catch (Exception)
                {
                    await alertService.ShowAlertMessage("Fehler beim übertragen der Daten\n\nVorgang wird abgebrochen");
                    return;
                }
                await logService.AddToLogFile(new LogEntry()
                    { Cocktail = SelectedEntry, Date = DateTime.Now });
                if (answer == "1")
                    return;
                else if (answer.StartsWith("0"))
                {
                    var pos = answer.Substring(1).Replace("&", "");
                    var DrinkPos = SpotConversions.FirstOrDefault(x => x.Value == pos).Key;
                    var emptyDrink = Config.Spots.FirstOrDefault(x => x.X.ToString() + x.Y.ToString() == DrinkPos);
                    await alertService.ShowErrorMessage("Folgendes Getränk ist leer und sollte ausgetauscht werden:"
                        + "\r\n\r\n" + emptyDrink.Drink.Brand + " " + emptyDrink.Drink.Name);
                    return;
                }
                else
                {
                    await alertService.ShowAlertMessage("Keine Antwort empfangen, überprüfe ob alles"
                        + " richtig geklappt hat");
                }

            }
            catch { }
            finally
            {
                IsBusy = false;
            }
        }

        private string ConvertRecipeToSendString()
        {
            string sendString = "&";
            foreach (var ingredient in SelectedEntry.Ingredients)
            {
                var spot = Config.Spots
                    .Where(x => x.Drink.Brand == ingredient.Drink.Brand
                    && x.Drink.Name == ingredient.Drink.Name).FirstOrDefault();
                sendString += SpotConversions[spot.X.ToString() + spot.Y.ToString()]
                    + ":" + ingredient.Amount.ToString() + "&";
            }
            return sendString;
        }

        private readonly Dictionary<string, string> SpotConversions = new Dictionary<string, string>()
        {
            { "00", "32" },{ "10", "36" },{ "20", "23" },{ "30", "29" },
            { "01", "28" },{ "11", "26" },{ "21", "27" },{ "31", "37" },
            { "02", "34" },{ "12", "24" },{ "22", "33" },{ "32", "25" },
            { "03", "30" },{ "13", "22" },{ "23", "31" },{ "33", "35" }
        };
    }
}
