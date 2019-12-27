using CleanArchitecture.Core.Common;
using CleanArchitecture.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Core.Interfaces
{
    public interface IAsyncLessonTopicCategoriesService
    {
        Task<ApiResponse<IReadOnlyList<LessonTopicCategoryDto>>> GetLessonTopicCategories();
    }
}
