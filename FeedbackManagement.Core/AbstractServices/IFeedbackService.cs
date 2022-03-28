using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeedbackManagement.Core.AbstractServices
{
    public interface IFeedbackService : IService<Feedback>
    {
        Task<Feedback> GetFeedbackWithCategoryAsync(int id);
        Task<IEnumerable<Feedback>> GetFeedbacksWithCategoryAsync();

        Task UpdateAsync(Feedback feedback);

       
    }
}
