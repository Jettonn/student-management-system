using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMS.Application.ViewModels.Lesson
{
    public class LessonViewModel
    {
        public List<LessonDto> lessonDto { get; set; }
        public StartLesson startLesson { get; set; }
    }

    public class LessonDto
    {
        public Guid ClassId { get; set; }
        public string ClassName { get; set; }
        public string ClassComment { get; set; }
        public int PlannedHours { get; set; }
        public string SubjectName { get; set; }
        public int StudiesYear { get; set; }
        public string SubjectColor { get; set; }
    }

    public class StartLesson
    {
        public Guid ClassId { get; set; }
        public int Hours { get; set; }
        public string Comment { get; set; }
        public DateTime Date { get; set; }
    }
}
