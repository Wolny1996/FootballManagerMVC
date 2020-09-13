using FluentValidation;
using FootballManager.API.DTOs.Command;
using System;

namespace FootballManager.API.Validators
{
    public class FootballerValidator : AbstractValidator<FootballerCommand>
    {
        public FootballerValidator()
        {
            RuleFor(x => x.Name).NotEmpty()
                .Must(n => Char.IsUpper(n[0]))
                .WithMessage("The name must be in upper case");
            
            RuleFor(x => x.Surname).NotEmpty()
                .Must(sn => Char.IsUpper(sn[0]))
                .WithMessage("The surname must be in upper case");
        }
    }
}