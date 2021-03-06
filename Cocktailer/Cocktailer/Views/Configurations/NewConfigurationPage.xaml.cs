﻿using Cocktailer.ViewModels.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Cocktailer.Views.Configurations
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewConfigurationPage : ContentPage
    {
        NewConfigurationViewModel ViewModel => BindingContext as NewConfigurationViewModel;
        public NewConfigurationPage()
        {
            InitializeComponent();
        }
    }
}