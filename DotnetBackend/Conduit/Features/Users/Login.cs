using MediatR;
using Microsoft.IdentityModel.Tokens;
using System;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Conduit.Features.Users
{
    public class Login
    {
        public class LoginDto
        {
            [Required]
            public string Email { get; set; }
            [Required]
            public string Password { get; set; }
        }

        public class Command : IRequest<string>
        {
            public LoginDto LoginDto { get; set; }
        }

        public class Handler : RequestHandler<Command, string>
        {
            protected override string HandleCore(Command request)
            {
                if (request.LoginDto.Email == "test" && request.LoginDto.Password == "herpderpwat")
                {
                    var claims = new[]
                    {
                        new Claim(ClaimTypes.Name, request.LoginDto.Email)
                    };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superlongandawesomesecuritykeynoonewillknow"));
                    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    var token = new JwtSecurityToken(issuer: "localhost", audience: "all", claims: claims, expires: DateTime.Now.AddMinutes(30), signingCredentials: creds);

                    return new JwtSecurityTokenHandler().WriteToken(token);
                }

                return "u suk";
            }
        }
    }
}
