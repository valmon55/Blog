using System;
using Entities = ASP.Blog.Data.Entities;

namespace ASP.Blog.BLL.ViewModels.Comment
{
    public class CommentViewModel
    {
        public int Id { get; set; }
        public string Comment { get; set; }
        public DateTime CommentDate { get; set; }
        public int UserId { get; set; }
        public int ArticleId { get; set; }
    }
}
