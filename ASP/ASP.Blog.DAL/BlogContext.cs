using ASP.Blog.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ASP.Blog.Data
{
    public class BlogContext : IdentityDbContext<User>
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
    //public BlogContext()
    //    {
    //        Database.EnsureCreated();
    //    }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Data Source=NORKA\\SQLEXPRESS;Database=MVC_StartApp;Integrated Security = true;Trust Server Certificate=True;Trusted_Connection=True;");
        //}
    }
}
