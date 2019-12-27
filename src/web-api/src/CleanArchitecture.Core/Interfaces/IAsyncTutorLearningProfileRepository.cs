using CleanArchitecture.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Core.Interfaces
{
    public interface IAsyncTutorLearningProfileRepository : IAsyncRepository<TutorLearningProfile>
    {
        Task<IReadOnlyList<TutorLearningProfile>> ListAllByLessonTopicCategoryAsync(Guid lessonTopicCategoryId);
        Task<TutorLearningProfile> GetByTutorIdAsync(Guid tutorId);
    }
}
