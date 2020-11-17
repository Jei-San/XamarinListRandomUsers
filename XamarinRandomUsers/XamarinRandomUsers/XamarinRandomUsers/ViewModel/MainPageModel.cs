using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using XamarinRandomUsers.Model;
using XamarinRandomUsers.View;

namespace XamarinRandomUsers.ViewModel
{
    
    public class MainPageModel : INotifyPropertyChanged
    {
        public IList<Result> Results { get; private set; }
        public string Message { get; set; }
        public string Search { get; set; }
        public string NumUsers { get; set; }
        public Result User { get; set; }
        public bool IsError { get; set; }
        public bool IsLoaded { get; set; }
        public INavigation Navigation { get; set; }
        public ICommand SearchUsers { get; set; }

        private IList<Result> userList;

        public MainPageModel()
        {
            ShowUsers();
            SearchUsers = new Command(RetrieveUsers);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void ShowUsers(string quantity = "50")
        {
            var response = RandomUsersApiClient.GetUsers(quantity);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                Users result = JsonConvert.DeserializeObject<Users>(response.Content);
                Results = result.results;
                userList = result.results;
                IsLoaded = true;
                IsError = false;
            }
            else
            {
                Message = $"Network Problems\nStatus Code: {response.StatusCode}";
                IsError = true;
                IsLoaded = false;
                Results.Clear();
                userList.Clear();
            }
            OnPropertyChanged(nameof(Results));
        }
        public void FilterUsers()
        {
            var filteredUsers = new List<Result>();
            if (!string.IsNullOrEmpty(Search))
            {
                filteredUsers = Results.Where(u => u.name.first.ToLower().Contains(Search.ToLower())).ToList();
                Results = filteredUsers;
            }
            else
                Results = userList;
            //When updating any property, we must tell the UI that it has changed with this method
            OnPropertyChanged(nameof(Results));
        }
        private void RetrieveUsers()
        {
            if (string.IsNullOrEmpty(NumUsers))
            {
                Message = "Please enter a number";
                IsError = true;
                IsLoaded = false;
                OnPropertyChanged(nameof(Message));
                OnPropertyChanged(nameof(IsError));
                OnPropertyChanged(nameof(IsLoaded));
            }
            else
            {
                ShowUsers(quantity: NumUsers);
            }
        }

        public async Task TapCommand(Result itemTapped)
        {
            User = itemTapped;
            await Navigation.PushAsync(new InfoUserPage(this));
        }

        private void OnPropertyChanged(string propertyName)
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
