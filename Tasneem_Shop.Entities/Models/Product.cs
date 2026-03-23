using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Tasneem_Shop.Entities.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please Enter Product Name")]
        public string Name { get; set; }
        public string Description { get; set; }

        public string Img { get; set; }
        [Required(ErrorMessage = "Please Enter Product Price")]
        public decimal Price { get; set; }

        public int CategoryId { get; set; }
        public Category? Category { get; set; }

    }
}
