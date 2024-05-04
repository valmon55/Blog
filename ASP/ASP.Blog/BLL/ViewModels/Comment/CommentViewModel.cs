using System;
using Entities = ASP.Blog.Data.Entities;

namespace ASP.Blog.BLL.ViewModels.Comment
{
    public class CommentViewModel
    {
        public int Id { get; set; }
        public string Comment { get; set; }
        public DateTime CommentDate { get; set; }
        public Entities.User User { get; set; }
        public Entities.Article Article { get; set; }
    }
}
