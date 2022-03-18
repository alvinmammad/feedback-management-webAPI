using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FeedbackManagement.API.DTO.RegisterDTOs
{
    public class RegisterUserDTO
    {
        public string FirstName { get; set; }

        public string Surname { get; set; }

        public string Email { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        [Compare("Password")]
        public string RePassword { get; set; }

        public int DepartmentID { get; set; }
        public int RoleId { get; set; }
    }
}
