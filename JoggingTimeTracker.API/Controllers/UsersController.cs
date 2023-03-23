using AutoMapper;
using JoggingTimeTracker.API.DTOs.User;
using JoggingTimeTracker.Core.Interfaces;
using JoggingTimeTracker.Core.Models;
using JoggingTimeTracker.Core.Models.Results;
using JoggingTimeTracker.Core.Utils;
using JoggingTimeTracker.Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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

        // GET: api/users
        /// <summary>
        /// Retrieves all registered users with basic user information. Only accessible to authorized users with 'Admin' or 'UserManager' roles.
        /// </summary>
        /// <returns>An array of UserDto objects with user information.</returns>
        /// <response code="200">Returns the list of users.</response>
        /// <response code="401">Unauthorized access. User does not have required role.</response>
        [Authorize(Roles = UserRoles.Admin)]
        [ProducesResponseType(typeof(UserDto), 201)]
        [ProducesResponseType(typeof(IEnumerable<IdentityError>), 400)]
        [HttpGet("users")]
        public async Task<ActionResult<ResultWithData<IEnumerable<UserDto>>>> GetAllUsersAsync()
        {
            var result = await _userService.GetAllUsersAsync();

            if (!result.Successed)
            {
                return BadRequest(result.ErrorMessage);
                
            }

            return Ok(result.Data);
        }


        // GET: api/users/{userName}
        /// <summary>
        /// Retrieves a user by their unique identifier. Only accessible to authorized users with 'Admin' or 'UserManager' roles.
        /// </summary>
        /// <param name="id">The unique identifier of the user.</param>
        /// <returns>A UserDto object with the user's information.</returns>
        /// <response code="200">Returns the user's information.</response>
        /// <response code="401">Unauthorized access. User does not have required role.</response>
        /// <response code="404">The user with the given id was not found.</response>
        [HttpGet("{id}")]
        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.UserManager)]
        [ProducesResponseType(typeof(UserDto), 201)]
        [ProducesResponseType(typeof(IEnumerable<IdentityError>), 400)]
        public async Task<ActionResult<UserDto>> GetUserById(string id)
        {
            var result = await _userService.GetUserByIdAsync(id);
            if (!result.Successed)
            {
                return BadRequest(result.ErrorMessage);
            }

            return Ok(_mapper.Map<UserDto>(result.Data));
        }

        // POST: api/users/regularuser
        /// <summary>
        /// Creates a new regular user. Only accessible to authorized users with the 'Admin' or 'UserManager' role.
        /// </summary>
        /// <param name="userCreateDto">The details of the user to create.</param>
        /// <returns>A response with status code 201 (Created) and the details of the created user.</returns>
        /// <response code="201">Returns the details of the created user.</response>
        /// <response code="400">Bad request. The user could not be created.</response>
        /// <response code="401">Unauthorized access. User does not have required role.</response>
        [HttpPost("regularuser")]
        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.UserManager)]
        [ProducesResponseType(typeof(UserDto), 201)]
        [ProducesResponseType(typeof(IEnumerable<IdentityError>), 400)]
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

            if (!result.Successed)
            {
                return BadRequest(result.ErrorMessage);
            }

            return Ok(_mapper.Map<UserDto>(result.Data));
        }

        // POST: api/users/usermanager
        /// <summary>
        /// Creates a new user with the 'UserManager' role. Only accessible to authorized users with the 'Admin' role.
        /// </summary>
        /// <param name="userCreateDto">The details of the user to be created.</param>
        /// <returns>A UserDto object with the new user's information.</returns>
        /// <response code="201">The user was successfully created.</response>
        /// <response code="400">The request was invalid or incomplete. The response body contains the validation errors.</response>
        /// <response code="401">Unauthorized access. User does not have required role.</response>
        [HttpPost("usermanager")]
        [Authorize(Roles = UserRoles.Admin)]
        [ProducesResponseType(typeof(UserDto), 201)]
        [ProducesResponseType(typeof(IEnumerable<string>), 400)]
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

            if (!result.Successed)
            {
                return BadRequest(result.ErrorMessage);
            }

            return Ok(_mapper.Map<UserDto>(result.Data));

        }

        // PUT: api/users/{id}
        /// <summary>
        /// Updates an existing user by their unique identifier. Only accessible to authorized users with the 'Admin' or 'UserManager' role.
        /// </summary>
        /// <param name="id">The unique identifier of the user to be updated.</param>
        /// <param name="userUpdateDto">The details of the user to be updated.</param>
        /// <returns>A response with status code 204 (No Content) if the update is successful.</returns>
        /// <response code="204">The user was successfully updated.</response>
        /// <response code="400">The request was invalid or incomplete. The response body contains the validation errors.</response>
        /// <response code="401">Unauthorized access. User does not have required role.</response>
        /// <response code="404">The user with the given id was not found.</response>
        [HttpPut("update")]
        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.UserManager)]
        [ProducesResponseType(typeof(UserDto), 201)]
        [ProducesResponseType(typeof(IEnumerable<IdentityError>), 400)]
        public async Task<IActionResult> UpdateUser(UserUpdateDto userUpdateDto)
        {
            var check = await _userService.GetUserByIdAsync(userUpdateDto.UserId);

            if (!check.Successed)
            {
                return BadRequest(check.ErrorMessage);
            }

            var user = _mapper.Map<ApplicationUser>(userUpdateDto);
            user.UserName = check.Data.UserName;
            var result = await _userService.UpdateUserAsync(user);

            if (!result.Successed)
            {
                return BadRequest(result.ErrorMessage);
            }

            return Ok(_mapper.Map<UserDto>(result.Data));
        }

        /// <summary>
        /// Deletes an existing user by their unique identifier. Only accessible to authorized users with the 'Admin' or 'UserManager' role.
        /// </summary>
        /// <param name="id">The unique identifier of the user to be deleted.</param>
        /// <returns>A response with status code 204 (No Content) if the deletion is successful.</returns>
        /// <response code="204">The user was successfully deleted.</response>
        /// <response code="400">The request was invalid or incomplete. The response body contains the validation errors.</response>
        /// <response code="401">Unauthorized access. User does not have required role.</response>
        /// <response code="404">The user with the given id was not found.</response>
        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.UserManager)]
        [ProducesResponseType(typeof(UserDto), 201)]
        [ProducesResponseType(typeof(IEnumerable<IdentityError>), 400)]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Result>> DeleteUser(string id)
        {
            var result = await _userService.DeleteUserAsync(id);
            if (result.Succeeded)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        /// <summary>
        /// Retrieves all users with a specific role. Only accessible to authorized users with the 'Admin' role.
        /// </summary>
        /// <param name="role">The role of the users to retrieve.</param>
        /// <returns>A response with status code 200 (OK) and a list of users with the specified role.</returns>
        /// <response code="200">Returns a list of users with the specified role.</response>
        /// <response code="401">Unauthorized access. User does not have required role.</response>
        [Authorize(Roles = UserRoles.Admin)]
        [HttpGet("by-role/{role}")]
        [ProducesResponseType(typeof(UserDto), 201)]
        [ProducesResponseType(typeof(IEnumerable<IdentityError>), 400)]
        public async Task<IActionResult> GetUsersByRoleAsync(string role)
        {
            var result = await _userService.GetUsersByRole(role);
            if (result.Successed)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result.ErrorMessage);
            }
        }
    }
}