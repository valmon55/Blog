﻿using ASP.Blog.BLL.ViewModels;
using ASP.Blog.BLL.ViewModels.Article;
using ASP.Blog.BLL.ViewModels.Comment;
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
            CreateMap<UserViewModel, User>()
                .ForMember(x => x.First_Name, opt => opt.MapFrom(c => c.First_Name))
                .ForMember(x => x.Last_Name, opt => opt.MapFrom(c => c.Last_Name))
                .ForMember(x => x.Middle_Name, opt => opt.MapFrom(c => c.Middle_Name))
                .ForMember(x => x.Email, opt => opt.MapFrom(c => c.Email))
                .ForMember(x => x.BirthDate, opt => opt.MapFrom(c => new DateTime(c.Year, c.Month, c.Day)));
            CreateMap<User, UserViewModel>()
                .ForMember(x => x.Id, opt => opt.MapFrom(c => c.Id))
                .ForMember(x => x.First_Name, opt => opt.MapFrom(c => c.First_Name))
                .ForMember(x => x.Last_Name, opt => opt.MapFrom(c => c.Last_Name))
                .ForMember(x => x.Middle_Name, opt => opt.MapFrom(c => c.Middle_Name))
                .ForMember(x => x.Email, opt => opt.MapFrom(c => c.Email));
                //.ForAllMembers(x, => x.BirthDate, opt => opt.MapFrom(c => c.BirthDate));
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
            CreateMap<Comment, CommentViewModel>()
                .ForMember(x => x.Id, opt => opt.MapFrom(c => c.ID))
                .ForMember(x => x.Comment, opt => opt.MapFrom(c => c.Comment_Text))
                .ForMember(x => x.CommentDate, opt => opt.MapFrom(c => c.CommentDate))
                .ForMember(x => x.UserId, opt => opt.MapFrom(c => c.UserId))
                .ForMember(x => x.ArticleId, opt => opt.MapFrom(c => c.ArticleId));
            CreateMap<CommentViewModel, Comment>()
                .ForMember(x => x.ID, opt => opt.MapFrom(c => c.Id))
                .ForMember(x => x.Comment_Text, opt => opt.MapFrom(c => c.Comment))
                .ForMember(x => x.CommentDate, opt => opt.MapFrom(c => c.CommentDate))
                .ForMember(x => x.UserId, opt => opt.MapFrom(c => c.UserId))
                .ForMember(x => x.ArticleId, opt => opt.MapFrom(c => c.ArticleId));
        }
    }
}
