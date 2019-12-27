using CleanArchitecture.Core.Common;
using CleanArchitecture.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Core.Interfaces
{
    public interface IAsyncStudentService
    {
        Task<ApiResponse<LessonDto>> OrderLessonAsync(Guid studentId, OrderLessonDto lesson);
        Task<ApiResponse> CancelLessonAsync(Guid studentId, Guid lessonId);
        Task<ApiResponse<IReadOnlyList<LessonDto>>> GetPlannedLessons(Guid studentId);
        Task<ApiResponse<IReadOnlyList<LessonDto>>> GetLessonsHistoryAsync(Guid studentId);
    }
}
