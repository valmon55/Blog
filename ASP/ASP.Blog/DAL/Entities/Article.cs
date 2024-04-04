﻿using System;

namespace ASP.Blog.Data.Entities
{
    public class Article
    {
        public int ID { get; set; }
        public string Content { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
    }
}
