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
    public class UserFeedbackManager : IUserFeedbackService
    {
        private readonly IUnitOfWork _unitofwork;

        public UserFeedbackManager(IUnitOfWork unitofwork)
        {
            _unitofwork = unitofwork;
        }
        public async Task<UserFeedback> CheckFeedbackNoteAsync(int id)
        {
            return await _unitofwork.UserFeedbacks.CheckFeedbackNoteAsync(id);
        }

        public async Task<UserFeedback> CheckFeedbackNotePostAsync(int FeedbackID, string secretaryNote)
        {
            return await _unitofwork.UserFeedbacks.CheckFeedbackNotePostAsync(FeedbackID, secretaryNote);
        }

        public async Task<UserFeedback> CreateAsync(UserFeedback entity)
        {
            await _unitofwork.UserFeedbacks.CreateAsync(entity);
            await _unitofwork.CommitAsync();
            return entity;
        }

        public async Task DeleteAsync(UserFeedback entity)
        {
            _unitofwork.UserFeedbacks.Delete(entity);
            await _unitofwork.CommitAsync();
        }

        public async Task<IEnumerable<UserFeedback>> GetAll()
        {
            return await _unitofwork.UserFeedbacks.GetAll();
        }

        public async Task<UserFeedback> GetByID(int id)
        {
            return await _unitofwork.UserFeedbacks.GetByIDAsync(id);
        }

        public async Task<UserFeedback> GetCustomerAsync(int id)
        {
            return await _unitofwork.UserFeedbacks.GetCustomerAsync(id);
        }

        public async Task<UserFeedback> GetHRFeedbackAsync(int id, int userID)
        {
            return await _unitofwork.UserFeedbacks.GetHRFeedbackAsync(id, userID);
        }

        public async Task<UserFeedback> HRFeedbackDetailsAsync(string HRNote, int FeedbackID, int currentUserID)
        {
            return await _unitofwork.UserFeedbacks.HRFeedbackDetailsAsync(HRNote, FeedbackID,currentUserID);
        }

        public async Task<IEnumerable<UserFeedback>> HRFeedbackListAsync(int currentUserID)
        {
            return await _unitofwork.UserFeedbacks.HRFeedbackListAsync(currentUserID);
        }

        

        public Task UpdateAsync(UserFeedback entityToUpdate, UserFeedback entity)
        {
            throw new NotImplementedException();
        }

        //public async Task UpdateAsync(UserFeedback entityToUpdate, UserFeedback entity)
        //{
        //    entityToUpdate.AppUserID = entity.AppUserID;
        //    entityToUpdate.CreatedDate = entity.CreatedDate;
        //    entityToUpdate.FeedbackID = entity.FeedbackID;
        //    entityToUpdate.HRNote = entity.HRNote;
        //    entityToUpdate.SecretaryNote = entity.SecretaryNote;
        //    await _unitofwork.CommitAsync();
        //    await _unitofwork.FeedbackCategories.up
        //}
    }
}
