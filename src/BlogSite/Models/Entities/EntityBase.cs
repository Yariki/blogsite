using System;
using System.ComponentModel.DataAnnotations;

namespace BlogSite.Models.Entities
{
    public abstract class EntityBase
    {

        public EntityBase()
        {
        }

        [Key]
        public string ID { get; set; }



        [DataType(DataType.Date)]
        public DateTime Created { get; set; }


        [DataType(DataType.Date)]
        public DateTime? Modified { get; set; }


    }
}