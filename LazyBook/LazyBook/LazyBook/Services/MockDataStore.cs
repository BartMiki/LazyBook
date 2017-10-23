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

[assembly: Xamarin.Forms.Dependency(typeof(LazyBook.MockDataStore))]
namespace LazyBook
{
    public class MockDataStore : IDataStore<Item>
    {
        public MobileServiceClient Client { get; set; } = null;
        IMobileServiceSyncTable<Item> table;

        public async Task Initialize()
        {
            if (Client?.SyncContext?.IsInitialized ?? false)
                return;

            var appUrl = "http://lazybook.azurewebsites.net";

            Client = new MobileServiceClient(appUrl);

            var path = "syncstore_books2.db";
            path = Path.Combine(MobileServiceClient.DefaultDatabasePath, path);

            //tworzenie lokalnej bazy danych
            var store = new MobileServiceSQLiteStore(path);
            //definiowanie tabeli
            store.DefineTable<Item>();
   
            //inicjalizownie synchronizacji
            await Client.SyncContext.InitializeAsync(store, new MobileServiceSyncHandler());

            //tabela polaczona z Azure
            table = Client.GetSyncTable<Item>();
        }

        public async Task SyncBooks()
        {
            try
            {
                await table.PullAsync("allBooks", table.CreateQuery());

                await Client.SyncContext.PushAsync();
            }
            catch (Exception e)
            {
                Debug.WriteLine("Unable to sync books " + e);
            }

        }

        public async Task<IEnumerable<Item>> GetBooks()
        {
            await Initialize();
            await SyncBooks();

            return await table.OrderBy(s => s.Name).ToEnumerableAsync();
            
        }

        public async Task<Item> AddBook(Item b)
        {
            await table.InsertAsync(b);

            //Synchronize coffee
            await SyncBooks();
            return b;
        }

        public async Task<List<Item>> GetListOfCategories(String category)
        {
            var itemsSerach = await table.Where((it) => it.Category == category).ToListAsync();
            return itemsSerach;
        }

        public async Task<List<Item>> GetListSortedByAge()
        {
            var itemsSerach = await table.ToListAsync();
            itemsSerach.Sort((Item a, Item b) =>  b.Date.CompareTo(a.Date));
            return itemsSerach;
        }


        public async Task<List<Item>> GetListSortedByRate()
        {
            var itemsSerach = await table.ToListAsync();
            itemsSerach.Sort((Item a, Item b) => b.Rate.CompareTo(a.Rate));
            return itemsSerach;
        }
        //-------------------------------------------------------------------------------------------------------

        List<Item> items;

        public MockDataStore()
        {
            items = new List<Item>();
        }

        public async Task<bool> UpdateItemAsync(Item item)
        {
            var _item = items.Where((Item arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(_item);
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            var _item = items.Where((Item arg) => arg.Id == id).FirstOrDefault();
            items.Remove(_item);

            return await Task.FromResult(true);
        }

        public async Task<Item> GetItemAsync(string id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<Item>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(items);
        }

        public Task<bool> AddItemAsync(Item item)
        {
            throw new NotImplementedException();
        }
    }
}
