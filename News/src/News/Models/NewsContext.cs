using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace News.Models
{
    public class NewsContext : DbContext
    {

        public NewsContext(DbContextOptions<NewsContext> options) : base(options)
        {

        }

        //Database Tables
        public DbSet<Article> Article { get; set; }
        public DbSet<ArticleLike> ArticleLike { get; set; }

        //no override for now
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }

    }
}
