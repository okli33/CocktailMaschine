using System;
using System.Collections.Generic;
using System.Text;

namespace Cocktailer.Models
{
    public enum MenuItemType
    {
        Cocktails, 
        Recipes,
        Configuration
    }
    public class HomeMenuItem
    {
        public MenuItemType Id { get; set; }

        public string Title { get; set; }
    }
}
