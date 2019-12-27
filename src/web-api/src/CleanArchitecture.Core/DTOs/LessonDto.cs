using System;
using System.ComponentModel.DataAnnotations;

namespace CleanArchitecture.Core.DTOs
{
    public class LessonDto
    {
        public Guid Id { get; set; }
        [Required]
        public Guid TutorId { get; set; }
        [Required]
        public Guid StudentId { get; set; }

        public string Location { get; set; }

        public bool AcceptedByTutor { get; set; }
        public bool CanceledByTutor { get; set; }
        public bool CancelledByStudent { get; set; }

        [Required]
        public double Length { get; set; }

        [Required]
        public DateTimeOffset Term { get; set; }

        [Required]
        public LessonTopicCategoryDto TopicCategory { get; set; }
    }
}
