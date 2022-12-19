using System;
using System.Collections.Generic;

namespace SkolPortalen.Models
{
    public partial class Course
    {
        public Course()
        {
            Classes = new HashSet<Class>();
            Examen = new HashSet<Examan>();
        }

        public int CourseId { get; set; }
        public int FkEmployeeId { get; set; }
        public string CourseName { get; set; } = null!;
        public string CourseStatus { get; set; } = null!;
        public int CoursePoints { get; set; }

        public virtual Employee FkEmployee { get; set; } = null!;
        public virtual ICollection<Class> Classes { get; set; }
        public virtual ICollection<Examan> Examen { get; set; }
    }
}
