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
using FavoriteUsers.Data;
using System.Collections.ObjectModel;

namespace XamarinRandomUsers.ViewModel
{
    
    public class MainPageModel : INotifyPropertyChanged
    {
        //Properties
        public ObservableCollection<Result> Results { get; private set; }
        public string Message { get; set; }
        public string Search { get; set; }
        public string NumUsers { get; set; }
        public string UserInfo { get; set; }
        public Result User { get; set; }
        public bool IsError { get; set; }
        public bool IsLoaded { get; set; }
        public string FavText { get; set; }

        private FavoriteUser favoritedUser;
        private bool notFav;

        //Commands
        public INavigation Navigation { get; set; }
        public ICommand SearchUsers { get; set; }
        public ICommand NameButton { get; set; }
        public ICommand EmailButton { get; set; }
        public ICommand DateButton { get; set; }
        public ICommand AddressButton { get; set; }
        public ICommand FavoriteButton { get; set; }
        public ICommand ShowFavorites { get; set; }
        public ICommand Clear { get; set; }
        public ICommand ClearDB { get; set; }
        private ObservableCollection<Result> userList;

        public MainPageModel()
        {
            ShowUsers();
            NameButton = new Command(UserName);
            EmailButton = new Command(UserEmail);
            DateButton = new Command(UserDate);
            AddressButton = new Command(UserAddress);
            SearchUsers = new Command(RetrieveUsers);
            FavoriteButton = new Command(FavButton);
            ShowFavorites = new Command(ShowFav);
            Clear = new Command(ClearUsers);
            ClearDB = new Command(clearDB);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        //Methods
        private void ShowUsers(string quantity = "50")
        {
            var response = RandomUsersApiClient.GetUsers(quantity);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                Users result = JsonConvert.DeserializeObject<Users>(response.Content);
                Results = new ObservableCollection<Result>(result.results);
                userList = new ObservableCollection<Result>(result.results);
                IsLoaded = true;
                IsError = false;
            }
            else
            {
                Message = $"Network Problems\nStatus Code: {response.StatusCode}";
                IsError = true;
                IsLoaded = false;
                ClearUsers();
            }
            OnPropertyChanged(nameof(Results));
        }
        public void FilterUsers()
        {
            var filteredUsers = new List<Result>();
            if (!string.IsNullOrEmpty(Search))
            {
                filteredUsers = Results.Where(u => u.name.first.ToLower().Contains(Search.ToLower())).ToList();
                Results = new ObservableCollection<Result>(filteredUsers);
            }
            else
                Results = userList;
            //When updating any property, we must tell the UI that it has changed with this method
            OnPropertyChanged(nameof(Results));
        }
        private void ShowFav()
        {
            ClearUsers();
            var favoritedUsers = FavoriteUserManager.GetFavoriteUsersAsync().Result;
            foreach (var u in favoritedUsers)
            {
                Results.Add(new Result
                {
                    name = new Name { first = u.Name, last = u.LastName },
                    email = u.Email,
                    location = JsonConvert.DeserializeObject<Location>(u.Address),
                    dob = new Dob { date = u.Date },
                    picture = new Picture { large = u.ImgSource, thumbnail = u.Thumbnail },
                    favorite = true,
                    FavUser = u
                });
            }
            OnPropertyChanged(nameof(Results));
        }
        private void ClearUsers()
        {
            if (Results != null)
                Results.Clear();
            if (userList != null)
                userList.Clear();
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
            UserInfo = $"{User.name.first} {User.name.last}";
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
        private void FavButton()
        {
            favoritedUser = new FavoriteUser()
            {
                ID = 0,
                Name = User.name.first,
                LastName = User.name.last,
                Email = User.email,
                Address = JsonConvert.SerializeObject(User.location),
                Date = User.dob.date,
                ImgSource = User.picture.large,
                Thumbnail = User.picture.thumbnail
            };
            FavoriteUserManager.SaveFavoriteUserAsync(favoritedUser);
            notFav = false;
            FavoriteBehaviour();
        }
        public async Task TapCommand(Result itemTapped)
        {
            User = itemTapped;
            favoritedUser = null;
            UserInfo = "";
            notFav = !User.favorite;
            FavoriteBehaviour();
            OnPropertyChanged(nameof(UserInfo));
            await Navigation.PushAsync(new InfoUserPage(this));
        }

        private void clearDB()
        {
            FavoriteUserManager.DeleteFavoriteUsersAsync();
        }
        private void unfavoriteUser()
        {
            if (favoritedUser != null)
            {
                FavoriteUserManager.DeleteFavoriteUserAsync(favoritedUser);
                favoritedUser = null;
            }
            else
            {
                FavoriteUserManager.DeleteFavoriteUserAsync(favoritedUser);
            }
            notFav = true;
            FavoriteBehaviour();
        }
        private void FavoriteBehaviour()
        {
            FavText = notFav ? "Favorite" : "Unfavorite";
            FavoriteButton = notFav ? new Command(FavButton) : new Command(unfavoriteUser);
            OnPropertyChanged(nameof(FavText));
            OnPropertyChanged(nameof(FavoriteButton));
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
