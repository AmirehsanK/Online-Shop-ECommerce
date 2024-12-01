using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Common;

namespace Domain.Entities.Account
{
    public class Users:BaseEntity
    {
        public string? FirstName { get; set; }

        public string? LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string EmailActiveCode { get; set; }

        public bool IsAdmin { get; set; }
        public bool IsEmailActive { get; set; }
    }
}
