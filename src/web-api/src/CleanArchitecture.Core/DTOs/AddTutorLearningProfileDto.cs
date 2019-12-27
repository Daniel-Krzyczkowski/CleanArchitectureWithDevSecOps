using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.DTOs
{
    public class AddTutorLearningProfileDto
    {
        [Required]
        public decimal PricePerHour { get; set; }
        [Required]
        public IList<LessonTopicCategoryDto> LessonTopicCategories { get; set; }
    }
}
