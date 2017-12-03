using MediatR;
using Microsoft.IdentityModel.Tokens;
using System;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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

        public class Handler : IRequestHandler<Command, string>
        {
            public Task<string> Handle(Command message, CancellationToken cancellationToken)
            {
                if (message.LoginDto.Email == "test" && message.LoginDto.Password == "herpderpwat")
                {
                    var claims = new[]
                    {
                        new Claim(ClaimTypes.Name, message.LoginDto.Email)
                    };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superlongandawesomesecuritykeynoonewillknow"));
                    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    var token = new JwtSecurityToken(issuer: "localhost", audience: "all", claims: claims, expires: DateTime.Now.AddMinutes(30), signingCredentials: creds);

                    return Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
                }

                return Task.FromResult("u suk");
            }
        }
    }
}
