using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CleanArchitecture.Core.DTOs;
using CleanArchitecture.WebAPI.Features.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace CleanArchitecture.WebAPI.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : BaseApiController
    {
        public AccountController(IMediator mediator) : base(mediator)
        {
        }

        /// <summary>
        /// Get profile
        /// </summary>
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var response = await _mediator.Send(new GetUserProfileRequest());

            if (response.CompletedWithSuccess)
            {
                return Ok(response.Result);
            }

            else
            {
                Log.Error(response.ErrorMessage.Title);
                Log.Error(response.ErrorMessage.Message);

                return BadRequest(response.ErrorMessage);
            }
        }

        /// <summary>
        /// Update profile
        /// </summary>
        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateUserDto profile)
        {
            var response = await _mediator.Send(new UpdateUserProfileRequest
            {
                UpdateUserProfileDto = profile
            });

            if (response.CompletedWithSuccess)
            {
                return NoContent();
            }

            else
            {
                Log.Error(response.ErrorMessage.Title);
                Log.Error(response.ErrorMessage.Message);

                return BadRequest(response.ErrorMessage);
            }
        }

        /// <summary>
        /// Delete profile
        /// </summary>
        [HttpDelete("profile")]
        public async Task<IActionResult> DeleteProfile()
        {
            var response = await _mediator.Send(new DeleteUserProfileRequest());

            if (response.CompletedWithSuccess)
            {
                return NoContent();
            }

            else
            {
                Log.Error(response.ErrorMessage.Title);
                Log.Error(response.ErrorMessage.Message);

                return BadRequest(response.ErrorMessage);
            }
        }
    }
}