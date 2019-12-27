using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CleanArchitecture.Infrastructure.Services.PushNotifications;
using CleanArchitecture.WebAPI.Features.Notifications;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : BaseApiController
    {
        public NotificationController(IMediator mediator) : base(mediator)
        {
        }

        /// <summary>
        /// Get registration ID
        /// </summary>
        [HttpGet("register/{handle}")]
        public async Task<IActionResult> CreatePushRegistrationId(string handle)
        {
            var response = await _mediator.Send(new CreateRegistrationIdRequest
            {
                Handle = handle
            });

            if (response.CompletedWithSuccess)
            {
                return Ok(response.Result);
            }

            else
            {
                return BadRequest(response.ErrorMessage);
            }
        }

        /// <summary>
        /// Delete registration ID and unregister from receiving push notifications
        /// </summary>
        [HttpDelete("unregister/{registrationId}")]
        public async Task<IActionResult> UnregisterFromNotifications(string registrationId)
        {
            var response = await _mediator.Send(new UnregisterFromPushNotificationsRequest
            {
                RegistrationId = registrationId
            });

            if (response.CompletedWithSuccess)
            {
                return NoContent();
            }

            else
            {
                return BadRequest(response.ErrorMessage);
            }
        }

        /// <summary>
        /// Register to receive push notifications
        /// (mobile platforms: wns = 0, apns = 1, fcm = 2)
        /// </summary>
        [HttpPut("enable/{id}")]
        public async Task<IActionResult> RegisterForPushNotifications(string id, [FromBody] DeviceRegistration deviceUpdate)
        {
            var response = await _mediator.Send(new RegisterForPushNotificationsRequest
            {
                RegistrationId = id,
                DeviceUpdate = deviceUpdate
            });

            if (response.CompletedWithSuccess)
            {
                return NoContent();
            }

            else
            {
                return BadRequest(response.ErrorMessage);
            }
        }

        /// <summary>
        /// Send push notification
        /// (mobile platforms: wns = 0, apns = 1, fcm = 2)
        /// </summary>
        [HttpPost("send")]
        public async Task<IActionResult> SendNotification([FromBody] PushNotification newNotification)
        {
            var response = await _mediator.Send(new SendPushNotificationRequest
            {
                NewNotification = newNotification
            });

            if (response.CompletedWithSuccess)
            {
                return Ok();
            }

            return BadRequest("An error occurred while sending push notification. Please try again");
        }
    }
}