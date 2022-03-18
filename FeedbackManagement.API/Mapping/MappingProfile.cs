using AutoMapper;
using Entity;
using FeedbackManagement.API.DTO;
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
            CreateMap<Feedback, FeedbackDTO>();
            CreateMap<FeedbackCategories, FeedbackCategoryDTO>();
            CreateMap<Department, DepartmentDTO>();
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
        }
    }
}
