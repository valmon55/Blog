using ASP.Blog.API.DAL.Entities;
using ASP.Blog.API.DAL.UoW;
using ASP.Blog.API.Data.Entities;
using ASP.Blog.API.Services.IServices;
using ASP.Blog.API.ViewModels.Comment;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASP.Blog.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CommentController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ILogger<CommentController> _logger;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<UserRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICommentService _commentService;

        public CommentController(UserManager<User> userManager,
                SignInManager<User> signInManager,
                IUnitOfWork unitOfWork, 
                IMapper mapper,
                ILogger<CommentController> logger,
                RoleManager<UserRole> roleManager,
                ICommentService commentService)
        {
            _mapper = mapper;
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _unitOfWork = unitOfWork;
            _roleManager = roleManager;
            _commentService = commentService;
        }

        [Route("AddComment")]
        [HttpPost]
        public async Task<IActionResult> AddComment(CommentAddRequest model) 
        {
            if (ModelState.IsValid) 
            { 
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                _commentService.AddComment(model, user);
                return StatusCode(201);
            }
            else
            {
                _logger.LogError("Модель CommentViewModel при добавлении комментария невалидна!");
                return StatusCode(403);
            }
        }
        [Route("AllArticleComments")]
        [HttpGet]
        public List<CommentViewModel> AllArticleComments(int articleId)
        {
            return _commentService.AllArticleComments(articleId);
        }
        [Route("Delete")]
        [HttpDelete]
        public IActionResult Delete(int id) 
        {
            var articleId = _commentService.DeleteComment(id);
            if (articleId is not null)
            {
                return StatusCode(201);
            }
            else
            {
                return StatusCode(403);
            }
        }
        [Route("Update")]
        [HttpPost]
        public IActionResult Update(CommentViewModel model)
        {
            int articleId;

            if (ModelState.IsValid) 
            {
                articleId = _commentService.UpdateComment(model);
                _logger.LogInformation($"Выполняется переход на страницу просмотра статьи c ID = {articleId.ToString()}");
                return StatusCode(201);
            }
            else
            {
                _logger.LogError("Модель CommentViewModel при обновлении комментария невалидна!");
                _logger.LogWarning($"Выполняется переход на страницу просмотра всех статей.");
                return StatusCode(403);
            }
        }
    }
}
