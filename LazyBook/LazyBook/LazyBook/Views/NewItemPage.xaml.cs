using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace LazyBook
{
    public partial class NewItemPage : ContentPage
    {
        public Item newItem { get; set; }

        public NewItemPage()
        {
            InitializeComponent();
            categoriesPicker.ItemsSource = Models.Helper.Categories;
            BindingContext = this;
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            if(titleEntry.Text == null || authorEntry.Text == null || publisherEntry.Text == null || yearEntry.Text == null || categoriesPicker.Items[categoriesPicker.SelectedIndex] == null || summaryEntry.Text == null)
            {
                return;
            }
            ItemsViewModel ivm = new ItemsViewModel();

            newItem = new Item();
            newItem.Name = titleEntry.Text;
            newItem.Author = authorEntry.Text;
            newItem.Publisher = publisherEntry.Text;
            newItem.Year = yearEntry.Text;
            newItem.Category = categoriesPicker.Items[categoriesPicker.SelectedIndex];
            newItem.Summary = summaryEntry.Text;
            newItem.Date = DateTime.UtcNow;
            newItem.Rate = 3.2f;

            await ivm.ExecuteAddItemCommandAsync(newItem);

            MessagingCenter.Send(this, "AddItem");
            await Navigation.PopToRootAsync();
        }
    }
}
