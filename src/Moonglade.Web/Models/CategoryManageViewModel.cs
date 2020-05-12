using Moonglade.Model;
using System.Collections.Generic;

namespace Moonglade.Web.Models
{
    public class CategoryManageViewModel
    {
        public CategoryEditViewModel CategoryEditViewModel { get; set; }

        public IReadOnlyList<Model.Category> Categories { get; set; }

        public CategoryManageViewModel()
        {
            CategoryEditViewModel = new CategoryEditViewModel();
            Categories = new List<Category>();
        }
    }
}
