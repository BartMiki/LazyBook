using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System.IO;
using Xamarin.Forms;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.Sync;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using System.Diagnostics;
using LazyBook.Models;

namespace LazyBook.Services
{
    class UserAzure : IDataStore<User>
    {
        public MobileServiceClient Client { get; set; } = null;
        IMobileServiceSyncTable<User> table;
        List<User> users;

        public async Task Initialize()
        {
            if (Client?.SyncContext?.IsInitialized ?? false)
                return;

            var appUrl = "http://lazybook.azurewebsites.net";

            Client = new MobileServiceClient(appUrl);

            var path = "syncstore_users.db";
            path = Path.Combine(MobileServiceClient.DefaultDatabasePath, path);

            //tworzenie lokalnej bazy danych
            var store = new MobileServiceSQLiteStore(path);
            //definiowanie tabeli
            store.DefineTable<Item>();

            //inicjalizownie synchronizacji
            await Client.SyncContext.InitializeAsync(store, new MobileServiceSyncHandler());

            //tabela polaczona z Azure
            table = Client.GetSyncTable<User>();
            users = await table.ToListAsync();
        }

        public async Task SyncBooks()
        {
            try
            {
                await table.PullAsync("allUsers", table.CreateQuery());

                await Client.SyncContext.PushAsync();

                users = await table.ToListAsync();
            }
            catch (Exception e)
            {
                Debug.WriteLine("Unable to sync users " + e);
            }

        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            await Initialize();
            await SyncBooks();
            return await table.OrderBy(s => s.UserName).ToEnumerableAsync();

        }

        public async Task<List<User>> GetUserList(User user)
        {
            return await table.Where((us) => us.Email == user.Email).ToListAsync();
        }

        public async Task<User> AddUser(User u)
        {
            await table.InsertAsync(u);
            //Synchronize Users
            await SyncBooks();
            return u;
        }

        public UserAzure()
        {
            users = new List<User>();
        }

        public async Task<bool> AddItemAsync(User user)
        {
            users.Add(user);
            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(User user)
        {
            var _item = users.Where((User arg) => arg.Id == user.Id).FirstOrDefault();
            users.Remove(_item);
            users.Add(user);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            var _item = users.Where((User arg) => arg.Id == id).FirstOrDefault();
            users.Remove(_item);

            return await Task.FromResult(true);
        }

        public async Task<User> GetItemAsync(string id)
        {
            return await Task.FromResult(users.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<User>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(users);
        }
    }
}
