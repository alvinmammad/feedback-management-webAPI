using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace FeedbackManagement.API.DTO.CheckFeedbackNoteDTOs
{
    public class FeedbackCategoryCheckDTO
    {
        //public int ID { get; set; }
        
        public string CategoryName { get; set; }
        public DepartmentCheckDTO Department { get; set; }
    }
}
