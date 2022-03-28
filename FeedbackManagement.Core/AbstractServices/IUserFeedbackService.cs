using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeedbackManagement.Core.AbstractServices
{
    public interface IUserFeedbackService : IService<UserFeedback>
    {
        Task<UserFeedback> GetHRFeedbackAsync(int id, int userID);
        Task<UserFeedback> CheckFeedbackNoteAsync(int id);
        Task<UserFeedback> CheckFeedbackNotePostAsync(int FeedbackID, string secretaryNote);
        Task<UserFeedback> HRFeedbackDetailsAsync(string HRNote, int FeedbackID, int currentUserID);
        Task<UserFeedback> GetCustomerAsync(int id);
        Task<IEnumerable<UserFeedback>> HRFeedbackListAsync(int currentUserID);
        


    }
}
