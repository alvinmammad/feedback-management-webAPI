using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeedbackManagement.API.DTO.HRFeedbackDTOs.HRFeedbackListDTOs
{
    public class ListFeedbackCategoryDTO
    {
        public string CategoryName { get; set; }
        public ListDepartmentDTO Department { get; set; }
    }
}
