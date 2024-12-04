using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Account;
using Domain.Entities.Common;
using Domain.Enums;
using static Domain.Enums.TicketsEnum;

namespace Domain.Entities.Ticket
{
    public class Ticket:BaseEntity
    {
        [ForeignKey("User")]
        public int OwnerId { get; set; }

        public string Title { get; set; }

        public bool IsClosed { get; set; }

        public DateTime UpdateDate { get; set; }

        public Section Section { get; set; }

        public Priority Priority { get; set; }

        public Status Status { get; set; }

        #region relation

        public ICollection<TicketsMessage> TicketsMessages { get; set; }
        public User User { get; set; }

        #endregion
        
    }
}
