using AutoMapper;
using MyRecipeBook.Application.Services.AutoMapper;
using MyRecipeBook.Application.Services.Cryptography;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;


namespace MyRecipeBook.Application.UseCases.User.Register;

public class RegisterUserUseCase : IRegisterUserUseCase // Regra de Negocio
{
    private readonly IUserReadOnlyRepository _ReadOnlyRepository;
    private readonly IUserWriteOnlyRepository _WriteOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly PasswordEncripter  _passwordEncripter;

    public RegisterUserUseCase(
        IUserReadOnlyRepository readOnlyRepository,       
        IUserWriteOnlyRepository writeOnlyRepository,
        IUnitOfWork unitOfWork,
        PasswordEncripter passwordEncripter,
        IMapper mapper)
    {
        _ReadOnlyRepository = readOnlyRepository;
        _WriteOnlyRepository = writeOnlyRepository;
        _mapper = mapper;
        _passwordEncripter = passwordEncripter;
        _unitOfWork = unitOfWork;

    }


    public async Task<ResponseRegisteredUserJson> Execute(RequestRegisterUserJson request)
    {

        // Validar a request
        await Validate(request);

        var user = _mapper.Map<Domain.Entities.User>(request);

        // Criptografar a senha
        user.Password = _passwordEncripter.Encrypt(request.Password);


        // Registrar o usuario no banco de dados

        await _WriteOnlyRepository.Add(user);

        await _unitOfWork.Commit();


        return new ResponseRegisteredUserJson
        {
            Name = request.Name,
        };
    }


    private async Task Validate(RequestRegisterUserJson request)
    {
        // Implementar a validação da request
        var validator = new RegisterUserValidator();
        var result = validator.Validate(request);

        var emailExist = await _ReadOnlyRepository.ExistActiveUserWithEmail(request.Email);
        if (emailExist)
        {
            result.Errors.Add(new FluentValidation.Results.ValidationFailure(string.Empty, ResourceMenssageException.EMAIL_ALREADY_REGISTERED));
        }

        if (result.IsValid == false)
        {
            var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();

            throw new ErrorOnValidationException(errorMessages);
        }
    }
}