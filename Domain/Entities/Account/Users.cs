using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Common;

namespace Domain.Entities.Account
{
    public class Users:BaseEntity
    {

        [MaxLength(200)]
        public string? FirstName { get; set; }

        [MaxLength(200)]
        public string? LastName { get; set; }

        [MaxLength(200)]
     
        public string Email { get; set; }
        [MaxLength(200)]

   
        public string Password { get; set; }
        public string EmailActiveCode { get; set; }

        public bool IsAdmin { get; set; }
        public bool IsEmailActive { get; set; }
    }
}
