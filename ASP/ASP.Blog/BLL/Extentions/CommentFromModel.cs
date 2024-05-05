using ASP.Blog.BLL.ViewModels.Comment;
using ASP.Blog.Data.Entities;

namespace ASP.Blog.BLL.Extentions
{
    public static class CommentFromModel
    {
        public static Comment Convert(this Comment comment, CommentViewModel commentViewModel)
        {
            comment.ID = commentViewModel.Id;
            comment.ArticleId = commentViewModel.ArticleId;
            comment.UserId = commentViewModel.UserId;
            comment.CommentDate = commentViewModel.CommentDate;
            comment.Comment_Text = commentViewModel.Comment;

            return comment;
        }
    }
}
