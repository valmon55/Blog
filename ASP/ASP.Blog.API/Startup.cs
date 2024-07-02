using ASP.Blog.API.DAL.Entities;
using ASP.Blog.API.DAL.Extentions;
using ASP.Blog.API.DAL.Repositories;
using ASP.Blog.API.Data;
using ASP.Blog.API.Data.Entities;
using ASP.Blog.API.Services;
using ASP.Blog.API.Services.IServices;
using ASP.Blog.API.Validators;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace ASP.Blog.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string connection = Configuration.GetConnectionString("HP_Connection");

            services
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<ArticleViewModelValidator>())
                .AddDbContext<BlogContext>(options => options.UseSqlServer(connection))
                .AddUnitOfWork()
                .AddCustomRepository<Article, ArticleRepository>()
                .AddTransient<IArticleService, ArticleService>()
                .AddCustomRepository<Comment, CommentRepository>()
                .AddTransient<ICommentService, CommentService>()
                .AddCustomRepository<Tag, TagRepository>()
                .AddTransient<ITagService, TagService>()
                .AddCustomRepository<Article_Tags, Article_TagsRepository>()
                .AddCustomRepository<User, UserRepository>()
                .AddTransient<IUserService, UserService>()
                .AddCustomRepository<UserRole, UserRoleRepository>()
                .AddTransient<IRoleService, RoleService>()
                .AddIdentity<User, UserRole>(opts =>
                {
                    opts.Password.RequiredLength = 6;
                    opts.Password.RequireNonAlphanumeric = false;
                    opts.Password.RequireDigit = false;
                })
                .AddEntityFrameworkStores<BlogContext>()
            ;
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ASP.Blog.API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ASP.Blog.API v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
