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
    public class FeedbackManager : IFeedbackService
    {
        private readonly IUnitOfWork _unitofwork;

        public FeedbackManager(IUnitOfWork unitofwork)
        {
            _unitofwork = unitofwork;
        }
        public async Task<Feedback> CreateAsync(Feedback entity)
        {
            await _unitofwork.Feedbacks.CreateAsync(entity);
            await _unitofwork.CommitAsync();
            return entity;
        }

        public async Task DeleteAsync(Feedback entity)
        {
            _unitofwork.Feedbacks.Delete(entity);
            await _unitofwork.CommitAsync();
        }

        public async Task<IEnumerable<Feedback>> GetAll()
        {
            return await _unitofwork.Feedbacks.GetAll();
        }

        public async Task<Feedback> GetByID(int id)
        {
            return await _unitofwork.Feedbacks.GetByIDAsync(id);
        }

        public async Task UpdateAsync(Feedback entityToUpdate, Feedback entity)
        {
            entityToUpdate.CustomerEmail = entity.CustomerEmail;
            entityToUpdate.CustomerName = entity.CustomerName;
            entityToUpdate.CustomerSurname = entity.CustomerSurname;
            entityToUpdate.EmailStatus = entity.EmailStatus;
            entityToUpdate.FeedbackCategoryID = entity.FeedbackCategoryID;
            entityToUpdate.FeedbackDesc = entity.FeedbackDesc;
            entityToUpdate.FeedbackFile = entity.FeedbackFile;
            entityToUpdate.FeedbackStatus = entity.FeedbackStatus;
            await _unitofwork.CommitAsync();
        }

        public async Task<IEnumerable<Feedback>> GetFeedbacksWithCategoryAsync()
        {
            return await _unitofwork.Feedbacks.GetFeedbacksWithCategoryAsync();
        }

        public async Task<Feedback> GetFeedbackWithCategoryAsync(int id)
        {
            return await _unitofwork.Feedbacks.GetFeedbackWithCategoryAsync(id);
        }

    }
}
