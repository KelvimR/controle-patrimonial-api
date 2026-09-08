using CPI.API.CPI.Application.DTOs;
using CPI.API.CPI.Domain.Entities;

namespace CPI.API.CPI.Application.Interfaces;

public interface IUserAuthService
{
    User? FindByUserName(string username);
    User Create(AccountCredentialsDTO dto);
    User Update(User user);
    bool RevokeToken(string username);

}
