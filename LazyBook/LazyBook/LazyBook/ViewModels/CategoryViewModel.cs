using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazyBook.ViewModels
{
    public class CategoryViewModel : BaseViewModel
    {
        public CategoryViewModel(string catName)
        {
            Title = catName;
        }
    }
}
