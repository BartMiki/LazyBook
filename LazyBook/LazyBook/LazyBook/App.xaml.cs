using System;
using LazyBook.Views;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace LazyBook
{
    public partial class App : Application
    {
        bool logged = true;
        LoggingView loggingView;
        MainMasterDetailPage masterDetail;

        public App()
        {
            InitializeComponent();
            loggingView = new LoggingView();
            masterDetail = new MainMasterDetailPage();
            if (logged)
            {
                Current.MainPage = masterDetail;
            }
            else
            {
                Current.MainPage = loggingView;
            }
        }

        private void LoggedHandler(object sender, EventArgs e)
        {
            logged = true;
            Current.MainPage = masterDetail;
        }

        protected override void OnStart()
        {
            loggingView.ThresholdReached += LoggedHandler;

            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}