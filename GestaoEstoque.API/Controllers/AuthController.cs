using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NetDevPack.Identity.Interfaces;
using NetDevPack.Identity.Jwt.Model;
using NetDevPack.Identity.Jwt;
using NetDevPack.Identity.Model;
using Microsoft.AspNetCore.Authorization;

namespace GestaoEstoque.API.Controllers
{
    [Route("api/account")]
    public class AuthController : MainController
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IJwtBuilder _jwtBuilder;

        public AuthController(SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            IJwtBuilder jwtBuilder)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _jwtBuilder = jwtBuilder;
        }

        [HttpPost("registro")]
        [AllowAnonymous]
        public async Task<ActionResult> Register(RegisterUser registerUser)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var user = new IdentityUser
            {
                UserName = registerUser.Email,
                Email = registerUser.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, registerUser.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok(_jwtBuilder.WithEmail(user.Email)
                                    .WithJwtClaims()
                                    .WithUserClaims()
                                    .WithUserRoles()
                                    .BuildUserResponse()
                                    .Result);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult> Login(LoginUser loginUser)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            if (loginUser is null) return BadRequest("Usuário não informado.");


            var result = await _signInManager.PasswordSignInAsync(loginUser.Email, loginUser.Password, false, true);

            if (result.Succeeded)
            {
                return Ok(_jwtBuilder.WithEmail(loginUser.Email)
                                    .WithJwtClaims()
                                    .WithUserClaims()
                                    .WithUserRoles()
                                    .BuildUserResponse()
                                    .Result);
            }

            if (result.IsLockedOut)
            {
                AddError("Este usuário está bloqueado.");
                return CustomResponse();
            }

            AddError("Usuário ou senha incorretos");
            return CustomResponse();
        }


        [HttpPost("refresh-token")]
        [AllowAnonymous]
        public async Task<ActionResult> RefreshToken([FromForm] string refreshToken)
        {
            var tokenValidation = await _jwtBuilder.ValidateRefreshToken(refreshToken);

            if (!tokenValidation)
            {
                ModelState.AddModelError("RefreshToken", "Expired token");
                return BadRequest(ModelState);
            }

            return CustomResponse(await _jwtBuilder
                .WithUserId(tokenValidation.UserId)
                .WithJwtClaims()
                .WithUserClaims()
                .WithUserRoles()
                .WithRefreshToken()
                .BuildToken());

        }
        private Task<UserResponse> GetUserResponse(string email)
        {
            return _jwtBuilder
                .WithEmail(email)
                .WithJwtClaims()
                .WithUserClaims()
                .WithUserRoles()
                .WithRefreshToken()
                .BuildUserResponse();
        }

        private Task<string> GetFullJwt(string email)
        {
            return _jwtBuilder
                .WithEmail(email)
                .WithJwtClaims()
                .WithUserClaims()
                .WithUserRoles()
                .WithRefreshToken()
                .BuildToken();
        }

        private Task<string> GetJwtWithoutClaims(string email)
        {
            return _jwtBuilder
                .WithEmail(email)
                .WithRefreshToken()
                .BuildToken();
        }

        private Task<string> GetJwtWithUserClaims(string email)
        {
            return _jwtBuilder
                .WithEmail(email)
                .WithUserClaims()
                .WithRefreshToken()
                .BuildToken();
        }
    }

}
