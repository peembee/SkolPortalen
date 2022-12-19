using System;
using System.Collections.Generic;

namespace SkolPortalen.Models
{
    public partial class Student
    {
        public Student()
        {
            Classes = new HashSet<Class>();
            Examen = new HashSet<Examan>();
            Phones = new HashSet<Phone>();
        }
        public Student(string personNumber, string firstname, string lastname, string gender, string email)
        {
            StudPersonNumber = personNumber;
            StudFirstName = firstname;
            StudLastName = lastname;
            StudGender = gender;
            StudEmail = email;
        }
        public Student(string personNumber, string firstname, string lastname, string gender) // if student don't have any email
        {
            StudPersonNumber = personNumber;
            StudFirstName = firstname;
            StudLastName = lastname;
            StudGender = gender;
        }
        public int StudentId { get; set; }
        public string StudPersonNumber { get; set; } = null!;
        public string StudFirstName { get; set; } = null!;
        public string StudLastName { get; set; } = null!;
        public string StudGender { get; set; } = null!;
        public string? StudEmail { get; set; }

        public virtual ICollection<Class> Classes { get; set; }
        public virtual ICollection<Examan> Examen { get; set; }
        public virtual ICollection<Phone> Phones { get; set; }
    }
}
