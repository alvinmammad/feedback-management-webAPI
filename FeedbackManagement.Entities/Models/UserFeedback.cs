using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
   public class UserFeedback:BaseEntity
    {
        public UserFeedback()
        {
            this.CreatedDate = DateTime.Now;

        }
        public string SecretaryNote { get; set; }
        public string HRNote { get; set; }
        public int FeedbackID { get; set; }
        public virtual Feedback Feedback { get; set; }
        public int AppUserID { get; set; }
        

    }
}
