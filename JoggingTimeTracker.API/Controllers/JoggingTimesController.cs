using AutoMapper;
using JoggingTimeTracker.API.DTOs.JoggingTime;
using JoggingTimeTracker.Core.Interfaces;
using JoggingTimeTracker.Core.Models;
using JoggingTimeTracker.Core.Models.Report;
using JoggingTimeTracker.Core.Models.Results;
using JoggingTimeTracker.Core.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace JoggingTimeTracker.API.Controllers
{
    [Authorize(Roles = UserRoles.Admin + "," + UserRoles.RegularUser)]
    [Route("api/[controller]")]
    [ApiController]
    public class JoggingTimesController : ControllerBase
    {
        private readonly IJoggingTimeRepository _joggingTimeRepository; 
        private readonly IMapper _mapper;

        public JoggingTimesController(IJoggingTimeRepository joggingTimeRepository, IMapper mapper)
        {
            _joggingTimeRepository = joggingTimeRepository;
            _mapper = mapper;
        }

        
        [HttpGet("date-from-to")]
        public async Task<ActionResult<ResultWithData<IEnumerable<WeeklyJoggingStats>>>> GetJoggingTimesFilterDate(DateTime? fromDate, DateTime? toDate)
        {
            string userId = GetUserIds(Request);
            var result = await _joggingTimeRepository.GetJoggingTimesFilterDate(userId, fromDate, toDate);
            if (result.Successed)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpGet("weekly-stats")]
        public async Task<ActionResult<ResultWithData<IEnumerable<JoggingTime>>>> GetJoggingTimesFilterWeeek()
        {
            string userId = GetUserIds(Request);
            var result = await _joggingTimeRepository.GetJoggingTimesFilterWeeek(userId);
            if (result.Successed)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpGet("get-all")]
        public async Task<ActionResult<ResultWithData<IEnumerable<JoggingTime>>>> GetAllJoggingTimes()
        {
            string userId = GetUserIds(Request);
            var result = await _joggingTimeRepository.GetAllJoggingTimes(userId);
            if (result.Successed)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpGet("get-by-id")]
        public async Task<IActionResult> GetJoggingTimeAsync(int id)
        {
            var result = await _joggingTimeRepository.GetJoggingTime(id);

            if (!result.Successed)
            {
                return BadRequest(result.ErrorMessage);
            }

            return Ok(_mapper.Map<JoggingTimeDto>(result.Data));
        }

        [HttpPost("Add")]
        public async Task<IActionResult> CreateJoggingTimeAsync([FromBody] JoggingTimeAddDto joggingTimeDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            JoggingTime joggingTime = _mapper.Map<JoggingTime>(joggingTimeDto);

            joggingTime.UserId = GetUserIds(Request);

            var result = await _joggingTimeRepository.AddJoggingTime(joggingTime);

            if (!result.Successed)
            {
                return BadRequest(result.ErrorMessage);
            }
            return Ok(_mapper.Map<JoggingTimeDto>(result.Data));
        }

        [HttpPut("Edit")]
        public async Task<IActionResult> UpdateJoggingTimeAsync([FromBody] JoggingTimeEditDto joggingTimeEditDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            JoggingTime joggingTime = _mapper.Map<JoggingTime>(joggingTimeEditDto);

            var result = await _joggingTimeRepository.UpdateJoggingTime(joggingTime);

            if (!result.Successed)
            {
                return BadRequest(result.ErrorMessage);
            }
            return Ok(_mapper.Map<JoggingTimeDto>(result.Data));
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteJoggingTimeAsync(int id)
        {
            var result = await _joggingTimeRepository.DeleteJoggingTimeAsync(id);

            if (!result.Succeeded)
            {
                return BadRequest(result.Message);
            }

            return Ok(result.Message);
        }

        private static string GetUserIds(HttpRequest request)
        {
            string authHeader = request.Headers["Authorization"];

            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
            {
                // The authorization header is missing or invalid
                return null;
            }

            string token = authHeader.Substring("Bearer ".Length).Trim();

            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
                var userId = jsonToken?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

                return userId;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}