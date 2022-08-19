﻿using FluentValidation;

namespace Domain.Aggregates;

public class UserValidator : AbstractValidator<User>
{
    public UserValidator()
    {
        RuleFor(user => user.Id)
            .NotEmpty();
        
        RuleFor(user => user.Password)
            .NotNull()
            .NotEmpty()
            .Equal(user => user.PasswordConfirmation);

        RuleFor(user => user.PasswordConfirmation)
            .NotNull()
            .NotEmpty()
            .Equal(user => user.Password);
    }
}