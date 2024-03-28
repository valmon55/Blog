using ASP.Blog.DAL.UoW;
using ASP.Blog.Data.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ASP.Blog.Controllers.Account
{
    public class AccountManagerController : Controller
    {
        private IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IUnitOfWork _unitOfWork;
        public AccountManagerController(UserManager<User> userManager, 
                SignInManager<User> signInManager, IUnitOfWork unitOfWork,IMapper mapper) 
        { 
            _mapper= mapper;
            _userManager= userManager;
            _signInManager= signInManager;
            _unitOfWork= unitOfWork;
        }
    }
}
