using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace MyReviewWeb.Models
{
    public class Review
    {
        [Key]
        public int Id { get; set; }

        [Required] 
        public string Title { get; set; }

        [Required]
        public string Text { get; set; }

        [Required]
        public string Link { get; set; }

        [DisplayName("Time")]
        public DateTime CreateDateTime { get; set; } = DateTime.Now;

        public string User { get; set; } = "";

    }
}
