using ASP.Blog.Data;
using ASP.Blog.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASP.Blog.DAL.Repositories
{
    public class ArticleRepository : Repository<Article>
    {
        public ArticleRepository(BlogContext db) : base(db) { }
        public List<Article> GetArticles()
        {
            return Set.ToList();
        }
        public Article GetArticleById(int id)
        {
            return Set.Where(x => x.ID == id).FirstOrDefault();
        }
        public List<Article> GetArticleByUserId(string userId)
        {
            return Set.Where(x => x.UserId == userId).ToList();
        }
        public void AddArticle(Article article)
        {
            Set.Add(article);
        }
        public void UpdateArticle(Article article)
        {
            Update(article);
        }
        public void DeleteArticle(Article article)
        {
            Delete(article);
        }
    }
}
