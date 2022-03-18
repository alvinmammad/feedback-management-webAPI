using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeedbackManagement.Core.AbstractRepositories
{
    public interface IUserFeedbackRepository : IRepository<UserFeedback>
    {
        Task<UserFeedback> GetHRFeedbackAsync(int id, int userID);
        Task<UserFeedback> CheckFeedbackNoteAsync(int id);
        Task<UserFeedback> CheckFeedbackNotePostAsync(int FeedbackID, string secretaryNote);
        Task<UserFeedback> HRFeedbackDetailsAsync(string HRNote, int FeedbackID, int currentUserID);
        Task<IEnumerable<UserFeedback>> HRFeedbackListAsync(int currentUserID);
        Task<UserFeedback> GetCustomerAsync(int id);
    }
}
