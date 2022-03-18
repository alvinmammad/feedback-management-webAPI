using Entity;
using FeedbackManagement.Core.AbstractRepositories;
using FeedbackManagement.Data.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeedbackManagement.Data.ConcreteRepositories
{
    public class DepartmentRepository : Repository<Department> , IDepartmentRepository
    {
        public DepartmentRepository(FeedbackManagementDBContext context) : base(context)
        {

        }

        private FeedbackManagementDBContext FeedbackManagementDBContext
        {
            get
            {
                return _context as FeedbackManagementDBContext;
            }
        }
        

        
    }
}
