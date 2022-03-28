using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeedbackManagement.API.DTO.CheckFeedbackNoteDTOs
{
    public class UserFeedbackCheckDTO
    {
        public int ID { get; set; }
        public string SecretaryNote  { get; set; }
        public string HRNote { get; set; }
        public FeedbackCheckDTO Feedback { get; set; }

    }
}
