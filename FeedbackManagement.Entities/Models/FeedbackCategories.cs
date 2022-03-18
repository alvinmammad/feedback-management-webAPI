using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
   public class FeedbackCategories:BaseEntity
    {
        public FeedbackCategories()
        {
            this.CreatedDate = DateTime.Now;
            Feedbacks = new HashSet<Feedback>();
        }
        
        public string CategoryName { get; set; }
        public int DepartmentID { get; set; }
        public virtual Department Department { get; set; }

        public virtual ICollection<Feedback> Feedbacks { get; set; }
    }
}
