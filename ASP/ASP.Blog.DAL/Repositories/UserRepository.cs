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
        public UserRepository(BlogContext db) : base(db) { }

        public List<User> GetUsers() 
        {
            return Set.ToList();
        }
        public User GetUserById(int id)
        {
            return Set.AsEnumerable().Where(x => x.ID == id).FirstOrDefault();
        }
        public void AddUser(User user)
        {
            Set.Add(user);
        }
        public void UpdateUser(User user, User newUser)
        {
            if( GetUsers().Where(x => x.ID == user.ID).FirstOrDefault() != null) 
            {
                var item = new User()
                {
                    ID = newUser.ID,
                    First_Name = newUser.First_Name,
                    Last_Name = newUser.Last_Name,
                    userRole = newUser.userRole
                };

                Update(item);
            }
        }
        public void DeleteUser(User user)
        {
            var item = GetUsers().Where(x => x.ID == user.ID).FirstOrDefault();
            if ( item != null)
            {
                Delete(item);
            }
        }
    }
}
