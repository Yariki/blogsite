using BlogSite.Models.Entities;

namespace BlogSite.Models.BlogPostViewModels
{
    public class PostCategoryViewModel
    {
        public BlogCategory BlogCategory { get; set; }

        public bool IsChecked { get; set; }
    }
}