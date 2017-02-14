using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlogSite.Models.Entities
{
    public class BlogCategory : EntityBase
    {
        
        [Required]
        [StringLength(50)]
        public string Name { get; set; }


        public bool Enabled { get; set; }

        public ICollection<PostCategory> PostCategories { get; set; }

    }
}