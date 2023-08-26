
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace MyReviewWeb.Models
{
    public class Like
    {

        [Key]
        public int Id { get; set; }

        public int IdReview { get; set; }

        
        public string User { get; set; }
    }
}
