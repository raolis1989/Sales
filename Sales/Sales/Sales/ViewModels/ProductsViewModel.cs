namespace Sales.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using Sales.Common.Models;
    using Sales.Helpers;
    using Sales.Services;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using Xamarin.Forms;

    public class ProductsViewModel :BaseViewModel
    {
        private ApiService apiService;

        private bool isRefreshing;

        private ObservableCollection<Product> products;
        public ObservableCollection<Product> Products
        {

            get { return this.products; }
            set { this.SetValue(ref this.products, value); }

        }
        public bool IsRefreshing
        {
            get { return this.isRefreshing; }
            set { this.SetValue(ref this.isRefreshing, value); }

        }

        public ProductsViewModel()
        {
            this.apiService = new ApiService();
            this.LoadProducts();
        }

        private  async void LoadProducts()
        {
            var verifyConnection = await this.apiService.CheckConnection();

            if(!verifyConnection.IsSuccess)
            {
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert(Languages.Error, verifyConnection.Message, Languages.Accept);
                return;

            }



            this.IsRefreshing = true;
            var url = Application.Current.Resources["UrlAPI"].ToString();
           
            var response = await this.apiService.GetList<Product>(url, "/api", "/Products");

            if(!response.IsSuccess)
            {
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert(Languages.Error, response.Message, "Accept");
                return;
            }
            var list = (List<Product>)response.Result;
            this.Products = new ObservableCollection<Product>(list);
            this.IsRefreshing = false;
        }
        public ICommand RefreshCommand {

            get
            {
                return new RelayCommand(LoadProducts);
            }

        }


    }
}
