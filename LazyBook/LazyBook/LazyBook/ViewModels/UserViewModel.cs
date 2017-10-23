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

namespace LazyBook.ViewModels
{
    class UserViewModel
    {
        UserAzure azureServices;
        public ObservableCollection<User> Users { get; set; }
        public Command LoadUsersCommand { get; set; }
        User User { get; set; }
        public bool IsBusy { get; set; }

        public UserViewModel()
        {
            azureServices = DependencyService.Get<UserAzure>();
            Users = new ObservableCollection<User>();
            LoadUsersCommand = new Command(async () => await ExecuteLoadUsersCommand());
        }

        public async Task ExecuteLoadUsersCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Users.Clear();
                var users = await azureServices.GetUsers();
                foreach (var user in users)
                {
                    Users.Add(user);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("UserViewModel LoadUser Failure: " + ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task ExecuteAddUserCommand(User user)
        {
            if (IsBusy)
                return;
            try
            {
                User = user;
                var item = await azureServices.AddUser(User);
                Users.Add(user);


            }
            catch (Exception ex)
            {
                Debug.WriteLine("UserViewModel AddUser Failure: " + ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
