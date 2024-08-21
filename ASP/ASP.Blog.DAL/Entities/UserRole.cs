using Microsoft.AspNetCore.Identity;

namespace ASP.Blog.API.DAL.Entities
{
    public class UserRole : IdentityRole
    {
        public string Description { get; set; }
    }
}
