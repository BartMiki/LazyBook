using System;
using System.Windows.Input;

using Xamarin.Forms;

namespace LazyBook
{
    public class AboutViewModel : BaseViewModel
    {
        public AboutViewModel()
        {
            Title = "About";

            OpenWebCommand = new Command(() => Device.OpenUri(new Uri("https://github.com/devmood/LazyBook/tree/Production")));
        }

        public ICommand OpenWebCommand { get; }
    }
}