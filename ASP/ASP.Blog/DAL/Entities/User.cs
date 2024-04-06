﻿using ASP.Blog.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace ASP.Blog.Data.Entities
{
    public class User : IdentityUser
    {
        //public int ID { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Middle_Name { get; set; }
        //public string Email { get; set; }
        public DateTime BirthDate { get; set; }
        public List<Article> Articles { get; set; }
        //public int UserRoleId { get; set; }
        public UserRole userRole { get; set; } = new UserRole();
    }
    //public enum UserRole
    //{
    //    User,
    //    Moderator,
    //    Admin
    //}
}