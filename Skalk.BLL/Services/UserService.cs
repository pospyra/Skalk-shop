using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Skalk.BLL.Interfaces;
using Skalk.BLL.Services.Abstract;
using Skalk.BLL.Services.Helpers;
using Skalk.Common.DTO.User;
using Skalk.DAL.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Skalk.BLL.Services
{
    public class UserService : BaseService, IUserService
    {
        private readonly IConfiguration _configuration;
        private readonly IClaimAccessor _claimAccessor;

        public UserService(
            SkalkContext context,
            IMapper mapper, 
            IConfiguration configuration,
            IClaimAccessor claimAccessor) 
            : base(context, mapper)
        {
            _configuration = configuration;
            _claimAccessor = claimAccessor;
        }

        public async Task<NewUserDTO> Registration(NewUserDTO newUserDTO)
        {
            var isExistingUser = await _context.Users.AnyAsync(x => x.Login == newUserDTO.Login);
            if (isExistingUser)
            {
                throw new ArgumentException($"User with login {newUserDTO.Login} already exists");
            }

            var userEntity = _mapper.Map<User>(newUserDTO);
            var salt = SecurityHelper.GetRandomBytes();
            userEntity.Salt = Convert.ToBase64String(salt);
            userEntity.Password = SecurityHelper.HashPassword(newUserDTO.Password, salt);

            _context.Add(userEntity);
            await _context.SaveChangesAsync();

            var shoppingCart = new ShoppingCart { UserId = userEntity.Id };
            _context.ShoppingCarts.Add(shoppingCart);
            await _context.SaveChangesAsync();
            return newUserDTO;
        }

        public async Task<string> Login(LoginUserDTO userLogin)
        {
            var userEntity = await _context.Users
                .FirstOrDefaultAsync(user => user.Login == userLogin.Login)
                    ?? throw new InvalidOperationException($"The user doesn't exist");


            if (!SecurityHelper.ValidatePassword(userLogin.Password, userEntity.Password, userEntity.Salt))
            {
                throw new ArgumentException($"Invalid login or password");
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userEntity.Id.ToString()),
                new Claim(ClaimTypes.Name, userEntity.Name),
                 new Claim(ClaimTypes.Role, userEntity.Role.ToString())
            };

            string secretKey = _configuration["Token:SecretJWTKey"];

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                notBefore: DateTime.UtcNow,
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                    SecurityAlgorithms.HmacSha256
                    )
                );

            var authToken = new JwtSecurityTokenHandler().WriteToken(token);

            return authToken;
        }

        public async Task<UserDTO> GetUserByIdAsync(int id)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(x=> x.Id == id);

            return _mapper.Map<UserDTO>(user);
        }

        public async Task<UserDTO> GetCurrentUserAsync()
        {
            var claim = await _claimAccessor.GetClaims();
            var id = int.Parse(claim.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

            var user = await _context.Users
                .FirstOrDefaultAsync(x=>x.Id == id);

            if (user is null)
            {
                throw new ArgumentNullException($"User is not found");
            }

            return _mapper.Map<UserDTO>(user);
        }

        public async Task<int> GetCurrentUserIdAsync()
        {
            var currenUser = await GetCurrentUserAsync();

            return currenUser.Id;
        }

        public async Task DeleteCurrentUserAsync()
        {
            var currenUserId = await GetCurrentUserIdAsync();
            var currentUser = await _context.Users
                .FirstOrDefaultAsync(x => x.Id == currenUserId) ?? throw new ArgumentException("User not found");

            _context.Users.Remove(currentUser);
            await _context.SaveChangesAsync();
        }
    }
}
