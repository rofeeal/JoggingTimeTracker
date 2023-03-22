using AutoMapper;
using JoggingTimeTracker.API.DTOs.User;
using JoggingTimeTracker.Core.Interfaces;
using JoggingTimeTracker.Core.Models;
using JoggingTimeTracker.Core.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JoggingTimeTracker.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UsersController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        // GET: api/user
        [HttpGet]
        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.UserManager)]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();

            var userDtos = users.Select(u => new UserDto
            {
                Id = u.Id,
                UserName = u.UserName,
                Email = u.Email,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Role = u.Role
            });

            return Ok(userDtos);
        }

        // GET: api/user/{userName}
        [HttpGet("{id}")]
        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.UserManager)]
        public async Task<ActionResult<UserDto>> GetUserById(string id)
        {
            var user = await _userService.GetUserByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            var userDto = new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = user.Role
            };

            return Ok(userDto);
        }

        // POST: api/user/regularuser
        [HttpPost("regularuser")]
        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.UserManager)]
        public async Task<ActionResult<UserDto>> CreateRegularUser(UserCreateDto userCreateDto)
        {
            var user = new ApplicationUser
            {
                UserName = userCreateDto.UserName,
                Email = userCreateDto.Email,
                FirstName = userCreateDto.FirstName,
                LastName = userCreateDto.LastName,
                Role = UserRoles.RegularUser
            };

            var result = await _userService.CreateUserAsync(user, userCreateDto.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            var userDto = new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = user.Role
            };

            return CreatedAtAction(nameof(GetUserById), new { userName = user.Id }, userDto);
        }

        // POST: api/user/usermanager
        [HttpPost("usermanager")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<ActionResult<UserDto>> CreateUserManager(UserCreateDto userCreateDto)
        {
            var user = new ApplicationUser
            {
                UserName = userCreateDto.UserName,
                Email = userCreateDto.Email,
                FirstName = userCreateDto.FirstName,
                LastName = userCreateDto.LastName,
                Role = UserRoles.UserManager
            };

            var result = await _userService.CreateUserAsync(user, userCreateDto.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            var userDto = new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = user.Role
            };

            return CreatedAtAction(nameof(GetUserById), new { userName = user.Id }, userDto);
        }

        // PUT: api/user/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.UserManager)]
        public async Task<IActionResult> UpdateUser(string id, UserUpdateDto userUpdateDto)
        {
            ApplicationUser user = await _userService.GetUserByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            user = _mapper.Map<ApplicationUser>(userUpdateDto);

            var result = await _userService.UpdateUserAsync(user);

            if (result.Succeeded)
            {
                return NoContent();
            }

            return BadRequest(result.Errors);
        }

        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.UserManager)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var result = await _userService.DeleteUserAsync(id);

            if (result.Succeeded)
            {
                return NoContent();
            }

            return BadRequest(result.Errors);
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpGet("by-role/{role}")]
        public IActionResult GetUsersByRole(string role)
        {
            var users = _userService.GetUsersByRole(role);
            return Ok(users);
        }
    }
}