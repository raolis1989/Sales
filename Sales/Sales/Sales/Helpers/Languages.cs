﻿namespace Sales.Helpers
{
    using Xamarin.Forms;
    using Interfaces;
    using Resources;

    public static class Languages
    {
        static Languages()
        {
            var ci = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
            Resource.Culture = ci;
            DependencyService.Get<ILocalize>().SetLocale(ci);
        }

        public static string Accept
        {
            get { return Resource.Accept; }
        }

        public static string Error
        {
            get { return Resource.Error; }
        }

        public static string NoInternet
        {
            get { return Resource.NotInternet; }
        }

        public static string Products
        {
            get { return Resource.Products; }
        }

        public static string TurnOnInternet
        {
            get { return Resource.TurnOnInternet; }
        }


        public static string AddProduct
        {
            get
            {
                return Resource.AddProduct;
            }
        }
        public static string Description
        {
            get { return Resource.DescriptionPlaceHolder; }
        }
        public static string Price
        {
            get { return Resource.Price; }
        }

        public static string PricePlaceHolder
        {
            get { return Resource.PricePlaceHolder; }
        }
        public static string Remarks
        {
            get { return Resource.Remarks; }
        }
        public static string Save
        {
            get { return Resource.Save; }
        }




    }
}
