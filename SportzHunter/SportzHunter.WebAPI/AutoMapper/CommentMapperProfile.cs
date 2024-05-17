using AutoMapper;
using SportzHunter.Model;
using SportzHunter.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportzHunter.WebAPI.AutoMapper
{
    public class CommentMapperProfile : Profile
    {
        public CommentMapperProfile()
        {
            CreateMap<Comment, CommentView>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Player.User.Username));
            CreateMap<PutCommentModelView, Comment>();
            CreateMap<PostCommentModelView, Comment>();
        }
    }
}