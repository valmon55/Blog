using ASP.Blog.API.DAL.Entities;
using ASP.Blog.API.DAL.UoW;
using ASP.Blog.API.Data.Entities;
using ASP.Blog.API.Services.IServices;
using ASP.Blog.API.ViewModels.Article;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.Blog.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ArticleController : Controller
    {
        private IMapper _mapper;
        private readonly ILogger<ArticleController> _logger;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<UserRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IArticleService _articleService;

        public ArticleController(ILogger<ArticleController> logger, 
                UserManager<User> userManager,
                SignInManager<User> signInManager,
                IUnitOfWork unitOfWork, IMapper mapper, 
                RoleManager<UserRole> roleManager,
                IArticleService articleService
            )
        {
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            _unitOfWork = unitOfWork;
            _roleManager = roleManager;
            _articleService = articleService;
        }
        [Authorize]
        [Route("AddArticle")]
        [HttpPost]
        public async Task<IActionResult> AddArticle(ArticleViewModel model)
        {
            if(ModelState.IsValid) 
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                _articleService.AddArticle(model, model.SelectedTags, user);

                //return RedirectToAction("AllUserArticles");
                return StatusCode(201);
            }
            else
            {
                _logger.LogError("Модель ArticleViewModel не прошла проверку!");

                ModelState.AddModelError("", "Ошибка в модели!");
                //return RedirectToAction("AllUserArticles");
                return StatusCode(403);
            }
        }
        [Authorize]
        [Route("AllUserArticles")]
        [HttpGet]
        public async Task<List<ArticleViewModel>> AllUserArticles() 
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            return _articleService.AllArticles(user);
        }

        [Route("AllArticles")]
        [HttpGet]
        public List<ArticleViewModel> AllArticles()
        {
            return _articleService.AllArticles();
        }
        [Authorize]
        [Route("Delete")]
        [HttpDelete]
        public IActionResult Delete(int Id) 
        {
            try
            {
                _articleService.DeleteArticle(Id);
                return StatusCode(201);
            }
            catch (Exception ex)
            {
                return StatusCode(403);
            }
        }
        [Authorize]
        [Route("Update")]
        [HttpPost]
        public async Task<IActionResult> Update(ArticleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                _articleService.UpdateArticle(model,model.SelectedTags,user);

                //return RedirectToAction("AllUserArticles", "Article");
                return StatusCode(201);
            }
            else
            {
                _logger.LogError($"Ошибка в модели ArticleViewModel");
                ModelState.AddModelError("", "Ошибка в модели!");

                //return RedirectToAction("AllUserArticles", "Article");
                return StatusCode(403);
            }
        }
    }
}
