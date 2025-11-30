using FluentValidation; // Biblioteca para validação
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Exceptions;

namespace MyRecipeBook.Application.UseCases.User.Register;

public class RegisterUserValidator : AbstractValidator<RequestRegisterUserJson> // Validação para os dados que recebemos
{
    public RegisterUserValidator()
    {
        RuleFor(user => user.Name).NotEmpty().WithMessage(ResourceMenssageException.NAME_EMPTY); // Controlar mensagens de erro
        RuleFor(user => user.Email).NotEmpty().WithMessage(ResourceMenssageException.EMAIL_EMPTY);
        RuleFor(user => user.Email).EmailAddress().WithMessage(ResourceMenssageException.EMAIL_INVALID);
        RuleFor(user => user.Password.Length).GreaterThanOrEqualTo(6).WithMessage(ResourceMenssageException.PASSWORD_EMPTY);
    }

}


