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
    public class DepartmentManager : IDepartmentService
    {
        private readonly IUnitOfWork _unitofwork;

        public DepartmentManager(IUnitOfWork unitofwork)
        {
            _unitofwork = unitofwork;
        }
        public async Task<Department> CreateAsync(Department entity)
        {
            await _unitofwork.Departments.CreateAsync(entity);
            await _unitofwork.CommitAsync();
            return entity;
        }

        public async Task DeleteAsync(Department entity)
        {
             _unitofwork.Departments.Delete(entity);
             await _unitofwork.CommitAsync();
        }

        public async Task<IEnumerable<Department>> GetAll()
        {
            return await _unitofwork.Departments.GetAll();
        }

        public async Task<Department> GetByID(int id)
        {
            return await _unitofwork.Departments.GetByIDAsync(id);
        }

        public async Task UpdateAsync(Department entityToUpdate, Department entity)
        {
            entityToUpdate.DepName = entity.DepName;
            entityToUpdate.CreatedDate = entity.CreatedDate;
            await _unitofwork.CommitAsync();
        }
    }
}
