using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EmployeeManagement.Api.Abstractions;
using EmployeeManagement.Api.Domain.Models;
using Microsoft.IdentityModel.Tokens;

namespace EmployeeManagement.Api.Infrastructure.Authentication;

public class JwtService: IJwtService
{
    public const string SECRETE = "78ecc1b3-ff9a-4b56-aaa2-65fa98618d4c";
    private readonly JwtSecurityTokenHandler _tokenHandler;
    public JwtService()
    {
        _tokenHandler = new JwtSecurityTokenHandler();
    }

    public Result<string> GenerateToken(Employee employee)
    {
        var key = Encoding.ASCII.GetBytes(SECRETE);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity([
                new Claim("id", employee.Id.ToString()),
                new Claim("doc-number", employee.DocNumber),
                new Claim(ClaimTypes.Name, $"{employee.FirstName} {employee.LastName}"),
                new Claim(ClaimTypes.Role, employee.Role),
                new Claim(ClaimTypes.Email, employee.Email),
                new Claim("aud", "employee-management-audience"),
                new Claim("iss", "employee-management-issuer")
            ]),
            Expires = DateTime.UtcNow.AddMinutes(60),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = _tokenHandler.CreateToken(tokenDescriptor);
        return _tokenHandler.WriteToken(token);
    }
}

