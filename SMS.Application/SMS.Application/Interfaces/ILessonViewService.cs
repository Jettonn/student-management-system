using SMS.Application.ViewModels.Lesson;
using SMS.Application.ViewModels.Subjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMS.Application.Interfaces
{
    public interface ILessonViewService
    {
        public string StartLesson(LessonViewModel model);
        List<StudentAttendanceViewModel> GetAttendanceListByLessonId(Guid lessonId);
        public void EditStudentAttendance(Guid attendanceId, int attendanceStatus);
    }
}
