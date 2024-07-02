using ASP.Blog.API.Data.Entities;
using ASP.Blog.BLL.ViewModels.Comment;
using ASP.Blog.Data.Entities;
using System.Collections.Generic;

namespace ASP.Blog.API.Services.IServices
{
    public interface ICommentService
    {
        public CommentViewModel AddComment(int articleId);
        public void AddComment(CommentViewModel model, User user);
        public List<CommentViewModel> AllArticleComments(int articleId);
        public int? DeleteComment(int id);
        public CommentViewModel UpdateComment(int id);
        public int UpdateComment(CommentViewModel model);
    }
}
