using ASP.Blog.Data;
using ASP.Blog.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASP.Blog.DAL.Repositories
{
    public class Article_TagsRepository : Repository<Article_Tags>
    {
        public Article_TagsRepository(BlogContext db) : base(db) { }
        public List<Article_Tags> GetArticles_Tags()
        {
            return Set.ToList();
        }
        public Article_Tags GetArticle_TagsById(int id)
        {
            return Set.AsEnumerable().Where(x => x.ID == id).FirstOrDefault();
        }
        public void AddArticle_Tags(Article_Tags article_Tags)
        {
            Set.Add(article_Tags);
        }
        public void UpdateArticle_Tags(Article_Tags article_Tags, Article_Tags newArticle_Tags)
        {
            if (GetArticles_Tags().Where(x => x.ID == article_Tags.ID).FirstOrDefault() != null)
            {
                var item = new Article_Tags()
                {
                    ID = newArticle_Tags.ID,
                    Tag = newArticle_Tags.Tag,
                    User = newArticle_Tags.User,
                };

                Update(item);
            }
        }
        public void DeleteArticle_Tags(Article_Tags article_Tags)
        {
            var item = GetArticles_Tags().Where(x => x.ID == article_Tags.ID).FirstOrDefault();
            if (item != null)
            {
                Delete(item);
            }
        }

    }
}
