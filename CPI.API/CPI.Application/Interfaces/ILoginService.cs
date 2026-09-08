using CPI.API.CPI.Application.DTOs;

namespace CPI.API.CPI.Application.Interfaces;

public interface ILoginService
{
    TokenDTO? ValidateCredentials(UserDTO user);
    TokenDTO? ValidateCredentials(TokenDTO token);
    bool RevokeToken(string userName);
    AccountCredentialsDTO? Create(AccountCredentialsDTO user);
}
