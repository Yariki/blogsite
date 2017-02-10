using System.ComponentModel.DataAnnotations;

namespace BlogSite.Models.Entities
{
    public class BlogPostComment : EntityBase
    {
        public string BlogPostId { get; set; }

        [Required(ErrorMessage = "*")]
        [StringLength(250)]
        public string Comment { get; set; }

        public string ApplicationUserId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
        
    }
}