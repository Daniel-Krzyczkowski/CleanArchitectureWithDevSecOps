using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanArchitecture.Core.DTOs
{
    public class TutorLearningProfileDto
    {
        public Guid Id { get; set; }
        public Guid TutorId { get; set; }
        public decimal PricePerHour { get; set; }
        public IList<LessonTopicCategoryDto> LessonTopicCategories { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Mail { get; set; }
    }
}
