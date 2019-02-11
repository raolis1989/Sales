

using GalaSoft.MvvmLight.Command;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Sales.Common.Models;
using Sales.Helpers;
using Sales.Services;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace Sales.ViewModels
{
    public class EditProductViewModel : BaseViewModel
    {
        #region Attributes
        private Product product;
        private MediaFile file;
        private ImageSource imageSource;
        private bool isRunning;
        private bool isEnabled;
        private ApiService apiService;

        #endregion

        #region  Properties
        public Product Product
        {
            get { return this.product; }
            set { this.SetValue(ref this.product, value); }
        }
        public bool IsRunning
        {
            get { return this.isRunning; }
            set { this.SetValue(ref this.isRunning, value); }
        }
        public bool IsEnabled
        {
            get { return this.isEnabled; }
            set { this.SetValue(ref this.isEnabled, value); }
        }

        public ImageSource ImageSource
        {
            get { return this.imageSource; }
            set { this.SetValue(ref this.imageSource, value); }
        }
        #endregion


        public EditProductViewModel(Product product)
        {
            this.product = product;
            this.apiService = new ApiService();
            this.isEnabled = true;
            this.imageSource = Product.ImageFullPath;
            
        }

        #region Command

        public ICommand DeleteProductCommand
        {
            get
            {
                return new RelayCommand(DeleteProduct);
            }
        }

        public ICommand ChangeImageCommand
        {
            get
            {
                return new RelayCommand(ChangeImage);
            }
        }

        private async void ChangeImage()
        {
            await CrossMedia.Current.Initialize();

            var source = await Application.Current.MainPage.DisplayActionSheet(
                Languages.ImageSource,
                Languages.Cancel,
                null,
                Languages.FromGallery,
                Languages.NewPicture);

            if (source == Languages.Cancel)
            {
                this.file = null;
                return;
            }

            if (source == Languages.NewPicture)
            {
                this.file = await CrossMedia.Current.TakePhotoAsync(
                    new StoreCameraMediaOptions
                    {
                        Directory = "Sample",
                        Name = "test.jpg",
                        PhotoSize = PhotoSize.Small,
                    }
                );
            }
            else
            {
                this.file = await CrossMedia.Current.PickPhotoAsync();
            }

            if (this.file != null)
            {
                this.ImageSource = ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();
                    return stream;
                });
            }
        }

        public ICommand SaveCommand
        {
            get
            {
                return new RelayCommand(Save);
            }
        }

        private async void Save()
        {
            if (string.IsNullOrEmpty(this.Product.Description))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.DescriptionError,
                    Languages.Accept);
                return;
            }


            if (this.Product.Prrice < 0)
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.PriceError,
                    Languages.Accept);
                return;
            }

            this.IsRunning = true;
            this.IsEnabled = false;


            var verifyConnection = await this.apiService.CheckConnection();

            if (!verifyConnection.IsSuccess)
            {
                this.IsRunning = false;
                this.IsEnabled = true;
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    verifyConnection.Message,
                    Languages.Accept);
                return;

            }
            byte[] imageArray = null;
            if (this.file != null)
            {
                imageArray = FilesHelper.ReadFully(this.file.GetStream());
                this.Product.ImageArray = imageArray;
            }

            var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.Put(url, "/api", "/Products", this.product, this.product.ProductId, Settings.TokenType, Settings.AccessToken);
            if (!response.IsSuccess)
            {
                this.IsRunning = false;
                this.IsEnabled = true;
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    response.Message,
                    Languages.Accept);
                return;
            }

            var newProduct = (Product)response.Result;
            var viewModel = ProductsViewModel.GetInstance();
            var oldProduct = viewModel.MyProducts.Where(p => p.ProductId == this.Product.ProductId).FirstOrDefault();
            if(oldProduct!=null)
            {
                viewModel.MyProducts.Remove(oldProduct);
            }
            viewModel.MyProducts.Add(newProduct);
            viewModel.RefreshList();


            this.IsRunning = false;
            this.IsEnabled = true;
            await App.Navigator.PopAsync();




        }

        private async void DeleteProduct()
        {
            var answer = await Application.Current.MainPage.DisplayAlert(Languages.Confirm, Languages.DeleteConfirmation, Languages.Yes, Languages.No);

            if (!answer)
            {
                return;
            }

            this.IsRunning = true;
            this.IsEnabled = false;

            var verifyConnection = await this.apiService.CheckConnection();

            if (!verifyConnection.IsSuccess)
            {
                this.IsRunning = false;
                this.IsEnabled = true;
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    verifyConnection.Message,
                    Languages.Accept);
                return;
            }

            var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.Delete(url, "/api", "/Products", this.Product.ProductId, Settings.TokenType, Settings.AccessToken);
            if (!response.IsSuccess)
            {

                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    response.Message,
                    Languages.Accept);
                return;
            }

            var viewModel = ProductsViewModel.GetInstance();
            var deletedProduct = viewModel.MyProducts.Where(p => p.ProductId == this.Product.ProductId).FirstOrDefault();

            if (deletedProduct != null)
            {
                viewModel.MyProducts.Remove(deletedProduct);
            }

            viewModel.RefreshList();

            this.IsRunning = false;
            this.IsEnabled = true;
            await App.Navigator.PopAsync();
        }
        #endregion
    }
}
