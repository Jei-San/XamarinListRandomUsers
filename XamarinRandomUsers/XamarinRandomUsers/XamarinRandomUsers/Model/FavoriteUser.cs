using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace XamarinRandomUsers.Model
{
    public class FavoriteUser
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public DateTime Date { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string ImgSource { get; set; }
        public string Thumbnail { get; set; }
    }
    public static class FavoriteUserManager
    {
        public static Task<int> SaveFavoriteUserAsync(FavoriteUser favoriteUser)
        {
            return App.Database.SaveFavUserAsync(favoriteUser);
        }
        public static Task<List<FavoriteUser>> GetFavoriteUsersAsync()
        {
            return App.Database.GetFavoriteUsersAsync();
        }
        public static Task<int> DeleteFavoriteUserAsync(FavoriteUser favoriteUser)
        {
            return App.Database.DeleteFavUserAsync(favoriteUser);
        }
        public static Task<int> DeleteFavoriteUsersAsync()
        {
            return App.Database.DeleteAllFavUsersAsync();
        }
    }
}
