﻿using SQLite;
using System;

namespace ASP.Blog.Data.Tables
{
    [Table("User")]
    public class User
    {
        [PrimaryKey]
        public int ID { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set;}
    }
}
