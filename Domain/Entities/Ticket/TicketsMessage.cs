using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Account;
using Domain.Entities.Common;

namespace Domain.Entities.Ticket
{
    public class TicketsMessage:BaseEntity
    {
        [ForeignKey("Ticket")]
        public int TicketId { get; set; }
        public int Text { get; set; }
        [ForeignKey("User")]
        public int SenderId { get; set; }

        #region Relation

        public Ticket? Ticket { get; set; }

        public User? User { get; set; }

        #endregion




    }
}
