using Entity;
using FeedbackManagement.Core.AbstractServices;
using FeedbackManagement.Core.AbstractUnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeedbackManagement.Business.ConcreteServices
{
    public class FeedbackCategoryManager : IFeedbackCategoryService
    {
        private readonly IUnitOfWork _unitofwork;

        public FeedbackCategoryManager(IUnitOfWork unitofwork)
        {
            _unitofwork = unitofwork;
        }
        public async Task<FeedbackCategories> CreateAsync(FeedbackCategories entity)
        {
            await _unitofwork.FeedbackCategories.CreateAsync(entity);
            await _unitofwork.CommitAsync();
            return entity;
        }

        public async Task DeleteAsync(FeedbackCategories entity)
        {
             _unitofwork.FeedbackCategories.Delete(entity);
            await _unitofwork.CommitAsync();
        }

        public async Task<IEnumerable<FeedbackCategories>> GetAll()
        {
            return await _unitofwork.FeedbackCategories.GetAll();
        }

        public async Task<FeedbackCategories> GetByID(int id)
        {
            return await _unitofwork.FeedbackCategories.GetByIDAsync(id);

        }

        public async Task UpdateAsync(FeedbackCategories entityToUpdate, FeedbackCategories entity)
        {
            entityToUpdate.CategoryName = entity.CategoryName;
            entityToUpdate.DepartmentID = entity.DepartmentID;
            entityToUpdate.CreatedDate = entity.CreatedDate;
            await _unitofwork.CommitAsync();
        }

        public async Task<IEnumerable<FeedbackCategories>> GetCategoriesAsync()
        {
            return await _unitofwork.FeedbackCategories.GetCategoriesAsync();
        }

        public async Task<FeedbackCategories> GetCategoryAsync(int id)
        {
            return await _unitofwork.FeedbackCategories.GetCategoryAsync(id);
        }
    }
}
