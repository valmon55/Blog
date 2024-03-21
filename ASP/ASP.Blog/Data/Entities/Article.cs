using System;

namespace ASP.Blog.Data.Entities
{
    public class Article
    {
        public Guid ID { get; set; }
        public string Content { get; set; }
        public User User { get; set; }
    }
}
