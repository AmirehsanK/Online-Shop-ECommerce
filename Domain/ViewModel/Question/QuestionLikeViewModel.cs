
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Question;

namespace Domain.ViewModel.Question
{
    public class QuestionLikeViewModel
    {

        public string Question { get; set; }

        public string? Answer { get; set; }



        public int LikeCount { get; set; }

        public int DissLikeCount { get; set; }
         
        
 
        public QuestionLike QuestionLike { get; set; }

    }
}
