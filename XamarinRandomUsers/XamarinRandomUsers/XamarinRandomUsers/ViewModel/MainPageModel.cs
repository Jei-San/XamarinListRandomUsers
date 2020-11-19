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
        public string UserInfo { get; set; }
        public Result User { get; set; }
        public bool IsError { get; set; }
        public bool IsLoaded { get; set; }



        public INavigation Navigation { get; set; }
        public ICommand SearchUsers { get; set; }
        public ICommand NameButton { get; set; }
        public ICommand EmailButton { get; set; }
        public ICommand DateButton { get; set; }
        public ICommand AddressButton { get; set; }

        private IList<Result> userList;

        public MainPageModel()
        {
            ShowUsers();
            NameButton = new Command(UserName);
            EmailButton = new Command(UserEmail);
            DateButton = new Command(UserDate);
            AddressButton = new Command(UserAddress);
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
            }
            else
            {
                IsError = false;
                IsLoaded = true;
                ShowUsers(NumUsers);
            }
            OnPropertyChanged(nameof(Message));
            OnPropertyChanged(nameof(IsError));
            OnPropertyChanged(nameof(IsLoaded));
        }
        private void UserName()
        {
            UserInfo = User.name.first;
            OnPropertyChanged(nameof(UserInfo));
        }
        private void UserEmail()
        {
            UserInfo = User.email;
            OnPropertyChanged(nameof(UserInfo));
        }
        private void UserDate()
        {
            UserInfo = $"{User.dob.date.Month}/{User.dob.date.Day}/{User.dob.date.Year}";
            OnPropertyChanged(nameof(UserInfo));
        }
        private void UserAddress()
        {
            UserInfo = $"{User.location.street.number}, {User.location.street.name}, {User.location.city}, {User.location.state}, {User.location.postcode}, {User.location.country}";
            OnPropertyChanged(nameof(UserInfo));
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
