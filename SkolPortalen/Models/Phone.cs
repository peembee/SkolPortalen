using System;
using System.Collections.Generic;

namespace SkolPortalen.Models
{
    public partial class Phone
    {
        public int PhoneId { get; set; }
        public int? FkStudentId { get; set; }
        public string? PhoneNumber { get; set; }

        public virtual Student? FkStudent { get; set; }
    }
}
