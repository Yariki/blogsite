using System;
using System.ComponentModel.DataAnnotations;

namespace BlogSite.Models.Entities
{
    public abstract class EntityBase
    {

        public EntityBase()
        {
            ID = Guid.NewGuid().ToString();
            Created = DateTime.Now;
        }

        [Required]
        [Key]
        public string ID { get; set; }

        [Required]
        public DateTime Created { get; set; }


        public DateTime? Modified { get; set; }


    }
}