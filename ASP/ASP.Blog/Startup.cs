using ASP.Blog.BLL;
using ASP.Blog.Data;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using ASP.Blog.Data.Entities;
using ASP.Blog.DAL.UoW;
using ASP.Blog.DAL.Entities;

namespace ASP.Blog
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
            string connection = Configuration.GetConnectionString("DefaultConnection");
            var mapperConfig = new MapperConfiguration(v =>
            {
                v.AddProfile(new MappingProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
            
            services.AddMvc();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            //services.AddUoW();
            //services.AddUnitOfWork();
            
            services
                .AddDbContext<BlogContext>(options => options.UseSqlServer(connection))
                .AddIdentity<User, UserRole>(opts =>
                {
                    opts.Password.RequiredLength = 6;
                    opts.Password.RequireNonAlphanumeric = false;
                    opts.Password.RequireDigit = false;
                })                                
                .AddEntityFrameworkStores<BlogContext>()
            ;

            services.AddControllersWithViews();
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseDatabaseErrorPage():
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            var cachePeriod = "0";
            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = ctx =>
                {
                    ctx.Context.Response.Headers.Append("Cache-Control", $"public, max-age = {cachePeriod}");
                }
            }) ;

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
