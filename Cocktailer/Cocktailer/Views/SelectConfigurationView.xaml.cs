﻿using Cocktailer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Cocktailer.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SelectConfigurationView : ContentPage
    {
        SelectConfigurationViewModel ViewModel => BindingContext as SelectConfigurationViewModel;
        public SelectConfigurationView()
        {
            InitializeComponent();
        }
    }
}