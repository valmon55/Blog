﻿using ASP.Blog.DAL.Entities;
using ASP.Blog.Data;
using ASP.Blog.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASP.Blog.DAL.Repositories
{
    public class UserRepository : Repository<User>
    {
        BlogContext db;
        public UserRepository(BlogContext db) : base(db) { this.db = db; }

        public List<User> GetUsers() 
        {
            return Set.ToList();
        }
        public User GetUserById(string UserId)
        {
            return Set.Where(x => x.Id == UserId).FirstOrDefault();
        }
        public void AddUser(User user, UserRole userRole = null)
        {
            if (userRole != null)
            {
                var _user = user;

                if (db.Roles.Where(x => x.Name == "User").FirstOrDefault() == null)
                {
                    db.Roles.Add(new UserRole() { Name = "User", Description = "Ordinary User" });
                    db.SaveChanges();
                }
            }
            Set.Add(user); 
        }
        public void UpdateUser(User user)
        {
            Update(user);
        }
        public void DeleteUser(User user)
        {
            Delete(user);
        }
    }
}
