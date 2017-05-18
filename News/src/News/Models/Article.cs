using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace News.Models
{
    public class Article
    {
        public int ArticleID { set; get; }
        public string Title { get; set; }
        public string Text { get; set; }
        public string Author { get; set; }

        //Ideally this is EditorID/EmployeeID but we don't have an authentication storage
        public string EditorEmail { get; set; }
        public DateTime PublishDate { get; set; }
        public DateTime EventDate { get; set; }

        ////navigation properties
        //public Editor Editor { get; set; }
    }
}
