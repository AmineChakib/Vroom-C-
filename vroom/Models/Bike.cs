using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using vroom.Extentions;

namespace vroom.Models
{
    public class Bike
    {
        public int Id { get; set; }
        public Make Make { get; set; }
        //[RegularExpression("^[1-9]*$",ErrorMessage ="Select a Make")]
        [Range(1, int.MaxValue, ErrorMessage = "Select a Model")]
        public int MakeID { get; set; }
        public Model Model { get; set; }

        //[RegularExpression("^[1-9]*$",ErrorMessage ="Select a Model")]
        [Range(1, int.MaxValue, ErrorMessage = "Select a Model")]
        public int ModelID { get; set; }

        [Required(ErrorMessage ="Provide a Year")]
        [YearRange(1990,ErrorMessage ="Provide a valide year")]
        public int Year { get; set; }
        [Required(ErrorMessage = "Provide Mileage")]
        [Range(1,int.MaxValue,ErrorMessage = "Provide Mileage")]
        public int Mileage { get; set; }
        public string Features { get; set; }
        [Required(ErrorMessage ="Provide Seller Name")]
        public string SellerName { get; set; }
        [EmailAddress]
        public string SellerEmail { get; set; }
        [Required(ErrorMessage = "Provide Seller Phone")]
        public string SellerPhone { get; set; }
        [Required(ErrorMessage = "Provide Price")]
        [Range(1, int.MaxValue, ErrorMessage = "Provide Mileage")]
        public int Price { get; set; }
        [RegularExpression("^[A-Za-z]*$", ErrorMessage = "Select a Currency")]
        public string Currency { get; set; }
        public string ImagePath { get; set; }
    }
}
