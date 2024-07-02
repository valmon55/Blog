﻿using ASP.Blog.BLL.ViewModels.Article;
using ASP.Blog.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASP.Blog.API.Services.IServices
{
    public interface IArticleService
    {
        public ArticleViewModel AddArticle(User user);
        public void AddArticle(ArticleViewModel article, List<int> SelectedTags, User user);
        public List<ArticleViewModel> AllUserArticles();
        public List<ArticleViewModel> AllArticles(User user = null);
        public ArticleViewModel ViewArticle(int id);
        public void DeleteArticle(int id);
        public ArticleViewModel UpdateArticle(int id, User user);
        public void UpdateArticle(ArticleViewModel article, List<int> SelectedTags, User user);
    }
}
