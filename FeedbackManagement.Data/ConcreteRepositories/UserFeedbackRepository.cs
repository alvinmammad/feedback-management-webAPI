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
    public class UserFeedbackRepository : Repository<UserFeedback> , IUserFeedbackRepository
    {
        public UserFeedbackRepository(FeedbackManagementDBContext context) : base(context)
        {

        }

        private FeedbackManagementDBContext FeedbackManagementDBContext
        {
            get
            {
                return _context as FeedbackManagementDBContext;
            }
        }

        public async Task<UserFeedback> CheckFeedbackNoteAsync(int id)
        {
            var feedback = await _context.UserFeedbacks.Include(uf => uf.Feedback)
                .ThenInclude(uf => uf.FeedbackCategory)
                .ThenInclude(uf => uf.Department)
                .Where(uf => uf.FeedbackID == id).FirstOrDefaultAsync();
            return feedback;
        }

        public async Task<UserFeedback> CheckFeedbackNotePostAsync(int FeedbackID, string secretaryNote)
        {
            var secretaryFeedback =await _context.UserFeedbacks
               .Include(sf => sf.Feedback)
               .Where(sf => sf.FeedbackID == FeedbackID)
               .FirstOrDefaultAsync();
            secretaryFeedback.Feedback.FeedbackStatus = Core.Enums.FeedbackStatus.Rejected;
            secretaryFeedback.SecretaryNote = secretaryNote;
            secretaryFeedback.HRNote = secretaryFeedback.HRNote;
            secretaryFeedback.Feedback.CreatedDate = DateTime.Now;
            return secretaryFeedback;
        }

        public async Task<UserFeedback> GetCustomerAsync(int id)
        {
            var customer =await _context.UserFeedbacks.Include(f => f.Feedback).Where(f => f.Feedback.ID == id).AsNoTracking().FirstOrDefaultAsync();
            return customer;
        }

        public async Task<UserFeedback> GetHRFeedbackAsync(int id, int userID)
        {
            var feedback = await _context.UserFeedbacks
                .Include(uf => uf.Feedback)
                .ThenInclude(uf => uf.FeedbackCategory)
                .Where(uf => uf.AppUserID == userID && uf.ID == id && (uf.Feedback.FeedbackStatus == Core.Enums.FeedbackStatus.inProgress || uf.Feedback.FeedbackStatus == Core.Enums.FeedbackStatus.Rejected))
                .FirstOrDefaultAsync();
            return feedback;
        }

        public async Task<UserFeedback> HRFeedbackDetailsAsync(string HRNote, int FeedbackID, int currentUserID)
        {
            var userFeedback = await _context.UserFeedbacks.Include(uf => uf.Feedback).Where(uf => uf.FeedbackID == FeedbackID && uf.AppUserID == currentUserID).FirstOrDefaultAsync();
            userFeedback.Feedback.FeedbackStatus = Core.Enums.FeedbackStatus.inProgress;
            userFeedback.HRNote = HRNote;
            userFeedback.Feedback.CreatedDate = DateTime.Now;
            if (userFeedback.SecretaryNote == null)
            {
                userFeedback.SecretaryNote = null;
            }
            else
            {
                userFeedback.SecretaryNote = userFeedback.SecretaryNote;
            }

            await _context.SaveChangesAsync();
            return userFeedback;
        }

        public async Task<IEnumerable<UserFeedback>> HRFeedbackListAsync(int currentUserID)
        {
            var feedback = await _context.UserFeedbacks
                .Include(uf => uf.Feedback)
                .ThenInclude(uf => uf.FeedbackCategory)
                .ThenInclude(uf => uf.Department)
                .Where(uf => uf.AppUserID == currentUserID && (uf.Feedback.FeedbackStatus == Core.Enums.FeedbackStatus.inProgress || uf.Feedback.FeedbackStatus == Core.Enums.FeedbackStatus.Rejected))
                .ToListAsync();
            return feedback;
        }
    }
}
