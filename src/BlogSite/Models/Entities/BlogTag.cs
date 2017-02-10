using System.ComponentModel.DataAnnotations;

namespace BlogSite.Models.Entities
{
    public class BlogTag : EntityBase
    {

        [Required]
        public string BlogPostId { get; set; }

        public BlogPost BlogPost { get; set; }


        [Required]
        [StringLength(50)]
        public string Name { get; set; }
    }
}