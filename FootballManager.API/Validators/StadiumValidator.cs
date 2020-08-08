using FluentValidation;
using FootballManager.API.DTOs.Command;
using System;

namespace FootballManager.API.Validators
{
    public class StadiumValidator : AbstractValidator<StadiumCommand>
    {
        public StadiumValidator()
        {
            RuleFor(x => x.StadiumName).NotEmpty()
                .Must(sn => Char.IsUpper(sn[0]))
                .WithMessage("The stadium's name must be in upper case");

            RuleFor(x => x.Capacity).NotEmpty();
        }
    }
}