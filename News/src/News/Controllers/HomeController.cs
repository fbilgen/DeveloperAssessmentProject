using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using News.Models.ViewModels;
using News.Models;
using News.Services;
using Microsoft.AspNetCore.Http;

namespace News.Controllers
{

    public class HomeController : Controller
    {
        //Dependency injection
        private readonly NewsContext _context;

        public HomeController(NewsContext context)
        {
            _context = context;
        }

        #region ACTION PAGES
        /// Get list of articles
        public IActionResult Index()
        {
            List<ArticleViewModel> articles = Helper.GetArticles(_context)
                                    .OrderByDescending(a => a.PublishDate).ToList();

            return View(articles);

        }

        public IActionResult Publish()
        {

            List<ArticleViewModel> articles = Helper.GetArticles(_context)
                                   .OrderByDescending(a => a.ArticleID).ToList();

            return View(articles);
        }

        public IActionResult ArticleDetail(int articleID)
        {
            //get selected article
            var article = Helper.GetArticles(_context)
                            .Where(a => a.ArticleID == articleID)
                            .Select(a => new ArticleDetailViewModel
                            {
                                ArticleID = a.ArticleID,
                                Title = a.Title,
                                Text = a.Text,
                                Author = a.Author,
                                EditorEmail = a.EditorEmail,
                                PublishDate = a.PublishDate,
                                IsLiked = false, //default
                            }).FirstOrDefault();

            //attach IsLiked and Comment properties here
            var username = HttpContext.Session.GetString("SessionKeyEmail");
            var articleLikeRecord = _context.ArticleLike.Where(a => a.ArticleID == articleID && a.EmployeeEmail == username).FirstOrDefault();
            if (articleLikeRecord != null)
            {
                article.IsLiked = articleLikeRecord.Like == null ? false : (bool)articleLikeRecord.Like;
                article.Comment = articleLikeRecord.Comment;
            }

            return View(article);
        }

        public IActionResult AddArticle()
        {
            return View();
        }

        public IActionResult EditArticle(int articleID)
        {
            //Get selected article from DB and fill the form on page
            var result = Helper.GetArticles(_context).Where(a => a.ArticleID == articleID).FirstOrDefault();
            if (result != null)
            {
                ArticleViewModel article = new ArticleViewModel()
                {
                    ArticleID = result.ArticleID,
                    Title = result.Title,
                    Text = result.Text,
                    Author = result.Author,
                    PublishDate = result.PublishDate
                };
                return View(article);
            }
            else
            {
                //Show error on page
                ViewData["Error"] = "There is no article with given ID";
                return View();
            }
        }

        public IActionResult Statistics()
        {
            return View();
        }
        public IActionResult About()
        {
            ViewData["Message"] = "Your about page.";

            return View();
        }
        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }
        public IActionResult Error()
        {
            return View();
        }

        #endregion

        #region Functions: READ DATA
        public JsonResult ArticleLikeChart_Read()
        {
            List<ArticleViewModel> articles = Helper.GetArticles(_context).ToList();

            //create chartdata.labels and chartdata.data initially.
            List<string> labels = new List<string>();
            List<int> data = new List<int>();

            foreach (ArticleViewModel awm in articles)
            {
                labels.Add(awm.ArticleID.ToString());  //since article titles might be long; use articleID. Can add titles to tooltip later
                data.Add(awm.LikeCount);
            }

            //Now create chartdata and return as Json object
            return Json(new
            {
                result = new
                {
                    labels = labels,
                    datasets = new[] { new { label = "Total Likes", data = data, backgroundColor ="#ed1234" } }
                }
            });

        }

        #endregion
        #region Functions:WRITE DATA
        [HttpPost]
        public IActionResult AddArticle(ArticleViewModel model)
        {
            //model validation check
            if (!ModelState.IsValid)
            {
                //sth wrong. Return to the page with model & error data
                return View("AddArticle", model);
            }

            //create new article with model data
            Article article = new Article
            {
                Title = model.Title.ToUpper(),
                Author = model.Author,
                Text = model.Text,
                PublishDate = model.PublishDate,
                EventDate = DateTime.Now,
                EditorEmail = HttpContext.Session.GetString("SessionKeyEmail")
            };

            //Add new article to database
            _context.Article.Add(article);
            _context.SaveChanges();

            //go back to Puclish page
            return RedirectToAction("Publish", "Home");

        }

        [HttpPost]
        public IActionResult EditArticle(ArticleViewModel model)
        {
            //model validation check
            if (!ModelState.IsValid)
            {
                //sth wrong. Return to the page with model & error data
                return View("EditArticle", model);
            }

            //get article and update its values
            var article = _context.Article.Where(a => a.ArticleID == model.ArticleID).FirstOrDefault();
            if (article != null)
            {
                article.Title = model.Title.ToUpper();
                article.Text = model.Text;
                article.Author = model.Author;
                article.PublishDate = model.PublishDate;
                article.EventDate = DateTime.Now;
                article.EditorEmail = HttpContext.Session.GetString("SessionKeyEmail");
            }

            _context.Update(article);
            _context.SaveChanges();

            //go back to Publish page
            return RedirectToAction("Publish", "Home");
        }

        [HttpPost]
        public JsonResult DeleteArticle(int articleID)
        {
            var article = _context.Article.Where(a => a.ArticleID == articleID).FirstOrDefault();

            if (article != null)
            {
                _context.Remove(article);
                _context.SaveChanges();

                //Note: It's not certain from assignment description that if we sould clear the ArticleLike data for deleted article.
                //Thus; did not implemented that part
            }

            return Json(new { result = true });
        }

        [HttpPost]
        public JsonResult LikeArticle(int articleID)
        {
            //prevent anonymous users from liking
            var username = HttpContext.Session.GetString("SessionKeyEmail");
            if (String.IsNullOrWhiteSpace(username))
            {
                return Json(new { result = false, errorMessage = "Please login first!" });
            }

            //check if user liked before?
            var isLiked = _context.ArticleLike.Where(a => a.ArticleID == articleID && a.EmployeeEmail == username && a.Like == true).Any();
            if (isLiked)
            {
                return Json(new { result = false, errorMessage = "You have already liked this article!" });
            }


            //exists check: maybe commented but not liked before
            var articleLike = _context.ArticleLike.Where(a => a.ArticleID == articleID && a.EmployeeEmail == username).FirstOrDefault();
            if (articleLike != null)
            {
                articleLike.Like = true;

                //update database
                _context.Update(articleLike);
                _context.SaveChanges();
            }
            else
            {
                //create new record for ArticleLike table
                ArticleLike entity = new ArticleLike
                {
                    ArticleID = articleID,
                    EmployeeEmail = username,
                    Like = true,
                    Comment = null
                };

                //update database
                _context.ArticleLike.Add(entity);
                _context.SaveChanges();
            }

            //return success message
            return Json(new { result = true });
        }

        [HttpPost]
        public JsonResult CommentArticle(int articleID, string comment)
        {
            //prevent anonymous users from liking
            var username = HttpContext.Session.GetString("SessionKeyEmail");
            if (String.IsNullOrWhiteSpace(username))
            {
                return Json(new { result = false, errorMessage = "Please login first!" });
            }


            //exists check: maybe liked but not commented before
            var articleLike = _context.ArticleLike.Where(a => a.ArticleID == articleID && a.EmployeeEmail == username).FirstOrDefault();
            if (articleLike != null)
            {
                //update comment
                articleLike.Comment = comment;

                //update database
                _context.Update(articleLike);
                _context.SaveChanges();
            }
            else
            {
                //create new record for ArticleLike table
                ArticleLike entity = new ArticleLike
                {
                    ArticleID = articleID,
                    EmployeeEmail = username,
                    Like = null,
                    Comment = comment
                };

                //update database
                _context.ArticleLike.Add(entity);
                _context.SaveChanges();
            }

            //return success message
            return Json(new { result = true });
        }



        #endregion


    }
}
