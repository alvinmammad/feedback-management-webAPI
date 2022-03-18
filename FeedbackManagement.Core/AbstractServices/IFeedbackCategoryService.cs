using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeedbackManagement.Core.AbstractServices
{
   public interface IFeedbackCategoryService:IService<FeedbackCategories>
    {
        Task<FeedbackCategories> GetCategoryAsync(int id);
        Task<IEnumerable<FeedbackCategories>> GetCategoriesAsync();
    }
}
