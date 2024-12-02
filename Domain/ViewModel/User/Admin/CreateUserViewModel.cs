using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModel.User.Admin
{
    public class CreateUserViewModel
    {
        [MaxLength(200)]
        [Display(Name = "نام")] 

        public string? FirstName { get; set; }
        [Display(Name = " نام خانوادگی")] 
        [MaxLength(200)]
        public string? LastName { get; set; }

        [MaxLength(200)]
     
        public string Email { get; set; }
        [MaxLength(200)]

   
        public string Password { get; set; }
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر داشته باشد")]
        [Display(Name = "")] 
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string EmailActiveCode { get; set; }

        public bool IsAdmin { get; set; }
        public bool IsEmailActive { get; set; }   
   
    }
}
