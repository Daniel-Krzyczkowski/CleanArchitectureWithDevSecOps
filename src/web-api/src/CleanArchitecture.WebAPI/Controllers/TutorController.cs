using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CleanArchitecture.WebAPI.Features.Tutors;
using CleanArchitecture.Core.DTOs;
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
    public class TutorController : BaseApiController
    {
        public TutorController(IMediator mediator) : base(mediator)
        {
        }

        /// <summary>
        /// Accept lesson
        /// </summary>
        [HttpPost("lesson/{id}/accept")]
        public async Task<IActionResult> AcceptLesson(Guid id)
        {

            var response = await _mediator.Send(new AcceptNewLessonByTutorRequest
            {
                LessonId = id
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
            var response = await _mediator.Send(new CancelLessonByTutorRequest
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
            var response = await _mediator.Send(new GetLessonsHistoryForTutorRequest());
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
            var response = await _mediator.Send(new GetPlannedLessonsForTutorRequest());

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
        /// Get tutors for specific lesson's topic category
        /// </summary>
        [HttpGet("lesson/category")]
        public async Task<IActionResult> GetTutorsForLessonTopicCategory([FromQuery] Guid lessonTopicCategoryId)
        {
            var response = await _mediator.Send(new GetTutorsForLessonTopicCategoryRequest
            {
                lessonTopicCategoryId = lessonTopicCategoryId
            });

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
        /// Create learning profile
        /// </summary>
        [HttpPost("learning/profile")]
        public async Task<IActionResult> CreateLearningProfile([FromBody] AddTutorLearningProfileDto tutorlearningProfile)
        {
            var response = await _mediator.Send(new CreateTutorLearningProfileRequest
            {
                CreateTutorLearningProfileDto = tutorlearningProfile
            });

            if (response.CompletedWithSuccess)
            {
                return CreatedAtAction(nameof(GetLearningProfile), response.Result);
            }

            else
            {
                Log.Error(response.ErrorMessage.Title);
                Log.Error(response.ErrorMessage.Message);

                return BadRequest(response.ErrorMessage);
            }
        }

        /// <summary>
        /// Get learning profiles
        /// </summary>
        [HttpGet("learning/profile")]
        public async Task<IActionResult> GetLearningProfile()
        {
            var response = await _mediator.Send(new GetTutorLearningProfileRequest());

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
        /// Update learning profile
        /// </summary>
        [HttpPut("learning/profile")]
        public async Task<IActionResult> UpdateLearningProfile([FromBody] UpdateTutorLearningProfileDto learningProfile)
        {
            var response = await _mediator.Send(new UpdateTutorLearningProfileRequest
            {
                UpdateTutorLearningProfileDto = learningProfile
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
        /// Delete learning profile
        /// </summary>
        [HttpDelete("learning/profile")]
        public async Task<IActionResult> DeleteLearningProfile()
        {
            var response = await _mediator.Send(new DeleteTutorLearningProfileRequest());

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