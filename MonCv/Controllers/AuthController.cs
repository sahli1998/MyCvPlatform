using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MonCv.IRepositories;
using MonCv.Model;

namespace MonCv.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IRepoAuth _repoAuth;
        public AuthController(IRepoAuth repoAuth)
        {
            _repoAuth = repoAuth;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result =  _repoAuth.Register(model);

            if (!result.IsAuthenticated)
                return BadRequest(result.Message);

            return Ok(result);
        }


        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = _repoAuth.Login(model);

            if (!result.IsAuthenticated)
                return BadRequest(result.Message);

            return Ok(result);
        }
    }
}
