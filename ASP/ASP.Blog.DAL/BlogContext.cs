using ASP.Blog.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace ASP.Blog.Data
{
    public class BlogContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Article_Tags> Article_Tags { get; set; }
        public BlogContext(DbContextOptions<BlogContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
