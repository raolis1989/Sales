namespace Sales.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using Sales.Common.Models;
    using Sales.Helpers;
    using Sales.Services;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;
    using Xamarin.Forms;

    public class ProductsViewModel :BaseViewModel
    {
        #region Attributes
        private string filter; 

        private ApiService apiService;

        private bool isRefreshing;

        private ObservableCollection<Product> products;
        #endregion

        #region Properties

        public string Filter
        {
            get { return this.filter; }
            set { this.filter = value;
                this.RefreshList();
                }
        }
       
        public List<Product> MyProducts { get; set; }
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
        #endregion

        #region Constructor

        public ProductsViewModel()
        {
            instance = this;
            this.apiService = new ApiService();
            this.LoadProducts();
        }
        #endregion

        #region Singleton
        private static ProductsViewModel instance;

        public static ProductsViewModel GetInstance()
        {
            if (instance==null)
            {
                return new ProductsViewModel();
            }
            return instance;
        }
        #endregion

        #region Methods

        private async void LoadProducts()
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
           
            var response = await this.apiService.GetList<Product>(url, "/api", "/Products", Settings.TokenType, Settings.AccessToken);

            if(!response.IsSuccess)
            {
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert(Languages.Error, response.Message, "Accept");
                return;
            }
            this.MyProducts = (List<Product>)response.Result;
            this.RefreshList();

            
            this.IsRefreshing = false;
        }


        public void RefreshList()
        {
            if (string.IsNullOrEmpty(this.Filter))
            {
                var mylistProductItemViewModel = MyProducts.Select(products => new ProductItemViewModel
                {
                    ProductId = products.ProductId,
                    Description = products.Description,
                    ImageArray = products.ImageArray,
                    ImagePath = products.ImagePath,
                    IsAvailable = products.IsAvailable,
                    Prrice = products.Prrice,
                    PublishOn = products.PublishOn,
                    Remarks = products.Remarks
                });
                this.Products = new ObservableCollection<Product>(
                    mylistProductItemViewModel.OrderBy(p => p.Description));
            }
            else
            {
                var mylistProductItemViewModel = MyProducts.Select(products => new ProductItemViewModel
                {
                    ProductId = products.ProductId,
                    Description = products.Description,
                    ImageArray = products.ImageArray,
                    ImagePath = products.ImagePath,
                    IsAvailable = products.IsAvailable,
                    Prrice = products.Prrice,
                    PublishOn = products.PublishOn,
                    Remarks = products.Remarks
                }).Where(p => p.Description.ToLower().Contains(this.Filter.ToLower())).ToList() ;
                this.Products = new ObservableCollection<Product>(
                    mylistProductItemViewModel.OrderBy(p => p.Description));
            }

           
        }

        #endregion

        #region Commands

        public  ICommand SearchCommand
        {
            get
            {
                return new RelayCommand(LoadProducts);
            }
        }
        public ICommand RefreshCommand {

            get
            {
                return new RelayCommand(RefreshList);
            }

        }
        #endregion


    }
}
