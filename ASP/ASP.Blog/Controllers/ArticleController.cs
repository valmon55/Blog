using ASP.Blog.BLL.Extentions;
using ASP.Blog.BLL.ViewModels.Article;
using ASP.Blog.DAL.Entities;
using ASP.Blog.DAL.Repositories;
using ASP.Blog.DAL.UoW;
using ASP.Blog.Data.Entities;
using ASP.Blog.Services.IServices;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.Blog.Controllers
{
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
        [HttpGet]
        public async Task<IActionResult> AddArticle() 
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            //var repo = _unitOfWork.GetRepository<Tag>() as TagRepository;
            //var allTags = repo.GetTags();
            //_logger.LogInformation("Выполняется переход на страницу добавления статьи.");
            //return View(new ArticleViewModel(user) { Tags = allTags, ArticleDate = DateTime.Now });    
            return View(_articleService.AddArticle(user));
        }
        [Authorize]
        [Route("AddArticle")]
        [HttpPost]
        public async Task<IActionResult> AddArticle(ArticleViewModel model, List<int> SelectedTags)
        {
            if(ModelState.IsValid) 
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                _articleService.AddArticle(model, SelectedTags, user);
                //var tagsId = new List<int>();
                //var tagRepo = _unitOfWork.GetRepository<Tag>() as TagRepository;
                //SelectedTags.ForEach(id => tagsId.Add(tagRepo.GetTagById(id).ID) );
                //var tags = new List<Tag>();
                //foreach(var tag in tagsId)
                //{
                //    tags.Add(tagRepo.GetTagById((int)tag));
                //    _logger.LogInformation($"Выбран тег {tagRepo.GetTagById((int)tag).Tag_Name}");
                //}

                //var user = await _userManager.FindByNameAsync(User.Identity.Name);
                //_logger.LogInformation($"Создаёт статью пользователь {user.UserName} : {user.First_Name} {user.Last_Name}");
                //model.User = user;
                //model.Tags = tags;
                //model.ArticleDate = DateTime.Now;

                //var article = _mapper.Map<Article>(model);
                //var repo = _unitOfWork.GetRepository<Article>();

                //_logger.LogInformation("Выполняется добавление новой статьи статьи.");
                //repo.Create(article);
                //_logger.LogInformation($"Выполняется переход на страницу просмотра статей пользователя {user.UserName} : {user.First_Name} {user.Last_Name}.");
                return RedirectToAction("AllUserArticles");
            }
            else
            {
                _logger.LogError("Модель ArticleViewModel не прошла проверку!");

                ModelState.AddModelError("", "Ошибка в модели!");
                return RedirectToAction("AllUserArticles");
            }
        }
        [Authorize]
        [Route("AllUserArticles")]
        [HttpGet]
        public async Task<IActionResult> AllUserArticles() 
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            //_logger.LogInformation($"Выполняется переход на страницу просмотра всех статей пользователя {user.UserName} : {user.First_Name} {user.Last_Name}.");

            //var repo = _unitOfWork.GetRepository<Article>() as ArticleRepository;
            //var articles = repo.GetArticlesByUserId(user.Id);
            //var articlesView = new List<ArticleViewModel>();

            //_logger.LogInformation($"Статьи пользователя {user.UserName} : {user.First_Name} {user.Last_Name}:");
            //foreach (var article in articles) 
            //{
            //    articlesView.Add(_mapper.Map<ArticleViewModel>(article));
            //    _logger.LogInformation($"Дата: {article.ArticleDate.ToShortDateString()} {article.ArticleDate.ToShortTimeString()} \n" + 
            //        $"заголовок: {article.Title}");

            //}
            
            //return View("AllArticles", articlesView);
            return View("AllArticles", _articleService.AllArticles(user));
        }

        [Route("AllArticles")]
        [HttpGet]
        public IActionResult AllArticles()
        {
            //_logger.LogInformation("Выполняется переход на страницу просмотра всех статей.");
            //var repo = _unitOfWork.GetRepository<Article>() as ArticleRepository;
            //var articles = repo.GetArticles();
            //var articlesView = new List<ArticleViewModel>();
            //_logger.LogInformation("Все статьи:");

            //foreach (var article in articles)
            //{
            //    articlesView.Add(_mapper.Map<ArticleViewModel>(article));
            //    _logger.LogInformation($"Дата: {article.ArticleDate.ToShortDateString()} {article.ArticleDate.ToShortTimeString()} \n" +
            //        $"заголовок: {article.Title}");
            //}

            //return View("AllArticles", articlesView);
            return View("AllArticles", _articleService.AllArticles());
        }
        [Route("ViewArticle")]
        [HttpGet]
        public IActionResult ViewArticle(int Id)
        {
            //_logger.LogInformation($"Выполняется переход на страницу просмотра статьи.");
            //var repo = _unitOfWork.GetRepository<Article>() as ArticleRepository;
            //var article = repo.GetArticleById(Id);
            //_logger.LogInformation($"Статья: \n"+ $"дата: {article.ArticleDate.ToShortDateString()} {article.ArticleDate.ToShortTimeString()} \n" +
            //        $"заголовок: {article.Title} \n" + $"текст: {article.Content}");
            //var commentRepo = _unitOfWork.GetRepository<Comment>() as CommentRepository;
            //var comments = commentRepo.GetCommentsByArticleId(Id);
            //_logger.LogInformation($"Количество комментариев: {comments.Count}");
            //var articleView = _mapper.Map<ArticleViewModel>(article);
            //articleView.Comments = comments;

            //return View("Article", articleView); 
            return View("Article", _articleService.ViewArticle(Id));
        }

        [Authorize]
        [Route("Delete")]
        [HttpPost]
        public IActionResult Delete(int Id) 
        {
            //var repo = _unitOfWork.GetRepository<Article>() as ArticleRepository;
            //_logger.LogInformation($"Удаление статьи, заголовок: {repo.Get(Id).Title}");
            //repo.DeleteArticle(repo.Get(Id));
            _articleService.DeleteArticle(Id);

            return RedirectToAction("AllUserArticles","Article");
        }

        [Authorize]
        [Route("Article/Update")]
        [HttpGet]
        public async Task<IActionResult> Update(int Id)
        {
            //var user = await _userManager.FindByNameAsync(User.Identity.Name);
            //var repo = _unitOfWork.GetRepository<Article>() as ArticleRepository;
            //var article = repo.GetArticleById(Id);
            //_logger.LogInformation($"Статья для обновления:\n" + $"дата {article.ArticleDate.ToShortDateString()} {article.ArticleDate.ToShortTimeString()} \n" +
            //        $"заголовок {article.Title} \n" + $"текст {article.Content}");
            //// TODO: менять автора статьи? возможно не потребуется
            //article.User = user;
            //var articleView = _mapper.Map<ArticleViewModel>(article);

            //var tagRepo =_unitOfWork.GetRepository<Tag>() as TagRepository;
            //var allTags = tagRepo.GetTags();

            //var checkedTags = article.Tags;

            //var checkedTagsDic = new Dictionary<Tag, bool>();

            //foreach (var tag in allTags) 
            //{
            //    checkedTagsDic.Add(tag, false);
            //    foreach (var checkedTag in checkedTags)
            //    {
            //        if (tag.Tag_Name == checkedTag.Tag_Name) 
            //        {
            //            checkedTagsDic[tag] = true;
            //        }
            //    }
            //}

            //return View("EditArticle", new ArticleViewModel(user) 
            //        { Tags = allTags, 
            //          CheckedTagsDic = checkedTagsDic,
            //          ArticleDate = articleView.ArticleDate,
            //          Title = articleView.Title,
            //          Content = articleView.Content
            //});

            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            return View("EditArticle", _articleService.UpdateArticle(Id, user));
        }

        [Authorize]
        [Route("Article/Update")]
        [HttpPost]
        public async Task<IActionResult> Update(ArticleViewModel model, List<int> SelectedTags)
        {
            //model.CheckedTagsDic = checkedTagsDic;
            if (ModelState.IsValid)
            {
                //var user = await _userManager.FindByNameAsync(User.Identity.Name);

                //var tagRepo = _unitOfWork.GetRepository<Tag>() as TagRepository;

                //var tagsId = new List<int>();
                //SelectedTags.ForEach(id => tagsId.Add(tagRepo.GetTagById(id).ID));
                //var tags = new List<Tag>();
                //foreach (var tag in tagsId)
                //{
                //    tags.Add(tagRepo.GetTagById((int)tag));
                //}
                //model.CheckedTagsDic = SelectedTags
                //    .Select(tagId => tagRepo.Get(tagId))
                //    .ToDictionary(tag => tag, tag => true);
                //model.User = user;
                //model.Tags= tags;
                //model.ArticleDate = DateTime.Now;

                //var repo = _unitOfWork.GetRepository<Article>() as ArticleRepository;
                //var article = repo.GetArticleById(model.Id);
                //article.Convert(model);

                //_logger.LogInformation($"Обновление статьи:\n" + $"дата {article.ArticleDate.ToShortDateString()} {article.ArticleDate.ToShortTimeString()} \n" +
                //        $"заголовок {article.Title} \n" + $"текст {article.Content}");

                //repo.Update(article);

                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                _articleService.UpdateArticle(model,SelectedTags,user);

                return RedirectToAction("AllUserArticles", "Article");
            }
            else
            {
                _logger.LogError($"Ошибка в модели ArticleViewModel");
                ModelState.AddModelError("", "Ошибка в модели!");

                return RedirectToAction("AllUserArticles", "Article");
            }
        }
    }
}
