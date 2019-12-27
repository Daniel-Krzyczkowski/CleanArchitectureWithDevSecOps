using CleanArchitecture.Core.Common;
using CleanArchitecture.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Core.Interfaces
{
    public interface IAsyncTutorService
    {
        Task<ApiResponse> AcceptLessonAsync(Guid tutorId, Guid lessonId);
        Task<ApiResponse> CancelLessonAsync(Guid tutorId, Guid lessonId);
        Task<ApiResponse<IReadOnlyList<LessonDto>>> GetPlannedLessons(Guid tutorId);
        Task<ApiResponse<IReadOnlyList<LessonDto>>> GetLessonsHistoryAsync(Guid tutorId);
        Task<ApiResponse<TutorLearningProfileDto>> GetLearningProfileAsync(Guid tutorId);
        Task<ApiResponse<TutorLearningProfileDto>> AddLearningProfileAsync(Guid tutorId, AddTutorLearningProfileDto tutorLearningProfile);
        Task<ApiResponse> UpdateLearningProfileAsync(Guid tutorId, UpdateTutorLearningProfileDto tutorLearningProfileToUpdate);
        Task<ApiResponse> DeleteLearningProfileAsync(Guid tutorId);
        Task<ApiResponse<IReadOnlyList<TutorLearningProfileDto>>> GetTutorsForLessonTopicCategoryAsync(Guid lessonTopicCategoryId);
    }
}
