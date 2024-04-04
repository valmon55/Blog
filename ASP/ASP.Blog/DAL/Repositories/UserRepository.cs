using ASP.Blog.DAL.Entities;
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
        BlogContext _db;
        public UserRepository(BlogContext db) : base(db) { _db = db; }

        public List<User> GetUsers() 
        {
            return Set.ToList();
        }
        public User GetUserById(string UserId)
        {
            return Set.Where(x => x.Id == UserId).FirstOrDefault();
        }
        public void AddUser(User user)
        {
            var _user = user;

            if (_db.User_Roles.Where(x => x.RoleName == "User").FirstOrDefault() == null)
            {
                _db.User_Roles.Add(new UserRole() { ID = 1, RoleName = "User", Description = "Ordinary User" });
            }

            _user.userRole = _db.User_Roles.Where(x => x.RoleName == "User").FirstOrDefault();
            //Set.Add(user); 
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
