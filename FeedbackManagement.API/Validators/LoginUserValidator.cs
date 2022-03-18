using FeedbackManagement.API.DTO.LoginDTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeedbackManagement.API.Validators
{
    public class LoginUserValidator: AbstractValidator<LoginUserDTO>
    {
        public LoginUserValidator()
        {
            RuleFor(u => u.UserName).NotEmpty().WithMessage("İstifadəçi adınızı qeyd edin");
            RuleFor(u => u.Password).NotEmpty().WithMessage("Şifrənizi qeyd edin");

        }
    }
}
