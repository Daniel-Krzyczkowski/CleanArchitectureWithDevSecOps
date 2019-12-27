using CleanArchitecture.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Core.Interfaces
{
    public interface IAsyncLessonRepository : IAsyncRepository<Lesson>
    {
        Task<IReadOnlyList<Lesson>> ListAllByTutorIdAsync(Guid tutorId);
        Task<IReadOnlyList<Lesson>> ListAllByStudentIdAsync(Guid studentId);
        Task<IReadOnlyList<Lesson>> GetPlannedForTutor(Guid tutorId);
        Task<IReadOnlyList<Lesson>> GetPlannedForStudent(Guid studentId);
    }
}
