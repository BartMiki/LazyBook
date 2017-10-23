using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LazyBook.Models;
using LazyBook.ViewModels;
using LazyBook.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LazyBook.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegistrationPage : ContentPage
    {
        public RegistrationPage()
        {
            InitializeComponent();
        }

        protected virtual void OnThresholdReached(EventArgs e)
        {
            EventHandler handler = ThresholdReached;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public event EventHandler ThresholdReached;

        async private void Button_Clicked(object sender, EventArgs e)
        {
            bool Success = true;

            var azure = new UserAzure();
            var users = azure.GetUsers();
            var userr = new User();

            foreach(var user in users.Result)
            {
                if(user.UserName == usernameEntry.Text)
                {
                    await DisplayAlert("Registration", "Username is already in use!", "OK");
                    usernameEntry.Text = String.Empty;
                    Success = false;
                    break;
                }
                if(user.Email == emailEntry.Text)
                {
                    await DisplayAlert("Registration", "E-mail is already in use!", "OK");
                    emailEntry.Text = String.Empty;
                    Success = false;
                    break;
                }
            }
            
            if(passwordEntry != password2Entry)
            {
                await DisplayAlert("Regitration", "Passwords don't match!", "OK");
                passwordEntry.Text = String.Empty;
                password2Entry.Text = String.Empty;
                Success = false;
            }
            if(Success == true)
            {
                userr.Email = emailEntry.Text;
                userr.Password = passwordEntry.Text;
                userr.UserName = usernameEntry.Text;
                await azure.AddUser(userr);
                await DisplayAlert("Registration", "Registration completed successfully!", "OK");
                OnThresholdReached(EventArgs.Empty);
            }
        }
    }
}