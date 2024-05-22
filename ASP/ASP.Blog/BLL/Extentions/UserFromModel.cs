using ASP.Blog.BLL.ViewModels;
using ASP.Blog.BLL.ViewModels.Role;
using ASP.Blog.DAL.Entities;
using ASP.Blog.Data.Entities;

namespace ASP.Blog.BLL.Extentions
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
