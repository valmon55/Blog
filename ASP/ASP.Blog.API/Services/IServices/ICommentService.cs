using ASP.Blog.API.Data.Entities;
using ASP.Blog.API.ViewModels.Comment;
using System.Collections.Generic;

namespace ASP.Blog.API.Services.IServices
{
    public interface ICommentService
    {
        public CommentViewModel AddComment(int articleId);
        public void AddComment(CommentAddRequest model, User user);
        public List<CommentViewRequest> AllArticleComments(int articleId);
        public int? DeleteComment(int id);
        public CommentViewModel UpdateComment(int id);
        public int UpdateComment(CommentViewModel model);
    }
}
