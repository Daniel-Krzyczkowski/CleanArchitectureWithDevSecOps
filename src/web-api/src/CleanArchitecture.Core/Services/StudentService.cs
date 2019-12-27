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
    public class StudentService : IAsyncStudentService
    {
        private readonly IAsyncLessonRepository _lessonRepository;

        public StudentService(IAsyncLessonRepository lessonRepository)
        {
            _lessonRepository = lessonRepository;
        }


        public async Task<ApiResponse<LessonDto>> OrderLessonAsync(Guid studentId, OrderLessonDto lesson)
        {
            if (studentId == null || studentId == Guid.Empty)
            {
                return new ApiResponse<LessonDto>()
                            .SetAsFailureResponse(Errors.Lesson.StudentIdForLessonIsEmpty());
            }

            var tutorPlannedLessons = await _lessonRepository.GetPlannedForTutor(lesson.TutorId);

            foreach (var tutorPlannedLesson in tutorPlannedLessons)
            {
                if (tutorPlannedLesson.Term != lesson.Term)
                {
                    double timeDifferenceBetweenLessons = tutorPlannedLesson.Term.Subtract(lesson.Term).TotalMinutes;
                    if (timeDifferenceBetweenLessons < 30)
                    {
                        return new ApiResponse<LessonDto>()
                                        .SetAsFailureResponse(Errors.Lesson.TooShortTimePeriodBetweenLessons());
                    }
                }

                else
                {
                    return new ApiResponse<LessonDto>()
                                        .SetAsFailureResponse(Errors.Lesson.TutorAlreadyBookedForLessonProposedTime());
                }
            }

            var newLesson = new Lesson
            {
                Length = lesson.Length,
                Location = lesson.Location,
                Term = lesson.Term,
                TopicCategory = new LessonTopicCategory
                {
                    Id = lesson.TopicCategory.Id,
                    CategoryName = lesson.TopicCategory.CategoryName
                },
                TutorId = lesson.TutorId
            };

            var orderedLesson = await _lessonRepository.AddAsync(newLesson);

            var orderedLessonDto = new LessonDto
            {
                Id = orderedLesson.Id,
                AcceptedByTutor = orderedLesson.AcceptedByTutor,
                CanceledByTutor = orderedLesson.CanceledByTutor,
                CancelledByStudent = orderedLesson.CancelledByStudent,
                Length = orderedLesson.Length,
                Location = orderedLesson.Location,
                StudentId = orderedLesson.StudentId,
                Term = orderedLesson.Term,
                TopicCategory = new LessonTopicCategoryDto
                {
                    Id = orderedLesson.TopicCategory.Id,
                    CategoryName = orderedLesson.TopicCategory.CategoryName
                },
                TutorId = orderedLesson.TutorId
            };

            return new ApiResponse<LessonDto>
            {
                Result = orderedLessonDto
            };
        }

        public async Task<ApiResponse> CancelLessonAsync(Guid studentId, Guid lessonId)
        {
            if (lessonId == null || lessonId == Guid.Empty)
            {
                return new ApiResponse().SetAsFailureResponse(Errors.Lesson.LessonIdIsEmpty());
            }

            if (studentId == null || studentId == Guid.Empty)
            {
                return new ApiResponse().SetAsFailureResponse(Errors.Lesson.StudentIdForLessonIsEmpty());
            }

            var lessonToCancel = await _lessonRepository.GetByIdAsync(lessonId);

            if (lessonToCancel == null)
            {
                return new ApiResponse().SetAsFailureResponse(Errors.Lesson.LessonNotFound());
            }

            else
            {
                if (lessonToCancel.StudentId != studentId)
                {
                    return new ApiResponse().SetAsFailureResponse(Errors.Lesson.LessonNotFound());
                }

                else
                {
                    lessonToCancel.CancelledByStudent = true;
                    await _lessonRepository.UpdateAsync(lessonToCancel);

                    return new ApiResponse();
                }
            }
        }

        public async Task<ApiResponse<IReadOnlyList<LessonDto>>> GetPlannedLessons(Guid studentId)
        {
            if (studentId == null || studentId == Guid.Empty)
            {
                return new ApiResponse<IReadOnlyList<LessonDto>>()
                                .SetAsFailureResponse(Errors.Lesson.StudentIdForLessonIsEmpty());
            }

            else
            {
                var plannedLessons = await _lessonRepository.GetPlannedForStudent(studentId);

                if (plannedLessons == null)
                {
                    return new ApiResponse<IReadOnlyList<LessonDto>>()
                                    .SetAsFailureResponse(Errors.Lesson.NoActiveLessonForStudent());
                }

                else
                {
                    var plannedLessonsDtos = plannedLessons.Select(lesson => new LessonDto
                    {
                        Id = lesson.Id,
                        AcceptedByTutor = lesson.AcceptedByTutor,
                        CanceledByTutor = lesson.CanceledByTutor,
                        CancelledByStudent = lesson.CancelledByStudent,
                        Length = lesson.Length,
                        Location = lesson.Location,
                        StudentId = lesson.StudentId,
                        Term = lesson.Term,
                        TopicCategory = new LessonTopicCategoryDto
                        {
                            Id = lesson.TopicCategory.Id,
                            CategoryName = lesson.TopicCategory.CategoryName
                        },
                        TutorId = lesson.TutorId
                    }).ToList();

                    return new ApiResponse<IReadOnlyList<LessonDto>>
                    {
                        Result = plannedLessonsDtos
                    };
                }
            }
        }

        public async Task<ApiResponse<IReadOnlyList<LessonDto>>> GetLessonsHistoryAsync(Guid studentId)
        {
            if (studentId == null || studentId == Guid.Empty)
            {
                return new ApiResponse<IReadOnlyList<LessonDto>>()
                            .SetAsFailureResponse(Errors.Lesson.StudentIdForLessonIsEmpty());
            }

            var lessonsHistoryForStudent = await _lessonRepository.ListAllByStudentIdAsync(studentId);

            if (lessonsHistoryForStudent == null)
            {
                return new ApiResponse<IReadOnlyList<LessonDto>>()
                             .SetAsFailureResponse(Errors.Lesson.NoLessonsHistoryFound());
            }

            else
            {
                var LessonsHistoryDtos = lessonsHistoryForStudent.Select(lesson => new LessonDto
                {
                    Id = lesson.Id,
                    AcceptedByTutor = lesson.AcceptedByTutor,
                    CanceledByTutor = lesson.CanceledByTutor,
                    CancelledByStudent = lesson.CancelledByStudent,
                    Length = lesson.Length,
                    Location = lesson.Location,
                    StudentId = lesson.StudentId,
                    Term = lesson.Term,
                    TopicCategory = new LessonTopicCategoryDto
                    {
                        Id = lesson.TopicCategory.Id,
                        CategoryName = lesson.TopicCategory.CategoryName
                    },
                    TutorId = lesson.TutorId
                }).ToList();

                return new ApiResponse<IReadOnlyList<LessonDto>>
                {
                    Result = LessonsHistoryDtos
                };
            }
        }
    }
}
