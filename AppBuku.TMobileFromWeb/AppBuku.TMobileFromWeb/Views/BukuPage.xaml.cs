using AppBuku.TMobileFromWeb.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppBuku.TMobileFromWeb.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BukuPage : ContentPage
    {
        BukuViewModel _viewModel;

        public BukuPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new BukuViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}