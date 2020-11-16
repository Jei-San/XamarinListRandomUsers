using System.ComponentModel;
using Xamarin.Forms;
using XamarinRandomUsers.ViewModel;

namespace XamarinRandomUsers
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        //create instance of the view model
        MainPageModel viewmodel = new MainPageModel();
        public MainPage()
        {
            InitializeComponent();

            //Assign method to the text changed event
            SearchEntry.TextChanged += SearchEntry_TextChanged;

            //Give the context to the XAML with this
            BindingContext = viewmodel;
        }

        private void SearchEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            //Call the method from viewmodel when text is changed
            viewmodel.FilterUsers();
        }
    }
}
