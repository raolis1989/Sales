﻿using Plugin.Settings;
using Plugin.Settings.Abstractions;


namespace Sales.Helpers
{
    public static class Settings
    {
        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        private const string tokenType = "TokeType";
        private const string accessToken = "AccessToken";
        private const string isRemembered = "IsRemembered";
        private static readonly string stringDefault = string.Empty;
        private static readonly bool booleanDefault = false;
        private const string userASP = "UserSP";


        public static string TokenType
        {
            get
            {
                return AppSettings.GetValueOrDefault(tokenType, stringDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(tokenType, value);

            }

        }


        public static string AccessToken
        {
            get
            {
                return AppSettings.GetValueOrDefault(accessToken, stringDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(accessToken, value);

            }

        }

        public static bool IsRemembered
        {
            get
            {
                return AppSettings.GetValueOrDefault(isRemembered, booleanDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(isRemembered, value);

            }

        }

        public static string UserASP
        {
            get
            {
                return AppSettings.GetValueOrDefault(userASP, stringDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(userASP, value);

            }

        }


    }
}
