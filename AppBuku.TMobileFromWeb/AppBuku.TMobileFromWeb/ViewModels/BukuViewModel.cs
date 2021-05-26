using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms;
using System.Net.Http;
using Newtonsoft.Json;
using AppBuku.Models;
using System.Threading.Tasks;
using AppBuku.TMobileFromWeb.Views;
using AppBuku.TMobileFromWeb.ViewModels;
using System.Collections.ObjectModel;

namespace AppBuku.TMobileFromWeb.ViewModels
{
    public class BukuViewModel: BaseViewModel
    {        
        public BukuViewModel()
        {
            Title = "Daftar Buku";            
            LoadItemsCommand = new Command(async () => await PerformCmdReloadCmdasync());            

            string baseUri = Application.Current.Properties["BaseWebUri"] as string;
            myHttpClient = new Services.MyHttpClient(baseUri);
        }

        // ---------------------------------------------------------------------------------------- //

        Services.MyHttpClient myHttpClient;
        public Command LoadItemsCommand { get; }

        private string hasilGet;
        public string HasilGet { get => hasilGet; set => SetProperty(ref hasilGet, value); }

        private Buku bukuGet;
        public Buku BukuGet { get => bukuGet; set => SetProperty(ref bukuGet, value); }

        private List<Buku> daftarBuku;
        public List<Buku> DaftarBuku { get => daftarBuku; set => SetProperty(ref daftarBuku, value); }

        // ---------------------------------------------------------------------------------------- //
        
        // Load //
        public void OnAppearing()
        {
            IsBusy = true;
        }

        // Reload //
        private ICommand cmdReload;
        public ICommand CmdReload
        {
            get
            {
                if (cmdReload == null)
                {
                    cmdReload = new Command(async () => await PerformCmdReloadCmdasync());
                }
                return cmdReload;
            }
        }
        private async Task PerformCmdReloadCmdasync()
        {
            if (!myHttpClient.IsEnable)
            {
                HasilGet = "HTTP Client disable";
                return;
            }
            IsBusy = true;
            try
            {
                string hsl = await myHttpClient.HttpGet("api/XBuku");
                HasilGet = hsl;
                var aLists = JsonConvert.DeserializeObject<List<Buku>>(hsl);
                DaftarBuku = aLists;
                foreach (Buku listform in aLists)
                {
                    BukuGet = listform;
                }
            }
            catch (Exception ex)
            {
                HasilGet = "ERROR: " + ex.Message;
            }
            finally
            {
                IsBusy = false;
            }
        }

        // Pilih Buku //
        public Buku selectedBuku;
        public Buku SelectedBuku
        {
            get => selectedBuku;
            set
            {
                SetProperty(ref selectedBuku, value);
                PerformBukuTapped(value);
            }
        }
        private Command<Buku> bukuTapped;
        public Command<Buku> BukuTapped
        {
            get
            {
                if (bukuTapped == null)
                {
                    bukuTapped = new Command<Buku>(PerformBukuTapped);
                }

                return bukuTapped;
            }
        }
        async void PerformBukuTapped(Buku item)
        {
            if (item == null)
                return;

            await Shell.Current.GoToAsync(
                $"{nameof(DataPage)}?{nameof(DataViewModel.TheId)}={item.Id}");
        }        
    }
}
