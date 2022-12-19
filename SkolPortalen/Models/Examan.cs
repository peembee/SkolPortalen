using System;
using System.Collections.Generic;

namespace SkolPortalen.Models
{
    public partial class Examan
    {
        public int ExamenId { get; set; }
        public int FkStudentId { get; set; }
        public int FkCourseId { get; set; }
        public int FkEmployeeId { get; set; }
        public decimal ExamenGrade { get; set; }
        public DateTime ExamenDate { get; set; }

        public virtual Course FkCourse { get; set; } = null!;
        public virtual Employee FkEmployee { get; set; } = null!;
        public virtual Student FkStudent { get; set; } = null!;
    }
}
