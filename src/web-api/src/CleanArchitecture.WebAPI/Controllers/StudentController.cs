using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CleanArchitecture.Core.DTOs;
using CleanArchitecture.WebAPI.Features.Students;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace CleanArchitecture.WebAPI.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : BaseApiController
    {
        public StudentController(IMediator mediator) : base(mediator)
        {
        }

        /// <summary>
        /// Order new lesson
        /// </summary>
        [HttpPost("lesson/order")]
        public async Task<IActionResult> OrderLesson(OrderLessonDto lesson)
        {
            var response = await _mediator.Send(new OrderNewLessonByStudentRequest
            {
                NewLessonDto = lesson
            });

            if (response.CompletedWithSuccess)
            {
                return Ok();
            }

            else
            {
                Log.Error(response.ErrorMessage.Title);
                Log.Error(response.ErrorMessage.Message);

                return BadRequest(response.ErrorMessage);
            }
        }

        /// <summary>
        /// Cancel lesson
        /// </summary>
        [HttpPut("lesson/{id}/cancel")]
        public async Task<IActionResult> CancelLesson(Guid id)
        {
            var response = await _mediator.Send(new CancelLessonByStudentRequest
            {
                LessonId = id
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
        /// Get lessons history
        /// </summary>
        [HttpGet("lesson/history")]
        public async Task<IActionResult> GetLessonsHistory()
        {
            var response = await _mediator.Send(new GetLessonsHistoryForStudentRequest());
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
        /// Get planned lessons
        /// </summary>
        [HttpGet("lesson/planned")]
        public async Task<IActionResult> GetPlannedLessons()
        {
            var response = await _mediator.Send(new GetPlannedLessonsForStudentRequest());

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