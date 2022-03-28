using FeedbackManagement.API.DTO.HRFeedbackDTOs.HRFeedbackListDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeedbackManagement.API.DTO.HRFeedbackDTOs.HRFeedbackListDTOs
{
    public class ListUserFeedbackDTO
    {
        public int ID { get; set; }
        public string SecretaryNote { get; set; }
        public string HRNote { get; set; }
        public ListFeedbackDTO Feedback { get; set; }
    }
}
