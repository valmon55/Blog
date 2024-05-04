using ASP.Blog.BLL.ViewModels.Comment;
using ASP.Blog.DAL.Entities;
using ASP.Blog.DAL.Repositories;
using ASP.Blog.DAL.UoW;
using ASP.Blog.Data.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ASP.Blog.Controllers
{
    public class CommentController : Controller
    {
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<UserRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;

        public CommentController(UserManager<User> userManager,
                SignInManager<User> signInManager,
                IUnitOfWork unitOfWork, IMapper mapper, RoleManager<UserRole> roleManager)
        {
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            _unitOfWork = unitOfWork;
            _roleManager = roleManager;
        }
        [Route("AddComment")]
        [HttpGet]
        public IActionResult AddComment(int articleId) 
        { 
            return View();
        }
        [Route("AddComment")]
        [HttpPost]
        public IActionResult AddComment(Comment comment) 
        {
            return RedirectToAction("AllUserArticles", "Article");
        }
        [Route("AllArticleComments")]
        [HttpGet]
        public IActionResult AllArticleComments(int articleId)
        {
            var repo = _unitOfWork.GetRepository<Comment>() as CommentRepository;
            var comments = repo.GetComments();
            var commentsView = new List<CommentViewModel>();
            foreach (var comment in comments) 
            {
                if (comment.Article.ID == articleId)
                {
                    commentsView.Add(_mapper.Map<CommentViewModel>(comment));
                }
            }

            return View(commentsView);
        }
    }
}
