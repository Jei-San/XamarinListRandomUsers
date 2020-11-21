using FavoriteUsers.Data;
using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XamarinRandomUsers
{
    public partial class App : Application
    {
        static FavoriteUsersDatabase database;

        public static FavoriteUsersDatabase Database
        {
            get
            {
                if (database == null)
                {
                    database = new FavoriteUsersDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "FavoriteUsers.db3"));
                }
                return database;
            }
        }

        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
