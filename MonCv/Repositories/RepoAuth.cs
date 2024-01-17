using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MonCv.Helpers;
using MonCv.IRepositories;
using MonCv.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MonCv.Repositories
{
    public class RepoAuth : IRepoAuth
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JWT _jwt;

        public RepoAuth(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IOptions<JWT> jwt)
        {
            _userManager = userManager;
            _jwt = jwt.Value;
            _roleManager = roleManager;
        }

        public  AuthModel Register(RegisterModel model)
        {
            if(  _userManager.FindByNameAsync(model.UserName).Result is not null ||  _userManager.FindByEmailAsync(model.Email).Result is not null)
            {
                return new AuthModel { Message = "username or email exist" };

            }

            ApplicationUser user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber.ToString(),
            };

            var result = _userManager.CreateAsync(user,model.Password).Result;
            if(result.Succeeded)
            {
                var user1 =  _userManager.FindByEmailAsync(user.Email).Result;
                if(user1 != null)
                {
                    if (_userManager != null)
                    {
                        var role =  _userManager.AddToRoleAsync(user1, "User").Result;
                        // Reste du code...
                    }
                    else
                    {
                        // Gérer le cas où _userManager est null
                    }
                    var role1 =  _roleManager.RoleExistsAsync("User").Result;
                    if (role1 is true)
                    {
                        // Logique pour créer le rôle si nécessaire
                    }
                   

                }
              

                var jwtSecurityToken =  CreateJwtToken(user).Result;
                return new AuthModel
                {
                    IsAuthenticated = true,
                    Message = "Succefuly Registred",
                    Roles = new List<string> { "User" },
                    Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                    Username = user.UserName,
                    ExpiresOn = jwtSecurityToken.ValidTo,

                };
            }
            else
            {
                string Errors = "";
                 foreach(var error in result.Errors)
                {
                    Errors = Errors + " , " + error.Description;

                }
                return new AuthModel { Message = Errors };
            }




            
        }


        public AuthModel Login(LoginModel model)
        {
            var user = _userManager.FindByNameAsync(model.UserName).Result;
            if (user is null || !_userManager.CheckPasswordAsync(user, model.Password).Result)
            {
                return new AuthModel { Message = "Username or Password was incorrect." };
            }
            else
            {
                var jwtTokensecure = CreateJwtToken(user).Result;
                var roles = _userManager.GetRolesAsync(user).Result;

                return new AuthModel
                {
                    Username = user.UserName,
                    Email = user.Email,
                    Token = new JwtSecurityTokenHandler().WriteToken(jwtTokensecure),
                    ExpiresOn = jwtTokensecure.ValidTo,
                    IsAuthenticated = true,
                    Roles = roles.ToList(),


                };
            }
        }
        private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
                roleClaims.Add(new Claim("roles", role));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id),
                new Claim("UserName", user.UserName)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.Now.AddDays(_jwt.DurationInDays),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }


    }
}
