using System;

namespace ASP.Blog.Data.Tables
{
    public class Article_Tags
    {
        public Guid ID { get; set; }
        public User User { get; set; }
        public Tag Tag { get; set; }
    }
}
