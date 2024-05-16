using ASP.Blog.BLL.ViewModels.Article;
using ASP.Blog.Data.Entities;

namespace ASP.Blog.BLL.Extentions
{
    public static class ArticleFromModel
    {
        public static Article Convert(this Article article, ArticleViewModel articleViewModel)
        {
            article.ID = articleViewModel.Id;
            article.Title = articleViewModel.Title;
            article.Content = articleViewModel.Content;
            article.UserId = articleViewModel.User.Id;                  
            article.User = articleViewModel.User;
            article.Tags = articleViewModel.Tags;

            return article;
        }
    }
}
