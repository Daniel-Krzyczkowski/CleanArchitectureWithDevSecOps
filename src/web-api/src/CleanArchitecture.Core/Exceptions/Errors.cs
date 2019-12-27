using CleanArchitecture.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.Exceptions
{
    public static class Errors
    {
        public static class General
        {
            public static Error SomethingWentWrong() =>
                new Error("Something went wrong", "Please try once again");
        }

        public static class User
        {
            public static Error UserProfileNotFound() =>
                new Error("User profile not found", "User is not registered yet");

            public static Error UserProfileTypeNotFound() =>
                new Error("User profile type not found", "User profile type has to be specified first: either student or tutor");

            public static Error UserProfileTypeIsAlreadyDefinied(ProfileType profileType) =>
                    new Error("User profile type was already definied", $"User profile is set to: {profileType} and cannot be changed. Please create new account");

            public static Error OperationNotAllowedForSpecificUserProfileType(string profileType) =>
                    new Error("Operation not allowed", $"Operation is not allowed for the {profileType} profile type");

            public static Error UserProfileCannotBeLoaded() => new Error("User profile cannot be loaded",
                                                                                            "Please try again");

            public static Error UserProfileCannotBeUpdated() => new Error("User profile cannot be updated",
                                                                                "Please try again");

            public static Error UserProfileCannotBeDeleted() => new Error("User profile cannot be deleted",
                                                                    "Please try again");
        }

        public static class Tutor
        {
            public static Error TutorsNotFoundForLessonCategory() =>
                new Error("Tutors not found for provided lesson category", "No tutor registered for provided lesson category");

            public static Error TutorLearningProfileNotFound() =>
                new Error("Tutor learning profile not found", "There is no such learning profile for tutor");

            public static Error TutorLearningProfileAlreadyExists() =>
                 new Error("Tutor learning profiles already exists", "Please update existing learning profile");

            public static Error TutorLearningProfileLessonCategoryNotFound() =>
                new Error("Lesson topic category not found", "Please provide at least one lesson topic category for learing profile");
        }

        public static class Lesson
        {
            public static Error LessonNotFound() =>
               new Error("Lesson not found", "Specific lesson is not registered in the system");

            public static Error LessonIdIsEmpty() =>
               new Error("Lesson Id is empty", "Lesson Id is not correct");

            public static Error StudentIdForLessonIsEmpty() =>
                new Error("Student Id is empty", "Student Id is not correct");

            public static Error TutorIdForLessonIsEmpty() =>
                new Error("Tutor Id is empty", "Tutor Id is not correct");

            public static Error NoLessonsHistoryFound() =>
               new Error("Lesson history is empty", "There is no lesson yet in the history");

            public static Error NoActiveLessonForTutor() =>
               new Error("There is not active lesson", "Tutor has no active lessons");

            public static Error NoActiveLessonForStudent() =>
                new Error("There is not active lesson", "Student has no active lessons");

            public static Error TooShortTimePeriodBetweenLessons() =>
               new Error("Specific term for lesson is unavailable", "After every lesson there is 30 minutes interval. Tutor has lesson which ends 30 minutes before proposed term");

            public static Error TutorAlreadyBookedForLessonProposedTime() =>
               new Error("Tutor is not available for proposed time", "Tutor is already booked for the lesson during proposed time");

            public static Error LessonTopicCategoriesNotFound() =>
               new Error("Lesson topic categories not found", "There are no lesson topic categories available");
        }

        public static class Chat
        {
            public static Error ChatMessageCannotBeSaved() =>
               new Error("Chat message cannot be saved", "An error occurred and chat message was not saved");
        }
    }
}
