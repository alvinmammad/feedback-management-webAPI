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
    public class UserManager : IUserService
    {
        private readonly IUnitOfWork _unitofwork;

        public UserManager(IUnitOfWork unitofwork)
        {
            _unitofwork = unitofwork;
        }
        public Task<AppUser> CreateAsync(AppUser entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(AppUser entity)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<AppUser>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<AppUser>> GetAllUsersAsync()
        {
            return await _unitofwork.Users.GetAllUsersAsync();
        }

        public Task<AppUser> GetByID(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<AppUser> GetHRRoleAsync(int id)
        {
            return await _unitofwork.Users.GetHRRoleAsync(id);
        }

        public Task UpdateAsync(AppUser entityToUpdate, AppUser entity)
        {
            throw new NotImplementedException();
        }
    }
}
