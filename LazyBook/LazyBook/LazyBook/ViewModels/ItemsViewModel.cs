using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Net.Http;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;
using LazyBook.Models;
using LazyBook.Services;

namespace LazyBook
{
    public class ItemsViewModel
    {
        MockDataStore azureServices;
        public ObservableCollection<Item> Items { get; set; }
        public Command LoadItemsCommand { get; set; }
        Item Item { get; set; }
        public bool IsBusy {get;set; }
        public ItemsViewModel()
        {
            azureServices = DependencyService.Get<MockDataStore>();
            Items = new ObservableCollection<Item>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
        }

        public ItemsViewModel(string category)
        {
            azureServices = DependencyService.Get<MockDataStore>();
            Items = new ObservableCollection<Item>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand(category));
        }

        public async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Items.Clear();
                var items = await azureServices.GetBooks();
                foreach (var item in items)
                {
                    Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task ExecuteLoadItemsCommand(string category)
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Items.Clear();
                var items = await azureServices.GetListOfCategories(category);
                foreach(var _item in items)
                {
                    Items.Add(_item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task ExecuteAddItemCommandAsync(Item it)
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Item = it;
                var item = await azureServices.AddBook(Item);
                Items.Add(item);

                
            }catch(Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task ExecuteSortByAgeAsync()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Items.Clear();
                var items = await azureServices.GetListSortedByAge();
                foreach (var _item in items)
                {
                    Items.Add(_item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task ExecuteSortByRankAsync()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Items.Clear();
                var items = await azureServices.GetListSortedByRate();
                foreach (var _item in items)
                {
                    Items.Add(_item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
