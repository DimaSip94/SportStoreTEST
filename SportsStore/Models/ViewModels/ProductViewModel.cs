using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Models.ViewModels
{
    public class ProductViewModel
    {
        public int ProductID { get; set; }
        [Required(ErrorMessage = "Please enter а product name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please enter а product description")]
        public string Description { get; set; }
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Please enter а product price")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "Please enter а product category")]
        public string Category { get; set; }
        public int CartLineID { get; set; }
        public int LogoID { get; set; }
        public IFormFile LogoFile { get; set; }
        public string LogoPath { get; set; }
    }
}
