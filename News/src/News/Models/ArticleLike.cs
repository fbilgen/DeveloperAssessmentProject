using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace News.Models
{
    public class ArticleLike
    {
        public int ArticleLikeID { set; get; }

        public int ArticleID { set; get; }

        //Ideally this is EmployeeID but we don't have an authentication storage
        [Required]
        public string EmployeeEmail { get; set; }

        public bool? Like { get; set; }

        [StringLength(300)]
        public string Comment { get; set; }

    }
}
