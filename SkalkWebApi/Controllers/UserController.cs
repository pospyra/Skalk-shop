using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Skalk.BLL.Interfaces;
using Skalk.Common.DTO.User;

namespace SkalkWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<ActionResult<UserDTO>> Registration(NewUserDTO userDto)
        {
            var user = await _userService.Registration(userDto);

            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<ActionResult<NewUserDTO>> Login(LoginUserDTO userDto)
        {
            var user = await _userService.Login(userDto);

            return Ok(user);
        }


        [Authorize]
        [HttpGet("current")]
        public async Task<ActionResult<UserDTO>> GetCurrentUser()
        {
            var user = await _userService.GetCurrentUserAsync();

            return Ok(user);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetUserById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);

            return Ok(user);
        }
    }
}
