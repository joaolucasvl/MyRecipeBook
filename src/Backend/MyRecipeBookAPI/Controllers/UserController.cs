using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;

namespace MyRecipeBookAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        // Criar EndPoint para registrar um novo usuário

        [HttpPost]
        [ProducesResponseType(typeof(ResponseRegisteredUserJson),StatusCodes.Status201Created)] // Retornar código 201, e o body do tipo ResponseRegisteredUserJson
        public IActionResult Register(RequestRegisterUserJson request)
        {
            return Created(); // Retornar código 201 - Created e Receber os dados do usuário
        }

    }
}
