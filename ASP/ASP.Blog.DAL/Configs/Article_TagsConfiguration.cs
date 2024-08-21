using ASP.Blog.API.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ASP.Blog.API.DAL.Configs
{
    public class Article_TagsConfiguration : IEntityTypeConfiguration<Article_Tags>
    {
        public void Configure(EntityTypeBuilder<Article_Tags> builder)
        {
            builder.ToTable("Article_Tags").HasKey(p => p.ID);
        }
    }
}
