using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Net.Http;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using LazyBook.Models;
using LazyBook.Services;

namespace LazyBook.ViewModels
{
    class BooksViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Item> Books { get; set; }
        public BooksViewModel()
        {
            Books = new ObservableCollection<Item>();
            GetBooksCommand = new Command(
               async () => await GetBooks(),
               () => !IsBusy);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        private bool isBusy;

        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                OnPropertyChanged();
                //Update the can execute
                GetBooksCommand.ChangeCanExecute();
            }
        }

        private async Task GetBooks()
        {
            if (IsBusy)
                return;

            Exception error = null;
            try
            {
                IsBusy = true;

                IsBusy = true;

                var service = DependencyService.Get<MockDataStore>();
                var items = await service.GetBooks();

                Books.Clear();
                foreach (var item in items)
                    Books.Add(item);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error: " + ex);
                error = ex;
            }
            finally
            {
                IsBusy = false;
            }

            if (error != null)
                await Application.Current.MainPage.DisplayAlert("Error!", error.Message, "OK");
        }

        public Command GetBooksCommand { get; set; }
    }
}
