using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModel.ContactUs
{
    public class ContactUsAdminViewModel
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string FullName { get; set; }
        public bool IsAnswered { get; set; }
    }
}
