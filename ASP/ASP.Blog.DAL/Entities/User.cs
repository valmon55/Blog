using System;

namespace ASP.Blog.Data.Entities
{
    public class User
    {
        public int ID { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public UserRole userRole { get; set; }
    }
    public enum UserRole
    {
        User,
        Moderator,
        Admin
    }
}
