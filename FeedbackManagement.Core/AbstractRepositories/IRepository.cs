using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FeedbackManagement.Core.AbstractRepositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<TEntity> GetByIDAsync(int id);
        Task<IEnumerable<TEntity>> GetAll();
        Task CreateAsync(TEntity entity);
        void Delete(TEntity entity);
        
    }
}
