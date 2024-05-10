using Entity = ASP.Blog.Data.Entities;
using System.Collections.Generic;

namespace ASP.Blog.BLL.ViewModels.Article
{
    public class ArticleViewModel
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public Entity.User User { get; set; }
        public List<Entity.Tag> Tags { get; set; }
        public ArticleViewModel(Entity.User user) => User = user;
        public ArticleViewModel() { }
    }
}
