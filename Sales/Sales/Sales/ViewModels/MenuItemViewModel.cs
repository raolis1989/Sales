﻿using GalaSoft.MvvmLight.Command;
using Sales.Helpers;
using Sales.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Sales.ViewModels
{
    public class MenuItemViewModel
    {
        public string Icon { get; set; }
        public string Title { get; set; }
        public string PageName { get; set; }


        #region Commands 
        public ICommand GotoCommand
        {
            get
            {
                return new RelayCommand(Goto);
            }
        }

        private void Goto()
        {
            if (this.PageName=="LoginPage")
            {

                Settings.AccessToken = string.Empty;
                Settings.TokenType = String.Empty;
                Settings.IsRemembered = false;

                MainViewModel.GetInstance().Login = new LoginViewModel(); 
                Application.Current.MainPage = new NavigationPage(new LoginPage());

            }
        }

        #endregion

    }
}
