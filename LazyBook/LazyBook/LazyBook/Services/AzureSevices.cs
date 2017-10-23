using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Xamarin.Forms;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.Sync;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using System.Diagnostics;
using LazyBook.Models;

namespace LazyBook.Services
{
    class AzureSevices
    {
        public MobileServiceClient Client { get; set; } = null;
        IMobileServiceSyncTable<Book> table;

        public async Task Initialize()
        {
            if (Client?.SyncContext?.IsInitialized ?? false)
                return;

            var appUrl = "http://lazybook.azurewebsites.net";

            Client = new MobileServiceClient(appUrl);

            var path = "syncstore_books.db";
            path = Path.Combine(MobileServiceClient.DefaultDatabasePath, path);

            var store = new MobileServiceSQLiteStore(path);

            store.DefineTable<Book>();

            await Client.SyncContext.InitializeAsync(store, new MobileServiceSyncHandler());

            table = Client.GetSyncTable<Book>();
        }

        public async Task SyncBooks()
        {
            try
            {
                await table.PullAsync("allBooks", table.CreateQuery());
                await Client.SyncContext.PushAsync();
            }
            catch(Exception e)
            {
                Debug.WriteLine("Unable to sync books " + e);
            }

        }

        public async Task<IEnumerable<Book>> GetBooks()
        {
            await Initialize();
            await SyncBooks();
            return await table.OrderBy(s => s.Name).ToEnumerableAsync();
        }

        public async Task AddBook(Book b)
        {
            await table.InsertAsync(b);

            //Synchronize coffee
            await SyncBooks();
        }
    }
}
