using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
   public class Department:BaseEntity
    {
        public Department()
        {
            FeedbackCategories = new HashSet<FeedbackCategories>();
            this.CreatedDate = DateTime.Now;
        }
        
        public string DepName { get; set; }
        public virtual ICollection<FeedbackCategories> FeedbackCategories { get; set; }

    }
}
