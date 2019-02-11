using GalaSoft.MvvmLight.Command;
using Sales.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Sales.Services;
using Xamarin.Forms;
using Sales.Helpers;
using System.Linq;
using Sales.Views;

namespace Sales.ViewModels
{
    public class ProductItemViewModel : Product
    {
        #region Attributes
        private ApiService apiService;
        #endregion

        #region Constructor

        public ProductItemViewModel()
        {
            this.apiService = new ApiService();
        }

        #endregion

        #region Commands
        public ICommand DeleteProductCommand
        {
            get
            {
                return new RelayCommand(DeleteProduct);
            }
        }


        public ICommand EditProductCommand
        {
            get
            {
                return new RelayCommand(EditProduct);
            }
        }


        #endregion

        #region Methods
        private async void EditProduct()
        {
            MainViewModel.GetInstance().EditProduct = new EditProductViewModel(this);
            await App.Navigator.PushAsync(new EditProductPage());
        }

        private  async void DeleteProduct()
        {
            var answer = await Application.Current.MainPage.DisplayAlert(Languages.Confirm, Languages.DeleteConfirmation, Languages.Yes, Languages.No);

            if (!answer)
            {
                return;
            }
            var verifyConnection = await this.apiService.CheckConnection();

            if (!verifyConnection.IsSuccess)
            {

                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    verifyConnection.Message,
                    Languages.Accept);
                return;
            }

            var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.Delete(url, "/api", "/Products", this.ProductId, Settings.TokenType, Settings.AccessToken);
            if (!response.IsSuccess)
            {

                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    response.Message,
                    Languages.Accept);
                return;
            }

            var viewModel = ProductsViewModel.GetInstance();
            var deletedProduct = viewModel.Products.Where(p => p.ProductId == this.ProductId).FirstOrDefault();

            if (deletedProduct!= null)
            {
                viewModel.Products.Remove(deletedProduct);
            }

        }
        #endregion
    }
}
