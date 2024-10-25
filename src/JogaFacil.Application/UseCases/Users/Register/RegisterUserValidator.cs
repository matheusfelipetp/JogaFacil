﻿using FluentValidation;
using JogaFacil.Comunication.Requests.Users;
using JogaFacil.Exception.Resources;

namespace JogaFacil.Application.UseCases.Users.Register
{
    public class RegisterUserValidator : AbstractValidator<RequestRegisterUserJson>
    {
        public RegisterUserValidator()
        {
            RuleFor(user => user.Name).NotEmpty().WithMessage(ResourceErrorMessages.NAME_EMPTY);
            RuleFor(user => user.Email)
                .NotEmpty()
                .WithMessage(ResourceErrorMessages.EMAIL_EMPTY)
                .EmailAddress()
                .When(user => string.IsNullOrWhiteSpace(user.Email) == false, ApplyConditionTo.CurrentValidator)
                .WithMessage(ResourceErrorMessages.EMAIL_INVALID);
            RuleFor(user => user.Password).SetValidator(new PasswordValidator<RequestRegisterUserJson>());
        }
    }
}