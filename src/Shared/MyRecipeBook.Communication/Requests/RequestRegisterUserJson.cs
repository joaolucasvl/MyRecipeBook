
namespace MyRecipeBook.Communication.Requests;

public class RequestRegisterUserJson
{

    // Registros que vamos receber
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}