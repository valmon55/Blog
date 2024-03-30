using ASP.Blog.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using System;

namespace ASP.Blog.Data.Entities
{
    public class User : IdentityUser
    {
        //public int ID { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Middle_Name { get; set; }
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }
        public UserRole userRole { get; set; }
    }
    //public enum UserRole
    //{
    //    User,
    //    Moderator,
    //    Admin
    //}
}
