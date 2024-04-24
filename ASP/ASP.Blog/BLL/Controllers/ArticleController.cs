using ASP.Blog.BLL.ViewModels.Article;
using ASP.Blog.DAL.Entities;
using ASP.Blog.DAL.Repositories;
using ASP.Blog.DAL.UoW;
using ASP.Blog.Data.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        [Authorize]
        [Route("AddArticle")]
        [HttpGet]
        public async Task<IActionResult> AddArticle() 
        {
            ///TODO: Как передать сюда авторизованного пользователя?
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            return View(new ArticleViewModel(user));    
        }
        [Authorize]
        [Route("AddArticle")]
        [HttpPost]
        public async Task<IActionResult> AddArticle(ArticleViewModel model)
        {
            if(ModelState.IsValid) 
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                model.User = user;
                var article = _mapper.Map<Article>(model);
                var repo = _unitOfWork.GetRepository<Article>();
                repo.Create(article);
            }
            else
            {
                ModelState.AddModelError("", "Ошибка в модели!");
            }
            return RedirectToAction("Index","Home");
        }
        [Authorize]
        [Route("AllUserArticles")]
        [HttpGet]
        public async Task<IActionResult> AllUserArticles() 
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var repo = _unitOfWork.GetRepository<Article>() as ArticleRepository;
            var articles = repo.GetArticleByUserId(user.Id);
            var articlesView = new List<ArticleViewModel>();
            foreach (var article in articles) 
            {
                articlesView.Add(_mapper.Map<ArticleViewModel>(article));
            }
            
            return View(articlesView);
        }

        [Route("AllArticles")]
        [HttpGet]
        public async Task<IActionResult> AllArticles()
        {
            var repo = _unitOfWork.GetRepository<Article>() as ArticleRepository;
            var articles = repo.GetAll();
            var articlesView = new List<ArticleViewModel>();
            foreach (var article in articles)
            {
                articlesView.Add(_mapper.Map<ArticleViewModel>(article));
            }

            return View(articlesView);
        }
        [Authorize]
        [Route("Delete")]
        [HttpPost]
        public IActionResult Delete(int Id) 
        {
            var repo = _unitOfWork.GetRepository<Article>() as ArticleRepository;
            repo.DeleteArticle(repo.Get(Id));

            return RedirectToAction("AllArticles","Article");
        }
        [Authorize]
        [Route("Article/Update")]
        [HttpPost]
        public async Task<IActionResult> Update(ArticleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                model.User = user;
                var article = _mapper.Map<Article>(model);
                var repo = _unitOfWork.GetRepository<Article>() as ArticleRepository;
                repo.Update(article);
            }
            return RedirectToAction("AllArticles", "Article");
        }

        [Route("Get")]
        [HttpGet]
        public async Task<IActionResult> Get(ArticleViewModel article) 
        {
            //var author = await _userManager.FindByIdAsync(article.User.Id);
            //Console.WriteLine($"Автор: {author.First_Name} {author.Email}");
            return View();
        }
    }
}
