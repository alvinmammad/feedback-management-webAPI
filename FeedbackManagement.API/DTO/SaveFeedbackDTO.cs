using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeedbackManagement.API.DTO
{
    public class SaveFeedbackDTO
    {
        //public int ID { get; set; }
        public string CustomerName { get; set; }
        public string CustomerSurname { get; set; }
        public string CustomerEmail { get; set; }
        public string FeedbackDesc { get; set; }
        public int FeedbackCategoryID { get; set; }

    }
}
