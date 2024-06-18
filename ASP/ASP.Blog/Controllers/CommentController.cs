using ASP.Blog.BLL.Extentions;
using ASP.Blog.BLL.ViewModels.Comment;
using ASP.Blog.DAL.Entities;
using ASP.Blog.DAL.Repositories;
using ASP.Blog.DAL.UoW;
using ASP.Blog.Data.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NLog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASP.Blog.Controllers
{
    public class CommentController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ILogger<CommentController> _logger;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<UserRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;

        public CommentController(UserManager<User> userManager,
                SignInManager<User> signInManager,
                IUnitOfWork unitOfWork, 
                IMapper mapper,
                ILogger<CommentController> logger,
                RoleManager<UserRole> roleManager)
        {
            _mapper = mapper;
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _unitOfWork = unitOfWork;
            _roleManager = roleManager;
        }
        [Route("AddComment")]
        [HttpGet]
        public IActionResult AddComment(int articleId) 
        {
            _logger.LogInformation($"Выполняется переход на страницу добавления комментария для статьи с ID = {articleId}");
            return View(new CommentViewModel() { ArticleId = articleId} );
        }
        [Route("AddComment")]
        [HttpPost]
        public async Task<IActionResult> AddComment(CommentViewModel model) 
        {
            if (ModelState.IsValid) 
            { 
                var comment = _mapper.Map<Comment>(model);
                
                comment.CommentDate = DateTime.Now;
                comment.User = await _userManager.FindByNameAsync(User.Identity.Name);
                comment.UserId = comment.User.Id;
                var repo = _unitOfWork.GetRepository<Comment>() as CommentRepository;
                repo.Create(comment);
                _logger.LogInformation($"Комментарий создал пользователь {comment.User.UserName} : {comment.User.First_Name} {comment.User.Last_Name}");
            }
            else
            {
                _logger.LogError("Модель CommentViewModel при добавлении комментария невалидна!");
                ModelState.AddModelError("", "Ошибка в модели!");
            }
            _logger.LogInformation($"Выполняется переход на страницу просмотра статьи c ID = {model.ArticleId}");
            return RedirectToAction("ViewArticle", "Article", new { Id = model.ArticleId });
        }
        [Route("AllArticleComments")]
        [HttpGet]
        public IActionResult AllArticleComments(int articleId)
        {
            _logger.LogInformation($"Выполняется переход на страницу просмотра всех статей комментариев статьи с ID = {articleId}.");
            var repo = _unitOfWork.GetRepository<Comment>() as CommentRepository;
            var comments = repo.GetComments();
            var commentsView = new List<CommentViewModel>();
            foreach (var comment in comments) 
            {
                if (comment.ArticleId == articleId)
                {
                    commentsView.Add(_mapper.Map<CommentViewModel>(comment));
                }
            }

            return View(commentsView);
        }
        [Route("Comment/Delete")]
        [HttpPost]
        public IActionResult Delete(int id) 
        {
            var repo =_unitOfWork.GetRepository<Comment>() as CommentRepository;
            var comment = repo.GetCommentById(id);
            var articleId = comment.ArticleId;
            _logger.LogInformation($"Удаление комментария с ID = {id}");

            repo.Delete(comment);

            return RedirectToAction("ViewArticle", "Article", new { Id = articleId });
        }
        [Route("Comment/Update")]
        [HttpGet]
        public IActionResult Update(int id)
        {
            var repo = _unitOfWork.GetRepository<Comment>() as CommentRepository;
            var comment = repo.GetCommentById(id);
            var commentView = _mapper.Map<CommentViewModel>(comment);
            _logger.LogInformation($"Выполняется переход на страницу обновления комментария с ID = {id}. ID статьи = {comment.ArticleId}.");

            return View("EditComment",commentView);
        }
        [Route("Comment/Update")]
        [HttpPost]
        public IActionResult Update(CommentViewModel model)
        {
            int articleId;

            if (ModelState.IsValid) 
            { 
                var repo = _unitOfWork.GetRepository<Comment>() as CommentRepository;
                var comment = repo.GetCommentById(model.Id);
                comment.Convert(model);
                articleId = comment.ArticleId;

                repo.Update(comment);
            }
            else
            {
                _logger.LogError("Модель CommentViewModel при обновлении комментария невалидна!");
                ModelState.AddModelError("", "Ошибка в модели!");
                _logger.LogWarning($"Выполняется переход на страницу просмотра всех статей.");
                return RedirectToAction("AllUserArticles", "Article");
            }
            _logger.LogInformation($"Выполняется переход на страницу просмотра статьи c ID = {model.ArticleId}");
            return RedirectToAction("ViewArticle", "Article", new { Id = articleId });
        }
    }
}
