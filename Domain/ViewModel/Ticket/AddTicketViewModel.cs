using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enums;
using Microsoft.AspNetCore.Http;


namespace Domain.ViewModel.Ticket
{
    public class AddTicketViewModel
    {
        
        public int UserId { get; set; }
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر داشته باشد")]
        [Display(Name = "موضوع")] 
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Title { get; set; }
        public TicketsEnum.Section Section { get; set; }
        public TicketsEnum.Priority Priority { get; set; }
        [Display(Name = "پیام")] 
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Message { get; set; }

        public IFormFile? Image { get; set; }

    }
}
