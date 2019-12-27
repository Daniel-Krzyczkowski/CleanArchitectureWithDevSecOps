using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.DTOs
{
    public class UpdateTutorLearningProfileDto
    {
        public decimal PricePerHour { get; set; }
        public IList<LessonTopicCategoryDto> LessonTopicCategories { get; set; }
    }
}
