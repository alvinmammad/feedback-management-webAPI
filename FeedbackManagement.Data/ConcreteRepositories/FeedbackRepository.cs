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
    public class FeedbackRepository : Repository<Feedback>, IFeedbackRepository
    {
        public FeedbackRepository(FeedbackManagementDBContext context) : base(context)
        {

        }

        private FeedbackManagementDBContext FeedbackManagementDBContext
        {
            get
            {
                return _context as FeedbackManagementDBContext;
            }
        }

        public async Task<UserFeedback> GetCustomerAsync(int id)
        {
            var customer = await _context.UserFeedbacks.Include(f => f.Feedback).Where(f => f.Feedback.ID == id).AsNoTracking().FirstOrDefaultAsync();
            return customer;
        }

        

        public async Task<IEnumerable<Feedback>> GetFeedbacksWithCategoryAsync()
        {
             return await _context.Feedbacks.Include(f => f.FeedbackCategory).ThenInclude(f=>f.Department).OrderByDescending(f => f.FeedbackStatus).ToListAsync();

        }

        public async Task<Feedback> GetFeedbackWithCategoryAsync(int id)
        {
            var feedback = await _context.Feedbacks
               .Include(f => f.FeedbackCategory)
               .Where(f => f.ID == id)
               .FirstOrDefaultAsync();
            return feedback;
        }

        public async Task UpdateAsync(Feedback feedback)
        {
             _context.Feedbacks.Update(feedback);
            await _context.SaveChangesAsync();
        }
    }
}
