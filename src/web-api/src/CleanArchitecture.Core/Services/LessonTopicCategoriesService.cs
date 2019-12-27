using CleanArchitecture.Core.Common;
using CleanArchitecture.Core.DTOs;
using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.Exceptions;
using CleanArchitecture.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Core.Services
{
    public class LessonTopicCategoriesService : IAsyncLessonTopicCategoriesService
    {
        private readonly IAsyncRepository<LessonTopicCategory> _lessonTopicCategoriesRepository;

        public LessonTopicCategoriesService(IAsyncRepository<LessonTopicCategory> lessonTopicCategoriesRepository)
        {
            _lessonTopicCategoriesRepository = lessonTopicCategoriesRepository;
        }

        public async Task<ApiResponse<IReadOnlyList<LessonTopicCategoryDto>>> GetLessonTopicCategories()
        {
            var lessonTopicCategories = await _lessonTopicCategoriesRepository.ListAllAsync();

            if (lessonTopicCategories == null || lessonTopicCategories.Count == 0)
            {
                return new ApiResponse<IReadOnlyList<LessonTopicCategoryDto>>()
                    .SetAsFailureResponse(Errors.Lesson.LessonTopicCategoriesNotFound());
            }

            else
            {
                var topics = lessonTopicCategories.Select(c => new LessonTopicCategoryDto
                {
                    Id = c.Id,
                    CategoryName = c.CategoryName
                }).ToList();

                return new ApiResponse<IReadOnlyList<LessonTopicCategoryDto>>
                {
                    Result = topics
                };
            }
        }
    }
}
