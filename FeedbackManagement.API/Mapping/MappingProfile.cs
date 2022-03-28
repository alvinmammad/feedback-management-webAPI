using AutoMapper;
using Entity;
using FeedbackManagement.API.DTO;
using FeedbackManagement.API.DTO.CheckFeedbackNoteDTOs;
using FeedbackManagement.API.DTO.HRFeedbackDTOs;
using FeedbackManagement.API.DTO.HRFeedbackDTOs.HRFeedbackListDTOs;
using FeedbackManagement.API.DTO.LoginDTOs;
using FeedbackManagement.API.DTO.RegisterDTOs;
using FeedbackManagement.API.DTO.RoleDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeedbackManagement.API.Mapping
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<Feedback, FeedbackDTO>().ForMember(f => f.FeedbackCategory, opt => opt.MapFrom(src => src.FeedbackCategory)).ReverseMap();
            CreateMap<FeedbackCategories, FeedbackCategoryDTO>().ReverseMap();
            //CreateMap<FeedbackDTO,Feedback>().ReverseMap();
            //CreateMap<FeedbackCategoryDTO, FeedbackCategories>().ReverseMap();
            //CreateMap<Department, DepartmentDTO>();
            CreateMap<SaveFeedbackDTO, Feedback>();
            CreateMap<SaveFeedbackCategoryDTO, FeedbackCategories>();
            CreateMap<RegisterUserDTO, AppUser>();
            CreateMap<DepartmentListDTO, Department>();
            CreateMap<DTO.RegisterDTOs.RoleListDTO, AppRole>();
            CreateMap<LoginUserDTO, AppUser>();
            CreateMap<AppRole, DTO.RoleDTOs.RoleListDTO>();
            CreateMap<DTO.FeedbackDetailDTOs.FeedbackDTO,Feedback>();
            CreateMap<DTO.FeedbackDetailDTOs.FeedbackCategoryDetailDTO, FeedbackCategories>();
            CreateMap<Feedback,DTO.FeedbackDetailDTOs.FeedbackDTO>();
            CreateMap<FeedbackCategories,DTO.FeedbackDetailDTOs.FeedbackCategoryDetailDTO>();

            CreateMap<Department,DepartmentCheckDTO>();
            CreateMap<FeedbackCategories,FeedbackCategoryCheckDTO>();
            CreateMap<Feedback,FeedbackCheckDTO>().ForMember(f=>f.FeedbackCategory,opt=>opt.MapFrom(src=>src.FeedbackCategory));
            CreateMap<UserFeedback,UserFeedbackCheckDTO>();

            CreateMap<ListDepartmentDTO, Department>().ReverseMap();
            CreateMap<ListFeedbackCategoryDTO, FeedbackCategories>().ReverseMap();
            CreateMap<ListFeedbackDTO, Feedback>().ReverseMap();
            CreateMap<ListUserFeedbackDTO, UserFeedback>().ReverseMap();




        }
    }
}
