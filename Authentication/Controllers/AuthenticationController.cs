using Authentication.Models;
using Authentication.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private UserManager<ApplicationUser> userManager;
        private IConfiguration configuration;

        public AuthenticationController(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.configuration = configuration;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginModel login)
        {
            if (!ModelState.IsValid)
            {
                var errorsModel = ModelState.Select(x => x.Value.Errors).ToList();
                var errors = new List<string>();
                foreach (var error in errorsModel)
                    errors.Add(error.Select(x => x.ErrorMessage).First());

                return BadRequest(errors);
            }

            try
            {
                var user = await userManager.FindByEmailAsync(login.Email);
                if (user == null)
                    return BadRequest("User or password not valid");

                var isPasswordValid = await userManager.CheckPasswordAsync(user, login.Password);
                if (!isPasswordValid)
                    return Unauthorized("User or password not valid");


                var claim = new[] {
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName)
                };
                var signinKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SigninKey"]));
                var expirationInMinutes =Convert.ToInt32(configuration["Jwt:ExpiryInMInutes"]);

                var token = new JwtSecurityToken(
                    issuer: configuration["Jwt:Site"],
                    audience: configuration["Jwt:Site"],
                    expires: DateTime.UtcNow.AddMinutes(expirationInMinutes),
                    signingCredentials: new SigningCredentials(signinKey, SecurityAlgorithms.HmacSha256)
                );

                return Ok(new TokenModel() { Token = new JwtSecurityTokenHandler().WriteToken(token), Expiration = token.ValidTo.ToLocalTime().ToString() });
            }
            catch (Exception ex)
            {
                return Unauthorized();
            }
        }

        [HttpPost]
        [Route("createuser")]
        public async Task<IActionResult> Create([FromBody] CreateUserModel newUser)
        {
            if (!ModelState.IsValid)
            {
                var errorsModel = ModelState.Select(x => x.Value.Errors).ToList();
                var errors = new List<string>();
                foreach (var error in errorsModel)
                    errors.Add(error.Select(x => x.ErrorMessage).First());

                return BadRequest(errors);
            }

            var user = new ApplicationUser()
            {
                Email = newUser.Email,
                UserName = newUser.UserName,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            try
            {
                var userCreated = await userManager.CreateAsync(user, newUser.Password);

                if (userCreated.Succeeded)
                    return CreatedAtAction(nameof(Create), user);

                else
                    return StatusCode(StatusCodes.Status500InternalServerError, userCreated.Errors);
            }
            catch (Exception ex)
            {
                //save this guid:
                var guid = Guid.NewGuid().ToString();
                return StatusCode(StatusCodes.Status500InternalServerError, $"Service was unable to create the resource, error Code: {guid}");
            }
        }
    }
}