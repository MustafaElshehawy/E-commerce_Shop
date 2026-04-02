using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Tasneem_Shop.Entities.Models;

namespace Tasneem_Shop.Entities.ViewModels
{
    public class UserVM
    {
        public ApplicationUser User { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } 

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; } 

        public string Role { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> RoleList { get; set; }
    }
}
