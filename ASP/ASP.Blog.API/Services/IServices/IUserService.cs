using ASP.Blog.API.ViewModels;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASP.Blog.API.Services.IServices
{
    public interface IUserService
    {
        public void Register(RegisterRequest model);
        public Task<SignInResult> Login(LoginRequest model);
        public Task<List<UserViewRequest>> AllUsers();
        public UserViewRequest UpdateUser(string userId);
        public Task UpdateUser(UserEditRequest model);
        public void DeleteUser(string userId);
    }
}
