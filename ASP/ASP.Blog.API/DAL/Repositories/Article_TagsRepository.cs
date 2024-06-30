﻿using ASP.Blog.Data;
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
            Create(article_Tags);
        }
        public void UpdateArticle_Tags(Article_Tags article_Tags)
        {
            Update(article_Tags);
        }
        public void DeleteArticle_Tags(Article_Tags article_Tags)
        {
            Delete(article_Tags);
        }
    }
}
