using System;
using System.Collections.Generic;

namespace SkolPortalen.Models
{
    public partial class Classroom
    {
        public Classroom()
        {
            Classes = new HashSet<Class>();
        }

        public int ClassroomId { get; set; }
        public string? ClassroomName { get; set; }
        public string? ClassroomNumber { get; set; }

        public virtual ICollection<Class> Classes { get; set; }
    }
}
