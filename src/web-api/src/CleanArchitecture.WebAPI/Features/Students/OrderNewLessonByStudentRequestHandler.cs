using CleanArchitecture.Core.Common;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Infrastructure.Identity;
using CleanArchitecture.Infrastructure.Services.PushNotifications;

namespace CleanArchitecture.WebAPI.Features.Students
{
    public class OrderNewLessonByStudentRequest : IRequest<ApiResponse<LessonDto>>
    {
        public OrderLessonDto NewLessonDto { get; set; }
    }

    public class OrderNewLessonByStudentRequestHandler : BaseStudentRequestHandler,
                                                           IRequestHandler<OrderNewLessonByStudentRequest, ApiResponse<LessonDto>>
    {
        private readonly IPushNotificationService _pushNotificationService;

        public OrderNewLessonByStudentRequestHandler(IAsyncUserService userService,
                                                     IAsyncStudentService studentService,
                                                     IIdentityService identityService,
                                                     IPushNotificationService pushNotificationService)
                                                        : base(userService, studentService, identityService)
        {
            _pushNotificationService = pushNotificationService;
        }

        public async Task<ApiResponse<LessonDto>> Handle(OrderNewLessonByStudentRequest request, CancellationToken cancellationToken)
        {
            VerifyAccountType();

            var orderNewLessonByStudentDto = request.NewLessonDto;

            var userId = _identityService.GetUserIdentity();

            var orderLessonRequestResult = await _studentService.OrderLessonAsync(userId, orderNewLessonByStudentDto);

            if (orderLessonRequestResult.CompletedWithSuccess)
            {
                var newNotification = new PushNotification
                {
                    Tags = new List<string> { orderNewLessonByStudentDto.TutorId.ToString() },
                    MobilePlatform = MobilePlatform.apns,
                    Message = "New lesson request arrived!"
                };

                await _pushNotificationService.SendNotification(newNotification);

                newNotification.MobilePlatform = MobilePlatform.fcm;
                await _pushNotificationService.SendNotification(newNotification);

                newNotification.MobilePlatform = MobilePlatform.wns;
                await _pushNotificationService.SendNotification(newNotification);
            }

            return orderLessonRequestResult;
        }
    }
}
