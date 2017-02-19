using System.Collections.Generic;
using BlogSite.Models.Entities;

namespace BlogSite.Models.BlogPostViewModels
{
    public class EditBlogPostViewModel
    {

        public EditBlogPostViewModel()
        {
            PostCategoryViewModels = new List<PostCategoryViewModel>();
        }


        public BlogPost BlogPost { get; set; }


        public IList<PostCategoryViewModel> PostCategoryViewModels { get; private set; }
        
    }
    
}