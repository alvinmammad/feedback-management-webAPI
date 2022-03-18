using Entity;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeedbackManagement.Business.ValidationRules
{
    public class FeedbackValidator:AbstractValidator<Feedback>
    {
        public FeedbackValidator()
        {
            RuleFor(f => f.CustomerName).NotEmpty().WithMessage("Zəhmət olmasa adınızı boş qoymayın.");
            RuleFor(f => f.CustomerSurname).NotEmpty().WithMessage("Zəhmət olmasa soyadınızı boş qoymayın.");
            RuleFor(f => f.CustomerEmail).NotEmpty().WithMessage("Zəhmət olmasa e-mail adresinizi boş qoymayın.");
            RuleFor(f => f.FeedbackDesc).NotEmpty().WithMessage("Zəhmət olmasa müraciətinizi qeyd edin.");
            RuleFor(f => f.FeedbackCategoryID).NotEmpty().WithMessage("Zəhmət olmasa müraciətin növünü seçin.");
        }
    }
}
