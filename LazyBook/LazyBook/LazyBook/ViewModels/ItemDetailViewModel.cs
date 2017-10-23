using System;

namespace LazyBook
{
    public class ItemDetailViewModel : BaseViewModel
    {
        public Item Item { get; set; }
        public ItemDetailViewModel(Item item = null)
        {
            Title = item?.Author;
            Item = item;
        }
    }
}
