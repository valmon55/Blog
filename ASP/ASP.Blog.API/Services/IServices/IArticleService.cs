using ASP.Blog.API.Data.Entities;
using ASP.Blog.API.ViewModels.Article;
using System.Collections.Generic;

namespace ASP.Blog.API.Services.IServices
{
    public interface IArticleService
    {
        public void AddArticle(ArticleAddRequest article, User user);
        public List<ArticleViewModel> AllUserArticles();
        public List<ArticleViewModel> AllArticles(User user = null);
        public void DeleteArticle(int id);
        public void UpdateArticle(ArticleViewModel article, List<int> SelectedTags, User user);
    }
}
