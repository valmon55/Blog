﻿using ASP.Blog;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ASP.Blog.Models;
using Microsoft.AspNetCore.Identity;
using ASP.Blog.Data.Entities;

namespace ASP.Blog.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SignInManager<User> _signInManager;

        public HomeController(ILogger<HomeController> logger, SignInManager<User> signInManager)
        {
            _logger = logger;
            _signInManager = signInManager;
        }

        //[Route("")]
        //[Route("[controller]/[action]")]
        public IActionResult Index()
        {
            _logger.LogInformation("Выполняется переход на стартовую страницу.");
            return RedirectToAction("AllArticles", "Article");
        }

        public IActionResult Privacy()
        {
            _logger.LogInformation("Выполняется переход на страницу Privacy.");
            //return StatusCode(500);
            //return StatusCode(403);
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [Route("Home/Error")]
        public IActionResult Error(int? statusCode = null)
        {
            if (statusCode.HasValue) 
            {
                _logger.LogError($"Произошла ошибка с кодом: {statusCode}");

                if (statusCode == 404)
                {
                    return View("ResourceNotFound");
                }
                else if (statusCode == 403)
                {
                    return View("AccessRestricted");
                }
                else
                {
                    return View("SomethingGoesWrong");
                }
            }
            else
            {
                _logger.LogInformation($"Произошла ошибка, код ошибки неизвестен...");
                return View("SomethingGoesWrong");
            }
        }
    }
}
