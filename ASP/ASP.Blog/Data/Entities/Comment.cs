using System;

namespace ASP.Blog.Data.Tables
{
    public class Comment
    {
        public Guid ID { get; set; }
        public string Comment_Text { get; set; }
        public Article Article { get; set; }
    }
}
