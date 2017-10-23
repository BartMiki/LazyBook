using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.TextToSpeech;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LazyBook.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ReaderPage : ContentPage
	{
        ItemDetailViewModel viewModel;

        // Note - The Xamarin.Forms Previewer requires a default, parameterless constructor to render a page.
        public ReaderPage()
        {
            InitializeComponent();

            var item = new Item();

            viewModel = new ItemDetailViewModel(item);
            BindingContext = viewModel;
        }

        public ReaderPage(ItemDetailViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;
        }

        private void ReaderButton_Clicked(object sender, EventArgs e)
        {
            CrossTextToSpeech.Current.Speak(this.viewModel.Item.Summary);
        }
    }
}