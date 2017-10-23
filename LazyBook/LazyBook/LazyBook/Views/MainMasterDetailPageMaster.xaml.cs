using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LazyBook.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainMasterDetailPageMaster : ContentPage
    {
        public ListView ListView;

        public MainMasterDetailPageMaster()
        {
            InitializeComponent();

            BindingContext = new MainMasterDetailPageMasterViewModel();
            ListView = MenuItemsListView;
        }

        class MainMasterDetailPageMasterViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<MainMasterDetailPageMenuItem> MenuItems { get; set; }

            public MainMasterDetailPageMasterViewModel()
            {
                MenuItems = new ObservableCollection<MainMasterDetailPageMenuItem>(new[]
                {
                    new MainMasterDetailPageMenuItem { Id = 0, Title = "Books", TargetType = typeof(BooksTabbedPage) },
                    new MainMasterDetailPageMenuItem { Id = 1, Title = "Waiting room", TargetType = typeof(BooksTabbedPage) },
                    new MainMasterDetailPageMenuItem { Id = 4, Title = "Settings",  TargetType = typeof(AboutPage) },
                    new MainMasterDetailPageMenuItem { Id = 4, Title = "About",TargetType = typeof(AboutPage) }
                });
            }

            #region INotifyPropertyChanged Implementation
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                if (PropertyChanged == null)
                    return;

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion
        }
    }
}