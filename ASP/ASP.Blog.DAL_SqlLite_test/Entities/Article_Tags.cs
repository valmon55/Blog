using SQLite;
using System;

namespace ASP.Blog.Data.Tables
{
    [Table("Article_Tags")]
    public class Article_Tags
    {
        [PrimaryKey]
        public int ID { get; set; }
        public User User { get; set; }
        public Tag Tag { get; set; }
    }
}
