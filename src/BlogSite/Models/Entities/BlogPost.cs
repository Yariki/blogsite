using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlogSite.Models.Entities
{
    public class BlogPost : EntityBase
    {
        [Required(ErrorMessage = "*")]
        [StringLength(100)]
        public string Title { get; set; }

        [Required(ErrorMessage = "*")]
        [StringLength(1000)]
        public string Article { get; set; }
        
        public string ApplicationUserId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
        
        public int Views { get; set; }

        public ICollection<BlogPostComment> BlogPostComments { get; set; }

    }
}