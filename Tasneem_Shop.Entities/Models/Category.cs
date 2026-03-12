using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Tasneem_Shop.Entities.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage ="Please Enter Gategory Name")]
        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime CreatedTime { get; set; } =DateTime.Now;
   

    }
}
