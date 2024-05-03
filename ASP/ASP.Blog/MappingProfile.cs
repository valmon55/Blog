using ASP.Blog.BLL.ViewModels;
using ASP.Blog.BLL.ViewModels.Article;
using ASP.Blog.BLL.ViewModels.Tag;
using ASP.Blog.Data.Entities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASP.Blog.BLL
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<RegisterViewModel, User>()
                .ForMember(x => x.BirthDate, opt => opt.MapFrom(c => new DateTime((int)c.Year, (int)c.Month, (int)c.Day)))
                .ForMember(x => x.Email, opt => opt.MapFrom(c => c.Email))
                .ForMember(x => x.UserName, opt => opt.MapFrom(c => c.Login));
            CreateMap<LoginViewModel, User>();
            CreateMap<ArticleViewModel,Article>()
                .ForMember(x => x.Content, opt => opt.MapFrom(c => c.Content))
                .ForMember(x => x.User, opt => opt.MapFrom(c => c.User))
                .ForMember(x => x.UserId, opt => opt.MapFrom(c => c.User.Id));
            CreateMap<Article, ArticleViewModel>()
                .ForMember(x => x.Id, opt => opt.MapFrom(c => c.ID))
                .ForMember(x => x.Content, opt => opt.MapFrom(c => c.Content))
                .ForMember(x => x.User, opt => opt.MapFrom(c => c.User));
                //.ForMember(x => x.User.Id, opt => opt.MapFrom(c => c.UserId));
            CreateMap<Tag, TagViewModel>()
                .ForMember(x => x.Id, opt => opt.MapFrom(c => c.ID))
                .ForMember(x => x.Tag_Name, opt => opt.MapFrom(c => c.Tag_Name));
            CreateMap<TagViewModel, Tag>()
                .ForMember(x => x.ID, opt => opt.MapFrom(c => c.Id))
                .ForMember(x => x.Tag_Name, opt => opt.MapFrom(c => c.Tag_Name));
        }
    }
}
