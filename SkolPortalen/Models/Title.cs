using System;
using System.Collections.Generic;

namespace SkolPortalen.Models
{
    public partial class Title
    {
        public Title()
        {
            Employees = new HashSet<Employee>();
        }

        public int TitleId { get; set; }
        public string? TitelName { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }
    }
}
