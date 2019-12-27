using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CleanArchitecture.WebAPI.Features.Lesson;
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
    public class LessonController : BaseApiController
    {
        public LessonController(IMediator mediator) : base(mediator)
        {
        }

        /// <summary>
        /// Get lesson's topic categories
        /// </summary>
        [HttpGet("topic/categories")]
        public async Task<IActionResult> GetLessonTopicCategories()
        {
            var response = await _mediator.Send(new GetLessonTopicCategoriesReques());
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
    }
}