using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Common;

namespace Domain.Entities.Faq
{
    public class FaqCategory : BaseEntity
    {
        [MaxLength(200)]
        public string Title { get; set; }

        #region Relation

        public ICollection<FaqQuestion> FaqQuestions { get; set; }

        #endregion

    }
}
