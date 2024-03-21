using SQLite;
using System;

namespace ASP.Blog.Data.Tables
{
    [Table("Article")]
    public class Article
    {
        [PrimaryKey]
        public int ID { get; set; }
        public string Content { get; set; }
        public User User { get; set; }
    }
}
