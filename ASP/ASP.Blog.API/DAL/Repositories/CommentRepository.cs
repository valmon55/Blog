﻿using ASP.Blog.Data;
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
        public List<Comment> GetCommentsByArticleId(int articleId) 
        { 
            return Set.AsEnumerable().Where(c => c.ArticleId == articleId).ToList();
        }
        public void AddComment(Comment comment)
        {
            Set.Add(comment);
        }
        public void UpdateComment(Comment comment)
        {
            Update(comment);
        }
        public void DeleteComment(Comment comment)
        {
            Delete(comment);
        }
    }
}
