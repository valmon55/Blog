﻿using ASP.Blog.DAL.Configs;
using ASP.Blog.DAL.Entities;
using ASP.Blog.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ASP.Blog.Data
{
    public class BlogContext : IdentityDbContext<User>
    {
        override public DbSet<User> Users { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Article_Tags> Article_Tags { get; set; }
        public BlogContext(DbContextOptions<BlogContext> options) : base(options)
        {
            Database.EnsureCreated();
            //Database.EnsureDeleted();
            //Database.Migrate();
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration<Article>(new ArticleConfiguration());
            builder.ApplyConfiguration<Comment>(new CommentConfiguration());
            builder.ApplyConfiguration<Tag>(new TagConfiguration());
            //builder.ApplyConfiguration<Article_Tags>(new Article_TagsConfiguration());
        }
    }
}
