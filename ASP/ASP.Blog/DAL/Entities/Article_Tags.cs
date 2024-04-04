﻿using System;

namespace ASP.Blog.Data.Entities
{
    public class Article_Tags
    {
        public int ID { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public int TagId { get; set; }
        public Tag Tag { get; set; }
    }
}
