using System;
using System.ComponentModel.DataAnnotations;

namespace CleanArchitecture.Core.DTOs
{
    public class OrderLessonDto
    {
        [Required]
        public Guid TutorId { get; set; }
        [Required]
        public string Location { get; set; }
        [Required]
        public DateTimeOffset Term { get; set; }
        [Required]
        public LessonTopicCategoryDto TopicCategory { get; set; }
        [Required]
        public double Length { get; set; }
    }
}
