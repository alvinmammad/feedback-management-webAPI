using FeedbackManagement.API.DTO.RegisterDTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeedbackManagement.API.Validators
{
    public class RegisterUserValidator:AbstractValidator<RegisterUserDTO>
    {
        public RegisterUserValidator()
        {
            RuleFor(u => u.FirstName).NotEmpty().WithMessage("Adınızı qeyd edin");
            RuleFor(u => u.Surname).NotEmpty().WithMessage("Soyadınızı qeyd edin");
            RuleFor(u => u.Email).NotEmpty().WithMessage("E-poçt adresinizi qeyd edin").
                EmailAddress().WithMessage("Yazılan e-poçt adres növü düzgün deyil");
            RuleFor(u => u.UserName).NotEmpty().WithMessage("İstifadəçi adınızı qeyd edin");
            RuleFor(u => u.Password).NotEmpty().WithMessage("Şifrənizi qeyd edin");
            RuleFor(u => u.DepartmentID).NotEmpty().WithMessage("Şöbə boş ola bilməz");
            RuleFor(u => u.RoleId).NotEmpty().WithMessage("Vəzifə boş ola bilməz");

        }
    }
}
