using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace News.Models.ViewModels
{
    public class ArticleDetailViewModel
    {
        public int ArticleID { get; set; }

        [Display(Name = "Publish Date")]
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

        //attach user data to viewModel
        public bool IsLiked { get; set; }

        [StringLength(300)]
        public string Comment { get; set; }


    }
}
