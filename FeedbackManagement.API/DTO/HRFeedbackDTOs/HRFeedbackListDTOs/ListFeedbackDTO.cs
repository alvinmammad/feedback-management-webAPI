using FeedbackManagement.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeedbackManagement.API.DTO.HRFeedbackDTOs.HRFeedbackListDTOs
{
    public class ListFeedbackDTO
    {
        public int ID { get; set; }
        public string CustomerName { get; set; }
        public string CustomerSurname { get; set; }
        public string CustomerEmail { get; set; }
        public string FeedbackFile { get; set; }

        public DateTime CreatedDate { get; set; }

        public FeedbackStatus FeedbackStatus { get; set; }

        public string FeedbackDesc { get; set; }

        public ListFeedbackCategoryDTO FeedbackCategory { get; set; }
    }
}
