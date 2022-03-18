using Entity;
using FeedbackManagement.Core.AbstractRepositories;
using FeedbackManagement.Data.DAL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeedbackManagement.Data.ConcreteRepositories
{
    public class FeedbackCategoryRepository : Repository<FeedbackCategories> , IFeedbackCategoryRepository
    {
        public FeedbackCategoryRepository(FeedbackManagementDBContext context) : base(context)
        {

        }

        private FeedbackManagementDBContext FeedbackManagementDBContext
        {
            get
            {
                return _context as FeedbackManagementDBContext;
            }
        }

        public async Task<IEnumerable<FeedbackCategories>> GetCategoriesAsync()
        {
            List<FeedbackCategories> feedbackCategories = await _context.FeedbackCategories
                .Include(fc => fc.Department)
                .OrderByDescending(fc => fc.CreatedDate)
                .ToListAsync();
            return feedbackCategories;
        }

        public async Task<FeedbackCategories> GetCategoryAsync(int id)
        {
             FeedbackCategories feedbackCategory = await _context.FeedbackCategories
                 .Include(fc => fc.Department)
                 .Where(fc => fc.ID == id).FirstOrDefaultAsync();
            return feedbackCategory;
        }
    }
}
