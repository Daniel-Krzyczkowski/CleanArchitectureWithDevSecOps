using CleanArchitecture.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.Entities
{
    public class Lesson : BaseEntity
    {
        public Guid TutorId { get; set; }

        public Guid StudentId { get; set; }

        public string Location { get; set; }

        public bool AcceptedByTutor { get; set; }
        public bool CanceledByTutor { get; set; }
        public bool CancelledByStudent { get; set; }

        public DateTimeOffset Term { get; set; }

        public double Length {get; set;}

        public Guid TopicCategoryId { get; set; }
        public LessonTopicCategory TopicCategory { get; set; }
    }
}
