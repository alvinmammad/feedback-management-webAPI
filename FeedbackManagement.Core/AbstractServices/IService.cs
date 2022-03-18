using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeedbackManagement.Core.AbstractServices
{
    public interface IService<TEntity> where TEntity : class
    {
        Task<TEntity> GetByID(int id);
        Task<IEnumerable<TEntity>> GetAll();
        Task<TEntity> CreateAsync(TEntity entity);
        Task UpdateAsync(TEntity entityToUpdate,TEntity entity);
        Task DeleteAsync(TEntity entity);
    }
}
