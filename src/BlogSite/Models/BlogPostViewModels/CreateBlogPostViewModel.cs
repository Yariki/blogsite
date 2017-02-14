using System.Collections.Generic;
using BlogSite.Models.Entities;

namespace BlogSite.Models.BlogPostViewModels
{
    public class CreateBlogPostViewModel
    {
        public BlogPost BlogPost { get; set; }

        public List<BlogCategory> BlogCategories { get; set; }
    }
}