using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using Xamarin.Forms;
using XamarinRandomUsers.Model;
using XamarinRandomUsers.ViewModel;

namespace XamarinRandomUsers
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        //create instance of the view model
        MainPageModel viewModel = new MainPageModel();
        public MainPage()
        {
            InitializeComponent();

            //Give navigation to viewmodel
            viewModel.Navigation = Navigation;

            //Assign method to the text changed event
            SearchEntry.TextChanged += SearchEntry_TextChanged;
            UserList.ItemTapped +=  async (s, e) => { await UserList_ItemTappedAsync(e); };

            //Give the context to the XAML with this
            BindingContext = viewModel;
        }

        private async System.Threading.Tasks.Task UserList_ItemTappedAsync(ItemTappedEventArgs e)
        {
            await viewModel.TapCommand((Result)e.Item);
        }

        private void SearchEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            //Call the method from viewmodel when text is changed
            viewModel.FilterUsers();
        }
    }
}
