using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LazyBook.Models;
using LazyBook.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LazyBook.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BooksTabbedPage : TabbedPage
    {
        ItemsViewModel viewModel;

        public BooksTabbedPage()
        {
            InitializeComponent();
            this.Title = this.CurrentPage.Title;
            BindingContext = viewModel = new ItemsViewModel();
            CategoriesListView.ItemsSource = Helper.Categories;

            this.CurrentPageChanged += CurrentPageHasChanged;
        }


        protected void CurrentPageHasChanged(object sender, EventArgs e)
        {
            this.Title = this.CurrentPage.Title;
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as Item;
            if (item == null)
                return;

            await Navigation.PushAsync(new ItemDetailPage(new ItemDetailViewModel(item)));

            // Manually deselect item
            ItemsListView.SelectedItem = null;
        }

        async void OnBookSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as string;
            if (item == null)
                return;
            await Navigation.PushAsync(new CategoryView(item));

            // Manually deselect item
            CategoriesListView.SelectedItem = null;
        }

        async void AddButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NewItemPage());
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Items.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }
    }
}