using ASP.Blog.Data.Entities;

namespace ASP.Blog.BLL.ViewModels.Article
{
    public class ArticleViewModel
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public User User { get; set; }
        public ArticleViewModel(User user) => User = user;
        public ArticleViewModel() { }
    }
}
