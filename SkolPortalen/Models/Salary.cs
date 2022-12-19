using System;
using System.Collections.Generic;

namespace SkolPortalen.Models
{
    public partial class Salary
    {
        public Salary()
        {
            Employees = new HashSet<Employee>();
        }

        public int SalaryId { get; set; }
        public string? SalaryStages { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }
    }
}
