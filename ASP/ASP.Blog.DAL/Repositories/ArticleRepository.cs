using ASP.Blog.Data;
using ASP.Blog.Data.Entities;
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
            return Set.AsEnumerable().Where(x => x.ID == id).FirstOrDefault();
        }
        public void AddArticle(Article article)
        {
            Set.Add(article);
        }
        public void UpdateArticle(Article article, Article newArticle)
        {
            if (GetArticles().Where(x => x.ID == article.ID).FirstOrDefault() != null)
            {
                var item = new Article()
                {
                    ID = newArticle.ID,
                    Content = newArticle.Content,
                    User = newArticle.User,
                };

                Update(item);
            }
        }
        public void DeleteArticle(Article article)
        {
            var item = GetArticles().Where(x => x.ID == article.ID).FirstOrDefault();
            if (item != null)
            {
                Delete(item);
            }
        }

    }
}
