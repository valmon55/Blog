using SQLite;
using System;

namespace ASP.Blog.Data.Tables
{
    [Table("Comment")]
    public class Comment
    {
        [PrimaryKey]
        public int ID { get; set; }
        public string Comment_Text { get; set; }
        public Article Article { get; set; }
    }
}
