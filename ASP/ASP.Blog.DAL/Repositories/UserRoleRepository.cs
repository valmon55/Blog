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
    public class UserRoleRepository : Repository<UserRole>
    {
        public UserRoleRepository(BlogContext db) : base(db) { }
        public List<UserRole> GetUserRoles()
        {
            return Set.ToList();
        }
        public UserRole GetUserRoleById(int id)
        {
            return Set.AsEnumerable().Where(x => x.ID == id).FirstOrDefault();
        }
        public void AddUserRole(UserRole userRole)
        {
            Set.Add(userRole);
        }
        public void UpdateUserRole(UserRole userRole, UserRole newUserRole)
        {
            if (GetUserRoles().Where(x => x.ID == userRole.ID).FirstOrDefault() != null)
            {
                var item = new UserRole()
                {
                    ID = newUserRole.ID,
                    RoleName = newUserRole.RoleName,
                    Description = newUserRole.Description,
                };

                Update(item);
            }
        }
        public void DeleteUserRole(UserRole userRole)
        {
            var item = GetUserRoles().Where(x => x.ID == userRole.ID).FirstOrDefault();
            if (item != null)
            {
                Delete(item);
            }
        }

    }
}
