using CleanArchitecture.Core.Common;
using CleanArchitecture.Core.DTOs;
using CleanArchitecture.Core.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.WebAPI.Features.Lesson
{
    public class GetLessonTopicCategoriesReques : IRequest<ApiResponse<IReadOnlyList<LessonTopicCategoryDto>>>
    {
    }

    public class GetLessonTopicCategoriesRequestHandler : IRequestHandler<GetLessonTopicCategoriesReques,
                                                                ApiResponse<IReadOnlyList<LessonTopicCategoryDto>>>
    {
        protected readonly IAsyncLessonTopicCategoriesService _lessonTopicCategoriesService;

        public GetLessonTopicCategoriesRequestHandler(IAsyncLessonTopicCategoriesService lessonTopicCategoriesService)
        {
            _lessonTopicCategoriesService = lessonTopicCategoriesService;
        }

        public async Task<ApiResponse<IReadOnlyList<LessonTopicCategoryDto>>> Handle(GetLessonTopicCategoriesReques request,
                                                                                                CancellationToken cancellationToken)
        {
            var lessonTopicCategories = await _lessonTopicCategoriesService.GetLessonTopicCategories();
            return lessonTopicCategories;
        }
    }
}
