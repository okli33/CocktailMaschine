using System;
using System.Collections.Generic;
using System.Text;

namespace Cocktailer.Models.Entries
{
    public class LogEntry : IEntry
    {
        private string name;
        public string Name { 
            get => Date.ToString("s") + "-" + Cocktail.ToString() + "\r\n"; 
            set { name = value; } }
        public RecipeEntry Cocktail { get; set; }
        public DateTime Date { get; set; }
    }
}
