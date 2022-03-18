using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeedbackManagement.Core.AbstractRepositories
{
   public interface IFeedbackCategoryRepository:IRepository<FeedbackCategories>
    {
       Task<FeedbackCategories> GetCategoryAsync(int id);
       Task<IEnumerable<FeedbackCategories>> GetCategoriesAsync();
    }
}
