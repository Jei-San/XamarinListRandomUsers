using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinRandomUsers.ViewModel;

namespace XamarinRandomUsers.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InfoUserPage : ContentPage
    {
        MainPageModel viewModel = new MainPageModel();
        public InfoUserPage(MainPageModel viewModel)
        {
            InitializeComponent();
            this.viewModel = viewModel;
            BindingContext = this.viewModel;
        }
    }
}