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
    public class StudentServiceTests
    {
        private StudentService _studentService;
        private Guid _studentId;
        private Guid _tutorId;
        private Fixture _fixture;

        public StudentServiceTests()
        {
            _fixture = new Fixture();
            _studentId = Guid.NewGuid();
            _tutorId = Guid.NewGuid();

            var lessonRepositoryMock = new Mock<IAsyncLessonRepository>();

            var plannedLessonsListForStudent = _fixture.CreateMany<Lesson>().ToList().AsReadOnly();
            lessonRepositoryMock.Setup(x => x.GetPlannedForStudent(_studentId))
                                                    .ReturnsAsync(plannedLessonsListForStudent);

            var lessonsHistoryListForStudent = _fixture.CreateMany<Lesson>().ToList().AsReadOnly();
            lessonRepositoryMock.Setup(x => x.ListAllByStudentIdAsync(_studentId))
                                        .ReturnsAsync(lessonsHistoryListForStudent);

            var plannedLesson = _fixture.Build<Lesson>()
                .With(x => x.Term, new DateTimeOffset(2019, 12, 30, 16, 00, 00, 00, TimeSpan.Zero))
                .Create();
            var plannedLessonsListForTutor = _fixture.CreateMany<Lesson>().ToList();
            plannedLessonsListForTutor.Add(plannedLesson);

            lessonRepositoryMock.Setup(x => x.GetPlannedForTutor(_tutorId))
                                        .ReturnsAsync(plannedLessonsListForTutor.AsReadOnly());

            _studentService = new StudentService(lessonRepositoryMock.Object);
        }

        [Fact]
        public async Task ShoulsReturnListWithPlannedLessonsForStudent()
        {
            var plannedLessons = await _studentService.GetPlannedLessons(_studentId);

            Assert.True(plannedLessons.CompletedWithSuccess);
            Assert.NotNull(plannedLessons.Result);
        }

        [Fact]
        public async Task ShoulsReturnLessonHistoryListForStudent()
        {
            var plannedLessons = await _studentService.GetLessonsHistoryAsync(_studentId);

            Assert.True(plannedLessons.CompletedWithSuccess);
            Assert.NotNull(plannedLessons.Result);
        }

        [Fact]
        public async Task ShoulsNotAcceptNewLessonOrderFromStudentForBusyTutor()
        {
            var newOrderedLesson = new OrderLessonDto
            {
                Length = 2,
                Location = "Warsaw, Poland",
                Term = new DateTimeOffset(2019, 12, 30, 16, 20, 00, 00, TimeSpan.Zero),
                TopicCategory = new LessonTopicCategoryDto
                {
                    Id = Guid.NewGuid(),
                    CategoryName = "Physics"
                },
                TutorId = _tutorId
            };

            var orderedLesson = await _studentService.OrderLessonAsync(_studentId, newOrderedLesson);

            Assert.False(orderedLesson.CompletedWithSuccess);
            Assert.NotNull(orderedLesson.ErrorMessage);
            Assert.Equal("Specific term for lesson is unavailable", orderedLesson.ErrorMessage.Title);
        }
    }
}
