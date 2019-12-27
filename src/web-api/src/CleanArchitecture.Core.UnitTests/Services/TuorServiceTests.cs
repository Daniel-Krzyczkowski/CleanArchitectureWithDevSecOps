using AutoFixture;
using CleanArchitecture.Core.DTOs;
using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CleanArchitecture.Core.UnitTests.Services
{
    public class TuorServiceTests
    {
        private TutorService _tutorService;
        private Guid _lessonTopicCategory;
        private Guid _tutorId;
        private Fixture _fixture;

        public TuorServiceTests()
        {
            _fixture = new Fixture();
            _lessonTopicCategory = Guid.NewGuid();
            _tutorId = Guid.NewGuid();

            var lessonRepositoryMock = new Mock<IAsyncLessonRepository>();
            var identityRepositoryMock = new Mock<IAsyncIdentityRepository<User>>();
            var tutorLearningProfileRepository = new Mock<IAsyncTutorLearningProfileRepository>();

            var plannedLessonsListForTutor = _fixture.CreateMany<Lesson>().ToList().AsReadOnly();
            lessonRepositoryMock.Setup(x => x.GetPlannedForTutor(_tutorId))
                                                    .ReturnsAsync(plannedLessonsListForTutor);

            var lessonsHistoryListForTutor = _fixture.CreateMany<Lesson>().ToList().AsReadOnly();
            lessonRepositoryMock.Setup(x => x.ListAllByTutorIdAsync(_tutorId))
                                        .ReturnsAsync(lessonsHistoryListForTutor);

            lessonRepositoryMock.Setup(x => x.GetPlannedForTutor(_tutorId))
                                        .ReturnsAsync(plannedLessonsListForTutor);


            var tutorLearningProfilesFoundForLessonCategory = _fixture.CreateMany<TutorLearningProfile>()
                                                                       .ToList()
                                                                       .AsReadOnly();
            tutorLearningProfilesFoundForLessonCategory.First().TutorId = _tutorId;

            tutorLearningProfileRepository.Setup(x => x.ListAllByLessonTopicCategoryAsync(_lessonTopicCategory))
                                                 .ReturnsAsync(tutorLearningProfilesFoundForLessonCategory);

            var tutorsIdentifiers = tutorLearningProfilesFoundForLessonCategory
                                                                        .Select(t => t.TutorId)
                                                                        .ToList();

            var userProfiles = _fixture.CreateMany<User>()
                                                .ToList()
                                                .AsReadOnly();
            userProfiles.First().Id = _tutorId;

            identityRepositoryMock.Setup(x => x.GetProfiles(tutorsIdentifiers))
                                                         .ReturnsAsync(userProfiles);


            _tutorService = new TutorService(identityRepositoryMock.Object, lessonRepositoryMock.Object,
                                                                        tutorLearningProfileRepository.Object);
        }

        [Fact]
        public async Task ShoulsReturnListWithPlannedLessonsForTutor()
        {
            var plannedLessons = await _tutorService.GetPlannedLessons(_tutorId);

            Assert.True(plannedLessons.CompletedWithSuccess);
            Assert.NotNull(plannedLessons.Result);
        }

        [Fact]
        public async Task ShoulsReturnLessonHistoryListForTutor()
        {
            var plannedLessons = await _tutorService.GetLessonsHistoryAsync(_tutorId);

            Assert.True(plannedLessons.CompletedWithSuccess);
            Assert.NotNull(plannedLessons.Result);
        }

        [Fact]
        public async Task ShoulsReturnTutorsForLessonTopicCategory()
        {
            var tutorLearningProfilesFoundForLessonCategory = await _tutorService
                                            .GetTutorsForLessonTopicCategoryAsync(_lessonTopicCategory);

            Assert.True(tutorLearningProfilesFoundForLessonCategory.CompletedWithSuccess);
            Assert.NotNull(tutorLearningProfilesFoundForLessonCategory.Result);
        }
    }
}
