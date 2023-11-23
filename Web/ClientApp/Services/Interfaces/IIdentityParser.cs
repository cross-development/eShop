using System.Security.Principal;

namespace ClientApp.Services.Interfaces;

public interface IIdentityParser<out T>
{
    T Parse(IPrincipal principal);
}