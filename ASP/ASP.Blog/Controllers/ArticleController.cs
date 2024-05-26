using ASP.Blog.BLL.Extentions;
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
using System.Linq;
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
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var repo = _unitOfWork.GetRepository<Tag>() as TagRepository;
            var allTags = repo.GetTags();
            return View(new ArticleViewModel(user) { Tags = allTags, ArticleDate = DateTime.Now });    
        }
        [Authorize]
        [Route("AddArticle")]
        [HttpPost]
        public async Task<IActionResult> AddArticle(ArticleViewModel model, List<int> SelectedTags)
        {
            if(ModelState.IsValid) 
            {
                var tagsId = new List<int>();
                var tagRepo = _unitOfWork.GetRepository<Tag>() as TagRepository;
                SelectedTags.ForEach(id => tagsId.Add(tagRepo.GetTagById(id).ID) );
                var tags = new List<Tag>();
                foreach(var tag in tagsId)
                {
                    tags.Add(tagRepo.GetTagById((int)tag));
                }

                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                model.User = user;
                model.Tags = tags;
                var article = _mapper.Map<Article>(model);
                var repo = _unitOfWork.GetRepository<Article>();
                repo.Create(article);
            }
            else
            {
                ModelState.AddModelError("", "Ошибка в модели!");
            }
            return RedirectToAction("AllUserArticles");
        }
        [Authorize]
        [Route("AllUserArticles")]
        [HttpGet]
        public async Task<IActionResult> AllUserArticles() 
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var repo = _unitOfWork.GetRepository<Article>() as ArticleRepository;
            var articles = repo.GetArticlesByUserId(user.Id);
            var articlesView = new List<ArticleViewModel>();
            foreach (var article in articles) 
            {
                articlesView.Add(_mapper.Map<ArticleViewModel>(article));
            }
            
            return View("AllArticles", articlesView);
        }

        [Route("AllArticles")]
        [HttpGet]
        public IActionResult AllArticles()
        {
            var repo = _unitOfWork.GetRepository<Article>() as ArticleRepository;
            var articles = repo.GetArticles();
            var articlesView = new List<ArticleViewModel>();
            foreach (var article in articles)
            {
                articlesView.Add(_mapper.Map<ArticleViewModel>(article));
            }

            return View("AllArticles", articlesView);
        }
        [Route("ViewArticle")]
        [HttpGet]
        public IActionResult ViewArticle(int Id)
        { 
            var repo = _unitOfWork.GetRepository<Article>() as ArticleRepository;
            var article = repo.GetArticleById(Id);
            var commentRepo = _unitOfWork.GetRepository<Comment>() as CommentRepository;
            var comments = commentRepo.GetCommentsByArticleId(Id);

            var articleView = _mapper.Map<ArticleViewModel>(article);
            articleView.Comments = comments;

            return View("Article", articleView); 
        }

        [Authorize]
        [Route("Delete")]
        [HttpPost]
        public IActionResult Delete(int Id) 
        {
            var repo = _unitOfWork.GetRepository<Article>() as ArticleRepository;
            repo.DeleteArticle(repo.Get(Id));

            return RedirectToAction("AllUserArticles","Article");
        }

        [Authorize]
        [Route("Article/Update")]
        [HttpGet]
        public async Task<IActionResult> Update(int Id)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var repo = _unitOfWork.GetRepository<Article>() as ArticleRepository;
            var article = repo.GetArticleById(Id);
            article.User = user;
            var articleView = _mapper.Map<ArticleViewModel>(article);

            var tagRepo =_unitOfWork.GetRepository<Tag>() as TagRepository;
            var allTags = tagRepo.GetTags();

            var checkedTags = article.Tags;

            var checkedTagsDic = new Dictionary<Tag, bool>();

            foreach (var tag in allTags) 
            {
                checkedTagsDic.Add(tag, false);
                foreach (var checkedTag in checkedTags)
                {
                    if (tag.Tag_Name == checkedTag.Tag_Name) 
                    {
                        checkedTagsDic[tag] = true;
                    }
                }
            }

            return View("EditArticle", new ArticleViewModel(user) 
                    { Tags = allTags, 
                      CheckedTagsDic = checkedTagsDic,
                      ArticleDate = articleView.ArticleDate,
                      Title = articleView.Title,
                      Content = articleView.Content
            });

            //return View("EditArticle", articleView);
        }

        [Authorize]
        [Route("Article/Update")]
        [HttpPost]
        public async Task<IActionResult> Update(ArticleViewModel model, List<int> SelectedTags)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);

                var tagsId = new List<int>();
                var tagRepo = _unitOfWork.GetRepository<Tag>() as TagRepository;
                SelectedTags.ForEach(id => tagsId.Add(tagRepo.GetTagById(id).ID));
                var tags = new List<Tag>();
                foreach (var tag in tagsId)
                {
                    tags.Add(tagRepo.GetTagById((int)tag));
                }

                model.User = user;
                model.Tags= tags;

                var repo = _unitOfWork.GetRepository<Article>() as ArticleRepository;
                var article = repo.GetArticleById(model.Id);
                article.Convert(model);

                repo.Update(article);
            }
            return RedirectToAction("AllUserArticles", "Article");
        }
    }
}
