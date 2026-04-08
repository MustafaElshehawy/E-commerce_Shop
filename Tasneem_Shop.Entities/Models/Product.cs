using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        [DisplayName("Image")]
        [ValidateNever]
        public string Img { get; set; }
        [Required(ErrorMessage = "Please Enter Product Price")]
        public decimal Price { get; set; }
        [Required]
        [DisplayName("Category")]
        public int CategoryId { get; set; }


        [ValidateNever]
        public Category? Category { get; set; }

        [DisplayName("Hot Deal")]
        public bool IsHotDeal { get; set; } = false;
        [DisplayName("Offer Price")]
        public decimal? OfferPrice { get; set; }
        [Display(Name = "Offer End Date")]
        public DateTime? EndTime { get; set; }

        [ValidateNever]
        public List<ProductImage> ProductImages { get; set; }


    }
}
