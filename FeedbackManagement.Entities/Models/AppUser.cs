using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Entity
{
   public class AppUser:IdentityUser<int>
   {
        public AppUser()
        {
            UserFeedbacks = new HashSet<UserFeedback>();
           
        }
        public string FirstName { get; set; }
        public string Surname { get; set; }

        public virtual Department Department { get; set; }
        public int DepartmentID { get; set; }

        public  ICollection<UserFeedback> UserFeedbacks;
        public ICollection<AppUserRole> UserRoles { get; set; }


    }
}
