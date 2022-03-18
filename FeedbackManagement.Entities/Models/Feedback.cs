using FeedbackManagement.Entities.Enums;
using System;
using System.Collections.Generic;

namespace Entity
{
    public class Feedback:BaseEntity
   {
        public Feedback()
        {
            this.CreatedDate = DateTime.Now;
            UserFeedbacks = new HashSet<UserFeedback>();
        }

        public string CustomerName { get; set; }
        public string CustomerSurname { get; set; }
        public string CustomerEmail { get; set; }

        public string FeedbackDesc { get; set; }
        
        public string FeedbackFile { get; set; }

        public FeedbackStatus FeedbackStatus { get; set; }
        public bool EmailStatus { get; set; }

        public int FeedbackCategoryID { get; set; }
        public virtual FeedbackCategories FeedbackCategory { get; set; }

        public virtual ICollection<UserFeedback> UserFeedbacks { get; set; }



    }

}
