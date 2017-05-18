using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace News.Models.ViewModels
{
    public class ArticleViewModel
    {
        public int ArticleID { get; set; }

        [Display(Name ="Publish Date")]
        [Required(ErrorMessage = "Please select {0}")]
        [DataType(DataType.Date)]
        public DateTime PublishDate { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Text { get; set; }

        [Required]
        public string Author { get; set; }
        public string EditorEmail { get; set; }

        public DateTime EventDate { get; set; }
        public int LikeCount { get; set; }
        public int CommentCount { get; set; }

    }
}
