using ASP.Blog.API.Data.Entities;
using ASP.Blog.API.ViewModels.Article;
using System.Collections.Generic;

namespace ASP.Blog.API.Services.IServices
{
    public interface IArticleService
    {
        public void AddArticle(ArticleAddRequest article, User user);
        public List<ArticleViewRequest> AllUserArticles();
        public List<ArticleViewRequest> AllArticles(User user = null);
        public void DeleteArticle(int id);
        public void UpdateArticle(ArticleViewModel article, List<int> SelectedTags, User user);
    }
}
