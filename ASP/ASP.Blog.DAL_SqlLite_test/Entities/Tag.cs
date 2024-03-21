using SQLite;
using System;

namespace ASP.Blog.Data.Tables
{
    [Table("Tag")]
    public class Tag
    {
        [PrimaryKey]
        public int ID { get; set; }
        public string Tag_Name { get; set; }
    }
}
