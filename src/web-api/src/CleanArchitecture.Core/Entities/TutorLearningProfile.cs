using CleanArchitecture.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.Entities
{
    public class TutorLearningProfile : BaseEntity
    {
        public Guid TutorId { get; set; }
        public decimal PricePerHour { get; set; }
        public IList<LessonTopicCategory> LessonTopicCategories { get; set; }
    }
}
