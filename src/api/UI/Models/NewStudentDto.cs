using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UI.Models
{
    public class NewStudentDto
    {
        [Required(ErrorMessage = "Name cannot be empty")]
        public string Name { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Name cannot be empty")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Name cannot be empty")]
        public string Course1 { get; set; }

        public Grade Course1Grade { get; set; }

        public string Course2 { get; set; }

        public Grade? Course2Grade { get; set; }
    }

    public enum Grade
    {
        A = 5,
        B = 4,
        C = 3,
        D = 2,
        E = 1
    }
}
