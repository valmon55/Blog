using ASP.Blog.API.DAL.Entities;
using ASP.Blog.API.ViewModels.Role;

namespace ASP.Blog.API.Extentions
{
    public static class RoleFromModel
    {
        public static UserRole Convert(this UserRole role, RoleViewModel roleeditvm)
        {
            role.Name = roleeditvm.Name;
            role.Description = roleeditvm.Description;

            return role;
        }
    }
}
