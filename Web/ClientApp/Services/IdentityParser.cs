using System.Security.Claims;
using System.Security.Principal;
using ClientApp.Models;
using ClientApp.Services.Interfaces;

namespace ClientApp.Services;

public sealed class IdentityParser : IIdentityParser<ApplicationUser>
{
    private readonly ILogger<IdentityParser> _logger;

    public IdentityParser(ILogger<IdentityParser> logger)
    {
        _logger = logger;
    }

    public ApplicationUser Parse(IPrincipal principal)
    {
        if (principal is ClaimsPrincipal claims)
        {
            _logger.LogInformation("[IdentityParser: Parse] ==> PROVIDED PRINCIPAL IS THE CLAIMS_PRINCIPAL TYPE\n");

            return new ApplicationUser
            {
                CardHolderName = claims.Claims.FirstOrDefault(x => x.Type == "card_holder")?.Value ?? "",
                CardNumber = claims.Claims.FirstOrDefault(x => x.Type == "card_number")?.Value ?? "",
                Expiration = claims.Claims.FirstOrDefault(x => x.Type == "card_expiration")?.Value ?? "",
                CardType = int.Parse(claims.Claims.FirstOrDefault(x => x.Type == "missing")?.Value ?? "0"),
                City = claims.Claims.FirstOrDefault(x => x.Type == "address_city")?.Value ?? "",
                Country = claims.Claims.FirstOrDefault(x => x.Type == "address_country")?.Value ?? "",
                Email = claims.Claims.FirstOrDefault(x => x.Type == "email")?.Value ?? "",
                Id = claims.Claims.FirstOrDefault(x => x.Type == "sub")?.Value ?? "",
                LastName = claims.Claims.FirstOrDefault(x => x.Type == "last_name")?.Value ?? "",
                Name = claims.Claims.FirstOrDefault(x => x.Type == "name")?.Value ?? "",
                PhoneNumber = claims.Claims.FirstOrDefault(x => x.Type == "phone_number")?.Value ?? "",
                SecurityNumber = claims.Claims.FirstOrDefault(x => x.Type == "card_security_number")?.Value ?? "",
                State = claims.Claims.FirstOrDefault(x => x.Type == "address_state")?.Value ?? "",
                Street = claims.Claims.FirstOrDefault(x => x.Type == "address_street")?.Value ?? "",
                ZipCode = claims.Claims.FirstOrDefault(x => x.Type == "address_zip_code")?.Value ?? ""
            };
        }

        _logger.LogInformation("[IdentityParser: Parse] ==> PROVIDED PRINCIPAL IS NOT THE CLAIMS_PRINCIPAL TYPE\n");

        throw new ArgumentException(message: "The principal must be a ClaimsPrincipal", paramName: nameof(principal));
    }
}