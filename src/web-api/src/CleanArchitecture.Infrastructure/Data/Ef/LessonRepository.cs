using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Infrastructure.Data.Ef
{
    public class LessonRepository : EfRepository<Lesson>, IAsyncLessonRepository
    {
        public LessonRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IReadOnlyList<Lesson>> GetPlannedForTutor(Guid tutorId)
        {
            var tutorLessons = await _dbContext.Lessons
                                         .Where(l => l.TutorId == tutorId)
                                         .Where(l => l.Term > DateTimeOffset.UtcNow)
                                         .Where(l => !l.CanceledByTutor)
                                         .Where(l => !l.CancelledByStudent)
                                         .ToListAsync();
            return tutorLessons;
        }

        public async Task<IReadOnlyList<Lesson>> GetPlannedForStudent(Guid studentId)
        {
            var studentLessons = await _dbContext.Lessons
                                                .Where(l => l.StudentId == studentId)
                                                .Where(l => l.Term > DateTimeOffset.UtcNow)
                                                .Where(l => !l.CanceledByTutor)
                                                .Where(l => !l.CancelledByStudent)
                                                .ToListAsync();
            return studentLessons;
        }

        public async Task<IReadOnlyList<Lesson>> ListAllByTutorIdAsync(Guid tutorId)
        {
            var tutorLessons = await _dbContext.Lessons
                                                    .Where(l => l.TutorId == tutorId)
                                                    .Where(l => l.Term < DateTimeOffset.UtcNow)
                                                    .ToListAsync();

            return tutorLessons;
        }

        public async Task<IReadOnlyList<Lesson>> ListAllByStudentIdAsync(Guid studentId)
        {
            var studentLessons = await _dbContext.Lessons
                                                    .Where(l => l.StudentId == studentId)
                                                    .Where(l => l.Term < DateTimeOffset.UtcNow)
                                                    .ToListAsync();

            return studentLessons;
        }
    }
}
