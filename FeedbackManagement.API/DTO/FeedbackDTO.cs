using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FeedbackManagement.API.DTO
{
    public class FeedbackDTO
    {
        //public int ID { get; set; }
        [Display(Name ="Muhsteri")]
        public string CustomerName { get; set; }
        public string CustomerSurname { get; set; }
        public string CustomerEmail { get; set; }
        public string FeedbackDesc { get; set; }
        public FeedbackCategoryDTO FeedbackCategory { get; set; }


    }
}
