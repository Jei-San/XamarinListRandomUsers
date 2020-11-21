using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;
using XamarinRandomUsers.Model;

namespace FavoriteUsers.Data
{
    public class FavoriteUsersDatabase
    {
        readonly SQLiteAsyncConnection _database;

        public FavoriteUsersDatabase(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<FavoriteUser>().Wait();
        }

        public Task<List<FavoriteUser>> GetFavoriteUsersAsync()
        {
            return _database.Table<FavoriteUser>().ToListAsync();
        }

        public Task<FavoriteUser> GetFavoriteUsersAsync(int id)
        {
            return _database.Table<FavoriteUser>()
                            .Where(i => i.ID == id)
                            .FirstOrDefaultAsync();
        }

        public Task<int> SaveFavUserAsync(FavoriteUser favoriteUser)
        {
            if (favoriteUser.ID != 0)
            {
                return _database.UpdateAsync(favoriteUser);
            }
            else
            {
                return _database.InsertAsync(favoriteUser);
            }
        }

        public Task<int> DeleteFavUserAsync(FavoriteUser favoriteUser)
        {
            return _database.DeleteAsync(favoriteUser);
        }
        public Task<int> DeleteAllFavUsersAsync()
        {
            return _database.DeleteAllAsync<FavoriteUser>();
        }
    }
}