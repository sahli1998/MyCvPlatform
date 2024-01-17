using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MonCv.Helpers;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MonCv.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationContrroller : ControllerBase
    {
        private readonly JWT _jwt;
        public AuthorizationContrroller(IOptions<JWT> jwt )
        {
            _jwt = jwt.Value;
        }

        [HttpPost("validate")]
        [AllowAnonymous] 
        public IActionResult ValidateJwt([FromBody] JwtValidationRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid request body");
            }

            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var claimsPrincipal = tokenHandler.ValidateToken(request.Token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key)),
                    ValidIssuer = _jwt.Issuer,
                    ValidAudience = _jwt.Audience,
                }, out SecurityToken validatedToken);

                // Récupérer les informations sur l'utilisateur depuis les claims
                var username = claimsPrincipal.FindFirst("UserName")?.Value;
                var roles = claimsPrincipal.FindAll(ClaimTypes.Role)?.Select(c => c.Value).ToList();

                return Ok(new { Username = username, Roles = roles });
            }
            catch (SecurityTokenException)
            {
                return BadRequest("Invalid token");
            }
        }
    }
    public class JwtValidationRequest
    {
        [Required]
        public string Token { get; set; }
    }
}
