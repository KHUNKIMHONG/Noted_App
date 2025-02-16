using API_BackEnd.Data;
using API_BackEnd.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace API_BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    // Route: localhost://api/UserAuth
    public class UserAuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signinManager;
        private readonly string? _jwtKey;
        private readonly string? _jwtIssuer;
        private readonly string? _jwtAudience;
        private readonly int _jwtExpiry;

        // Constructor to inject dependencies
        public UserAuthController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signinManager,
            IConfiguration configuration) // Fixed typo here
        {
            _userManager = userManager;
            _signinManager = signinManager;

            // Retrieve JWT settings from appsettings.json
            _jwtKey = configuration["Jwt:Key"];
            _jwtIssuer = configuration["Jwt:Issuer"];
            _jwtAudience = configuration["Jwt:Audience"];
            _jwtExpiry = int.Parse(configuration["Jwt:ExpiryMinutes"]); // Parse the expiry to int
        }

        // Route:localhost://api/UserAuth/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel registerModel)
        {
            if (registerModel == null
                || string.IsNullOrEmpty(registerModel.Name)
                || string.IsNullOrEmpty(registerModel.Email)
                || string.IsNullOrEmpty(registerModel.Password)
                )
            {
                return BadRequest("Invalid Registration details!!");
            }

            var existingUser = await _userManager.FindByEmailAsync(registerModel.Email);

            if (existingUser != null)
            {
                return Conflict("Email already Exist");
            }

            var user = new ApplicationUser
            {
                UserName = registerModel.Email,
                Email = registerModel.Email,
                Name = registerModel.Name,
            };

            var result = await _userManager.CreateAsync(user, registerModel.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return CreatedAtAction(nameof(Register), new { id = user.Id }, "Registration successful.");
        }

        //Route:localhost://api/UserAuth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            var user = await _userManager.FindByEmailAsync(loginModel.Email);

            if (user == null)
            {
                return Unauthorized(new { success = false, message = "Invalid username or email!" });
            }

            var result = await _signinManager.CheckPasswordSignInAsync(user, loginModel.Password, false);

            if (!result.Succeeded)
            {
                return Unauthorized(new { success = false, message = "Invalid password!" });
            }

            var token = GenerateJWTToken(user);

            return Ok(new { success = true, token });
        }

        private string GenerateJWTToken(ApplicationUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtKey); // Get key from appsettings.json

            var claims = new List<Claim>
        {
        new Claim(JwtRegisteredClaimNames.Sub, user.Id), // Subject
        new Claim(JwtRegisteredClaimNames.Email, user.Email),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // Unique identifier for token
        new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64) // Issued at
        };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(_jwtExpiry), // Token expires in 1 hour
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        // Route: localhost://api/UserAuth/getUser/{id}
        [HttpGet("getUser/{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound(new { success = false, message = "User not found!" });
            }

            return Ok(new
            {
                success = true,
                user = new
                {
                    id = user.Id,
                    name = user.Name,
                    email = user.Email,
                }
            });
        }

        // Route: localhost://api/UserAuth/updateUser/{id}
        [HttpPut("updateUser")]
        public async Task<IActionResult> UpdateUserProfile([FromBody] UpdateUserModel model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { success = false, message = "Unauthorized access!" });
            }

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound(new { success = false, message = "User not found!" });
            }

            // Update user details
            user.Name = model.Name ?? user.Name;

            // Prevent email change for security reasons (optional)
            // user.Email = model.Email ?? user.Email;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                return BadRequest(new { success = false, errors = result.Errors });
            }

            return Ok(new { success = true, message = "Profile updated successfully!" });
        }


        // Route: localhost://api/UserAuth/updatePassword
        [HttpPut("updatePassword")]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordModel model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { success = false, message = "Unauthorized access!" });
            }

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound(new { success = false, message = "User not found!" });
            }

            // Check if the current password is correct
            var result = await _userManager.CheckPasswordAsync(user, model.CurrentPassword);

            if (!result)
            {
                return Unauthorized(new { success = false, message = "Current password is incorrect!" });
            }

            // Change the password
            var changePasswordResult = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

            if (!changePasswordResult.Succeeded)
            {
                return BadRequest(new { success = false, errors = changePasswordResult.Errors });
            }

            return Ok(new { success = true, message = "Password updated successfully!" });
        }


        // Route: localhost://api/UserAuth/logout
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            // Sign out the user from the current session
            await _signinManager.SignOutAsync();

            return Ok(new { success = true, message = "User logged out successfully!" });
        }
    }
}
