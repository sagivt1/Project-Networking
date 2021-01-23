using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace Networking_Project.Models
{
    public class CreditCard
    {
        [DisplayName("Card Holder Name")]
        [Required]
        public string FullName { get; set; }
        [DisplayName("Card Number")]
        [Required]
        [CreditCard]
        public String CardNumber { get; set; }
        [DisplayName("3 Digit At The Back")]
        [StringLength(3)]
        [Required]
        public string DigitAtBack { get; set; }
        [DisplayName("Exp Month")]
        [Required]
        [Range(1,12)]
        public int Month { get; set; }
        [DisplayName("Exp Year")]
        [Required]
        [Range(2021,2030)]
        public int Year { get; set; }
        [DisplayName("Email For Recption")]
        [Required]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "Bad email format")]
        public string Email { get; set; }
    }
}