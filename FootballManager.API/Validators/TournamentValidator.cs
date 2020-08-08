using FluentValidation;
using FootballManager.API.DTOs.Command;
using System;

namespace FootballManager.API.Validators
{
    public class TournamentValidator : AbstractValidator<TournamentCommand>
    {
        public TournamentValidator()
        {
            RuleFor(x => x.TournamentName).NotEmpty()
                .Must(tn => Char.IsUpper(tn[0]))
                .WithMessage("The tournament's name must be in upper case");
        }
    }
}