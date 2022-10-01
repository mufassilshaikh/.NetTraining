using JWT.Authentication.Server.Core.Contract.Repositories;
using JWT.Authentication.Server.Core.Entities;
using JWT.Authentication.Server.Infrastructure.Extensions;
using JWT.Authentication.Server.Infrastructure.VM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace JWT.Authentication.Server.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _config;


        public AuthController(IUserRepository userRepository, IConfiguration config)
        {
            _userRepository = userRepository;
            _config = config;
        }

        [Route("register")]
        [HttpPost]
        public async Task<ActionResult> Register([FromBody]UserVm userVm)
        {
            var passwordSalt = GenerateSalt();
            userVm.Password += passwordSalt;
            var passwordHash = GenerateHashPassword(userVm.Password);
            User user = new()
            {
                FullName = userVm.FullName,
                Email = userVm.Email,
                Password = passwordHash,
                PasswordSalt = passwordSalt
            };
            await _userRepository.RegisterUser(user);
            return Ok(true);
        }

        [Route("login")]
        [HttpPost]
        public async Task<ActionResult> Login([FromBody]LoginVm userVm)
        {
            var user = await _userRepository.GetUserDetails(userVm.Email);
            if (user is null)
                return BadRequest("Invalid Email address or Password");
            userVm.Password += user.PasswordSalt;
            var passwordHash = GenerateHashPassword(userVm.Password);
            if (passwordHash == user.Password)
            {
                var token = GenerateToken(user);
                return Ok(token);
            }
            return BadRequest("Invalid Email address or Password");
        }

        private string GenerateHashPassword(string password)
        {
            string machineKey = _config["MachineKey"].ToString();
            var hmac = new HMACSHA1()
            {
                Key = machineKey.HexToByte()
            };
            return Convert.ToBase64String(hmac.ComputeHash(password.GetByteArray()));
        }

        private static string GenerateSalt()
        {
            int saltLength = 8;
            byte[] salt = new byte[saltLength];
            using (var random = new RNGCryptoServiceProvider())
            {
                random.GetNonZeroBytes(salt);
            }
            return Convert.ToBase64String(salt);
        }

        private string GenerateToken(User user)
        {
            var claims = new[]
            {
                new Claim("UserId", user.UserId.ToString()),
                new Claim("FullName", user.FullName),
                new Claim("Email", user.Email),
                new Claim(ClaimTypes.Role,"admin")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: signIn);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
