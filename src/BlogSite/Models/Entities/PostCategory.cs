using System.Collections.Generic;

namespace BlogSite.Models.Entities
{
    public class PostCategory : EntityBase
    {
        public string BlogPostId { get; set; }

        public string BlogCategoryId { get; set; }


        public BlogPost BlogPost { get; set; }
        
        public BlogCategory BlogCategory { get; set; }

    }
}