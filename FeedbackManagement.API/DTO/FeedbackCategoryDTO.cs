using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeedbackManagement.API.DTO
{
    public class FeedbackCategoryDTO
    {
        //public int ID { get; set; }

        public string CategoryName { get; set; }

        public DepartmentDTO Department { get; set; }
    }
}
