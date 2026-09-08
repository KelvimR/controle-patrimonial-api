using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace CPI.API.CPI.Application.DTOs;

public class AccountCredentialsDTO
{
    public AccountCredentialsDTO()
    {        
    }

    public string UserName { get; set; }
    public string Password { get; set; }
    public string FullName { get; set; }
}
