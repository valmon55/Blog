using ASP.Blog.API.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASP.Blog.API.Services.IServices
{
    public interface IUserService
    {
        public void Register(RegisterViewModel model);
        public void Login(LoginViewModel model);
        public Task<List<UserViewModel>> AllUsers();
        public UserViewModel UpdateUser(string userId);
        public Task UpdateUser(UserViewModel model, List<string> SelectedRoles);
        public void DeleteUser(string userId);
    }
}
