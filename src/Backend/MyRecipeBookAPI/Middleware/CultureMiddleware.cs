using System.Globalization;

namespace MyRecipeBookAPI.Middleware;

public class CultureMiddleware
{
    private readonly RequestDelegate _next;
    public CultureMiddleware(RequestDelegate next)
    {
        _next = next;
    }



    public async Task Invoke(HttpContext context)
    {
        var supportedCultures = CultureInfo.GetCultures(CultureTypes.AllCultures);
        var requestCulture = context.Request.Headers.AcceptLanguage.FirstOrDefault();

        // Setar uma cultura padrão (inglês) caso nenhuma seja fornecida
        var cultureinfo = new CultureInfo("en");

        if (string.IsNullOrWhiteSpace(requestCulture) == false && supportedCultures.Any(c => c.Name.Equals(requestCulture)))
        {
            cultureinfo = new CultureInfo(requestCulture);
        }

        CultureInfo.CurrentCulture = cultureinfo;
        CultureInfo.CurrentUICulture = cultureinfo;

        await _next(context);
    }

}

