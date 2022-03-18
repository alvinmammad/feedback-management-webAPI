using FeedbackManagement.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeedbackManagement.API.DTO.FeedbackDetailDTOs
{
    public class FeedbackDTO
    {
        public string CustomerName { get; set; }
        public string CustomerSurname { get; set; }
        public string CustomerEmail { get; set; }
        public string FeedbackDesc { get; set; }
        public string FeedbackFile { get; set; }

        public FeedbackStatus FeedbackStatus { get; set; }
        public FeedbackCategoryDetailDTO FeedbackCategory { get; set; }

    }
}
