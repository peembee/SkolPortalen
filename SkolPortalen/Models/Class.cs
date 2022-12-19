using System;
using System.Collections.Generic;

namespace SkolPortalen.Models
{
    public partial class Class
    {
        public Class(int fkStudentId, int fkCourseId, int fkClassroomId)
        {
            FkStudentId = fkStudentId;
            FkCourseId = fkCourseId;
            FkClassroomId = fkClassroomId;
        }
        public int ClassId { get; set; }
        public int FkStudentId { get; set; }
        public int FkCourseId { get; set; }
        public int FkClassroomId { get; set; }

        public virtual Classroom FkClassroom { get; set; } = null!;
        public virtual Course FkCourse { get; set; } = null!;
        public virtual Student FkStudent { get; set; } = null!;
    }
}
