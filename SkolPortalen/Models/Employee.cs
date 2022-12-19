using System;
using System.Collections.Generic;

namespace SkolPortalen.Models
{
    public partial class Employee
    {
        //
        public Employee()
        {
            Courses = new HashSet<Course>();
            Examen = new HashSet<Examan>();           
        }
        public Employee(string firstname, string lastname, string gender, int titleId, int salaryId)
        {
            EmpFirstName = firstname;
            EmpLastname = lastname;
            EmpGender = gender;
            FkTitleId = titleId;
            FkSalaryId = salaryId;
            EmploymentDate = DateTime.Now;           
        }
        public int EmployeeId { get; set; }
        public int FkTitleId { get; set; }
        public int FkSalaryId { get; set; }
        public string EmpFirstName { get; set; } = null!;
        public string EmpLastname { get; set; } = null!;
        public string EmpGender { get; set; } = null!;
        public DateTime EmploymentDate { get; set; }

        public virtual Salary FkSalary { get; set; } = null!;
        public virtual Title FkTitle { get; set; } = null!;
        public virtual ICollection<Course> Courses { get; set; }
        public virtual ICollection<Examan> Examen { get; set; }
    }
}
