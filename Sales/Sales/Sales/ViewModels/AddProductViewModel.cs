

namespace Sales.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using System.Windows.Input;
    using Xamarin.Forms;
    using Sales.Helpers;
    using Sales.Services;
    using Sales.Common.Models;
    using System.Linq;
    using Plugin.Media.Abstractions;
    using Plugin.Media;

    public class AddProductViewModel: BaseViewModel
    {
        #region Attibutes
        private MediaFile file;
        private ImageSource imageSource;
        private bool isRunning;       
        private bool isEnabled;
        private ApiService apiService;
        #endregion

        #region Properties
        public string Description { get; set; }
         public string Price { get; set; }
         public string Remarks { get; set; }
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

        #region Constructor 
        public AddProductViewModel()
        {
            this.apiService = new ApiService();
            this.isEnabled = true;
            this.ImageSource = "dummyp";


        }
        #endregion

        #region Command

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
            if (string.IsNullOrEmpty(this.Description))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error, 
                    Languages.DescriptionError, 
                    Languages.Accept);
                return;
            }

            if (string.IsNullOrEmpty(this.Price))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.PriceError,
                    Languages.Accept);
                return;
            }

            var price = decimal.Parse(this.Price);

            if (price<0)
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
            byte[] imageArray= null; 
            if(this.file!=null)
            {
                imageArray = FilesHelper.ReadFully(this.file.GetStream());
            }


            var product = new Product
            {
                Description= this.Description,
                Prrice= price,
                Remarks= this.Remarks,
                ImageArray= imageArray
            };

            var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.Post(url, "/api", "/Products", product, Settings.TokenType, Settings.AccessToken );
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
            viewModel.MyProducts.Add(newProduct);
            viewModel.RefreshList();
       
         
            this.IsRunning = false;
            this.IsEnabled = true;
            await App.Navigator.PopAsync();




        }
        #endregion


    }
}
