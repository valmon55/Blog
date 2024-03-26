using ASP.Blog.Data;
using ASP.Blog.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASP.Blog.DAL.Repositories
{
    public class CommentRepository : Repository<Comment>
    {
        public CommentRepository(BlogContext db) : base(db) { }
        public List<Comment> GetComments()
        {
            return Set.ToList();
        }
        public Comment GetCommentById(int id)
        {
            return Set.AsEnumerable().Where(x => x.ID == id).FirstOrDefault();
        }
        public void AddComment(Comment comment)
        {
            Set.Add(comment);
        }
        public void UpdateComment(Comment comment, Comment newComment)
        {
            if (GetComments().Where(x => x.ID == comment.ID).FirstOrDefault() != null)
            {
                var item = new Comment()
                {
                    ID = newComment.ID,
                    Comment_Text= newComment.Comment_Text,
                    Article = newComment.Article,
                };

                Update(item);
            }
        }
        public void DeleteComment(Comment comment)
        {
            var item = GetComments().Where(x => x.ID == comment.ID).FirstOrDefault();
            if (item != null)
            {
                Delete(item);
            }
        }
    }
}
