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
    public class TutorService : IAsyncTutorService
    {
        private readonly IAsyncIdentityRepository<User> _identityRepository;
        private readonly IAsyncLessonRepository _lessonRepository;
        private readonly IAsyncTutorLearningProfileRepository _tutorLearningProfileRepository;

        public TutorService(IAsyncIdentityRepository<User> identityRepository,
                            IAsyncLessonRepository lessonRepository,
                            IAsyncTutorLearningProfileRepository tutorLearningProfileRepository)
        {
            _identityRepository = identityRepository;
            _lessonRepository = lessonRepository;
            _tutorLearningProfileRepository = tutorLearningProfileRepository;
        }

        public async Task<ApiResponse<IReadOnlyList<LessonDto>>> GetPlannedLessons(Guid tutorId)
        {
            if (tutorId == null || tutorId == Guid.Empty)
            {
                return new ApiResponse<IReadOnlyList<LessonDto>>()
                                    .SetAsFailureResponse(Errors.Lesson.TutorIdForLessonIsEmpty());
            }

            else
            {
                var plannedLessons = await _lessonRepository.GetPlannedForTutor(tutorId);

                if (plannedLessons == null)
                {
                    return new ApiResponse<IReadOnlyList<LessonDto>>()
                                    .SetAsFailureResponse(Errors.Lesson.NoActiveLessonForTutor());
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

        public async Task<ApiResponse<IReadOnlyList<LessonDto>>> GetLessonsHistoryAsync(Guid tutorId)
        {
            if (tutorId == null || tutorId == Guid.Empty)
            {
                return new ApiResponse<IReadOnlyList<LessonDto>>()
                            .SetAsFailureResponse(Errors.Lesson.TutorIdForLessonIsEmpty());
            }

            var lessonsHistoryListForTutor = await _lessonRepository.ListAllByTutorIdAsync(tutorId);

            if (lessonsHistoryListForTutor == null)
            {
                return new ApiResponse<IReadOnlyList<LessonDto>>()
                             .SetAsFailureResponse(Errors.Lesson.NoLessonsHistoryFound());
            }

            else
            {
                var LessonsHistoryDtos = lessonsHistoryListForTutor.Select(lesson => new LessonDto
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

        public async Task<ApiResponse> AcceptLessonAsync(Guid tutorId, Guid lessonId)
        {
            if (lessonId == null || lessonId == Guid.Empty)
            {
                return new ApiResponse().SetAsFailureResponse(Errors.Lesson.LessonIdIsEmpty());
            }

            else if (tutorId == null || tutorId == Guid.Empty)
            {
                return new ApiResponse().SetAsFailureResponse(Errors.Lesson.TutorIdForLessonIsEmpty());
            }

            else
            {
                var lessonToAccept = await _lessonRepository.GetByIdAsync(lessonId);

                if (lessonToAccept == null)
                {
                    return new ApiResponse().SetAsFailureResponse(Errors.Lesson.LessonNotFound());
                }

                else
                {
                    if (lessonToAccept.TutorId != tutorId)
                    {
                        return new ApiResponse().SetAsFailureResponse(Errors.Lesson.LessonNotFound());
                    }
                    else
                    {
                        lessonToAccept.TutorId = tutorId;
                        lessonToAccept.AcceptedByTutor = true;

                        await _lessonRepository.UpdateAsync(lessonToAccept);

                        return new ApiResponse();
                    }
                }
            }
        }

        public async Task<ApiResponse> CancelLessonAsync(Guid tutorId, Guid lessonId)
        {
            if (lessonId == null || lessonId == Guid.Empty)
            {
                return new ApiResponse().SetAsFailureResponse(Errors.Lesson.LessonIdIsEmpty());
            }

            else if (tutorId == null || tutorId == Guid.Empty)
            {
                return new ApiResponse().SetAsFailureResponse(Errors.Lesson.TutorIdForLessonIsEmpty());
            }

            else
            {
                var lessonToCancel = await _lessonRepository.GetByIdAsync(lessonId);

                if (lessonToCancel == null)
                {
                    return new ApiResponse().SetAsFailureResponse(Errors.Lesson.LessonNotFound());
                }

                else
                {
                    if (lessonToCancel.TutorId != tutorId)
                    {
                        return new ApiResponse().SetAsFailureResponse(Errors.Lesson.LessonNotFound());
                    }

                    else
                    {
                        lessonToCancel.CanceledByTutor = true;
                        await _lessonRepository.UpdateAsync(lessonToCancel);

                        return new ApiResponse();
                    }
                }
            }
        }

        public async Task<ApiResponse<IReadOnlyList<TutorLearningProfileDto>>> GetTutorsForLessonTopicCategoryAsync(Guid lessonTopicCategoryId)
        {
            var tutorsFoundForLessonCategory = await _tutorLearningProfileRepository.ListAllByLessonTopicCategoryAsync(lessonTopicCategoryId);
            if (tutorsFoundForLessonCategory == null)
            {
                return new ApiResponse<IReadOnlyList<TutorLearningProfileDto>>()
                                        .SetAsFailureResponse(Errors.Tutor.TutorsNotFoundForLessonCategory());
            }

            else
            {
                var tutorsIdentifiers = tutorsFoundForLessonCategory.Select(t => t.TutorId).ToList();
                var tutorLearningProfilesDtos = tutorsFoundForLessonCategory.Select(p => new TutorLearningProfileDto
                {
                    Id = p.Id,
                    PricePerHour = p.PricePerHour,
                    TutorId = p.TutorId
                }).ToList();

                var tutorsProfiles = await _identityRepository.GetProfiles(tutorsIdentifiers);

                foreach (var tutorLearingProfile in tutorLearningProfilesDtos)
                {
                    foreach (var tutorProfile in tutorsProfiles)
                    {
                        if (tutorLearingProfile.TutorId == tutorProfile.Id)
                        {
                            tutorLearingProfile.FirstName = tutorProfile.FirstName;
                            tutorLearingProfile.LastName = tutorProfile.LastName;
                            tutorLearingProfile.Phone = tutorProfile.Phone;
                            tutorLearingProfile.Mail = tutorProfile.Email;
                        }
                    }
                }

                return new ApiResponse<IReadOnlyList<TutorLearningProfileDto>>
                {
                    Result = tutorLearningProfilesDtos
                };
            }
        }

        public async Task<ApiResponse<TutorLearningProfileDto>> AddLearningProfileAsync(Guid tutorId, AddTutorLearningProfileDto tutorLearningProfile)
        {
            if (tutorId == null || tutorId == Guid.Empty)
            {
                return new ApiResponse<TutorLearningProfileDto>()
                                .SetAsFailureResponse(Errors.Lesson.TutorIdForLessonIsEmpty());
            }

            var existingProfile = await _tutorLearningProfileRepository.GetByTutorIdAsync(tutorId);

            if (existingProfile != null)
            {
                return new ApiResponse<TutorLearningProfileDto>()
                                .SetAsFailureResponse(Errors.Tutor.TutorLearningProfileAlreadyExists());
            }

            if (tutorLearningProfile.LessonTopicCategories == null)
            {
                return new ApiResponse<TutorLearningProfileDto>()
                                .SetAsFailureResponse(Errors.Tutor.TutorLearningProfileLessonCategoryNotFound());
            }


            var learningProfile = new TutorLearningProfile
            {
                Id = Guid.NewGuid(),
                LessonTopicCategories = tutorLearningProfile.LessonTopicCategories.Select(c => new LessonTopicCategory
                {
                    Id = c.Id,
                    CategoryName = c.CategoryName
                }).ToList(),
                PricePerHour = tutorLearningProfile.PricePerHour,
                TutorId = tutorId
            };

            var addedLearningProfile = await _tutorLearningProfileRepository.AddAsync(learningProfile);

            var tutorProfile = await _identityRepository.GetProfile(tutorId);

            var addedTutorLearninfProfileDto = new TutorLearningProfileDto
            {
                Id = addedLearningProfile.Id,
                FirstName = tutorProfile.FirstName,
                LastName = tutorProfile.LastName,
                Mail = tutorProfile.Email,
                Phone = tutorProfile.Phone,
                LessonTopicCategories = addedLearningProfile.LessonTopicCategories.Select(c => new LessonTopicCategoryDto
                {
                    Id = c.Id,
                    CategoryName = c.CategoryName
                }).ToList(),
                PricePerHour = tutorLearningProfile.PricePerHour,
                TutorId = tutorId
            };

            return new ApiResponse<TutorLearningProfileDto>
            {
                Result = addedTutorLearninfProfileDto
            };
        }

        public async Task<ApiResponse<TutorLearningProfileDto>> GetLearningProfileAsync(Guid tutorId)
        {
            if (tutorId == null || tutorId == Guid.Empty)
            {
                return new ApiResponse<TutorLearningProfileDto>()
                                .SetAsFailureResponse(Errors.Lesson.TutorIdForLessonIsEmpty());
            }

            var learningProfile = await _tutorLearningProfileRepository.GetByTutorIdAsync(tutorId);

            if (learningProfile == null)
            {
                return new ApiResponse<TutorLearningProfileDto>()
                                .SetAsFailureResponse(Errors.Tutor.TutorLearningProfileNotFound());
            }

            else
            {
                var tutorProfile = await _identityRepository.GetProfile(tutorId);
                var tutorLearningProfile = new TutorLearningProfileDto
                {
                    Id = learningProfile.Id,
                    TutorId = learningProfile.TutorId,
                    FirstName = tutorProfile.FirstName,
                    LastName = tutorProfile.LastName,
                    Mail = tutorProfile.Email,
                    Phone = tutorProfile.Phone,
                    PricePerHour = learningProfile.PricePerHour,
                    LessonTopicCategories = learningProfile.LessonTopicCategories.Select(c => new LessonTopicCategoryDto
                    {
                        Id = c.Id,
                        CategoryName = c.CategoryName
                    }).ToList()
                };

                return new ApiResponse<TutorLearningProfileDto>
                {
                    Result = tutorLearningProfile
                };
            }
        }

        public async Task<ApiResponse> UpdateLearningProfileAsync(Guid tutorId, UpdateTutorLearningProfileDto tutorLearningProfileToUpdate)
        {
            if (tutorId == null || tutorId == Guid.Empty)
            {
                return new ApiResponse().SetAsFailureResponse(Errors.Lesson.TutorIdForLessonIsEmpty());
            }

            var tutorLearningProfile = await _tutorLearningProfileRepository.GetByTutorIdAsync(tutorId);
            if (tutorLearningProfile == null)
            {
                return new ApiResponse().SetAsFailureResponse(Errors.Tutor.TutorLearningProfileNotFound());
            }

            else if (tutorLearningProfile.TutorId != tutorId)
            {
                return new ApiResponse().SetAsFailureResponse(Errors.Tutor.TutorLearningProfileNotFound());
            }

            else
            {
                tutorLearningProfile.PricePerHour = tutorLearningProfileToUpdate.PricePerHour;
                if(tutorLearningProfileToUpdate.LessonTopicCategories == null)
                {
                    return new ApiResponse().SetAsFailureResponse(Errors.Tutor.TutorLearningProfileLessonCategoryNotFound());
                }
                tutorLearningProfile.LessonTopicCategories = tutorLearningProfileToUpdate.LessonTopicCategories
                                                                                        .Select(c => new LessonTopicCategory
                                                                                        {
                                                                                            Id = c.Id,
                                                                                            CategoryName = c.CategoryName
                                                                                        }).ToList();


                await _tutorLearningProfileRepository.UpdateAsync(tutorLearningProfile);

                return new ApiResponse();
            }
        }

        public async Task<ApiResponse> DeleteLearningProfileAsync(Guid tutorId)
        {
            if (tutorId == null || tutorId == Guid.Empty)
            {
                return new ApiResponse().SetAsFailureResponse(Errors.Lesson.TutorIdForLessonIsEmpty());
            }

            var tutorLearningProfile = await _tutorLearningProfileRepository.GetByTutorIdAsync(tutorId);
            if (tutorLearningProfile == null)
            {
                return new ApiResponse().SetAsFailureResponse(Errors.Tutor.TutorLearningProfileNotFound());
            }

            else if (tutorLearningProfile.TutorId != tutorId)
            {
                return new ApiResponse().SetAsFailureResponse(Errors.Tutor.TutorLearningProfileNotFound());
            }

            else
            {
                await _tutorLearningProfileRepository.DeleteAsync(tutorLearningProfile);

                return new ApiResponse();
            }
        }
    }
}
