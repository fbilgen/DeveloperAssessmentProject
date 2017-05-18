using News.Models;
using News.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace News.Services
{
    /// <summary>
    /// This is a helper class containing general purpose functions which 
    /// are used in various controllers.
    /// </summary>
    public static class Helper
    {
        internal static IQueryable<ArticleViewModel> GetArticles(NewsContext _context)
        {
            var query1 = (from a in _context.Article
                         select new
                         {
                             ArticleID = a.ArticleID,
                             Title = a.Title,
                             Text = a.Text,
                             Author = a.Author,
                             PublishDate = a.PublishDate,
                             EventDate = a.EventDate,
                             EditorEmail = a.EditorEmail,
                         });



            var query2 = (from al in _context.ArticleLike
                          group al by al.ArticleID into g
                          select new
                          {
                              ArticleID = g.Key,
                              ArticleLikes = g.Where(c =>c.Like == true).Count(),
                              ArticleComments = g.Where(c => !String.IsNullOrWhiteSpace(c.Comment)).Count() //do not include empty,null,whitespace
                          });



            var articles = (from x in query1
                            join y in query2
                            on x.ArticleID equals y.ArticleID into grp
                            from xy in grp.DefaultIfEmpty(new { ArticleID = x.ArticleID, ArticleLikes = 0, ArticleComments = 0 })
                            select new ArticleViewModel
                            {
                                ArticleID = x.ArticleID,
                                Title = x.Title,
                                Text = x.Text,
                                Author = x.Author,
                                PublishDate = x.PublishDate,
                                EventDate = x.EventDate,
                                EditorEmail = x.EditorEmail,
                                LikeCount = xy.ArticleLikes,
                                CommentCount = xy.ArticleComments
                            });

            return articles;

        }
    }
}
