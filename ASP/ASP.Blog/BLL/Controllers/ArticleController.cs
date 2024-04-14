using ASP.Blog.BLL.ViewModels.Article;
using ASP.Blog.DAL.Entities;
using ASP.Blog.DAL.UoW;
using ASP.Blog.Data.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ASP.Blog.Controllers
{
    public class ArticleController : Controller
    {
        private IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<UserRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;

        public ArticleController(UserManager<User> userManager,
                SignInManager<User> signInManager,
                IUnitOfWork unitOfWork, IMapper mapper, RoleManager<UserRole> roleManager)
        {
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            _unitOfWork = unitOfWork;
            _roleManager = roleManager;
        }
        [Route("Add")]
        [HttpGet]
        public IActionResult Add() 
        {
            ///TODO: Как передать сюда авторизованного пользователя?
            return View(new ArticleViewModel());    
        }
        [Route("AllArticles")]
        [HttpGet]
        public IActionResult AllArticles() 
        {
            return View(new AllArticlesViewModel());
        }
        [Route("Article=")]
        [HttpGet]
        public IActionResult UserArticles() 
        { 
            return View();
        }
    }
}
