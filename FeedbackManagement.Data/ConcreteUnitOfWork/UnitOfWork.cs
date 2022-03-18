using FeedbackManagement.Core.AbstractRepositories;
using FeedbackManagement.Core.AbstractUnitOfWork;
using FeedbackManagement.Data.ConcreteRepositories;
using FeedbackManagement.Data.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeedbackManagement.Data.ConcreteUnitOfWork
{
    public class UnitOfWork:IUnitOfWork
    {
        private readonly FeedbackManagementDBContext _context;
        private DepartmentRepository _departmentRepository;
        private FeedbackCategoryRepository _feedbackCategoryRepository;
        private FeedbackRepository _feedbackRepository;
        private UserFeedbackRepository _userFeedbackRepository;
        private UserRepository _userRepository;

        public UnitOfWork(FeedbackManagementDBContext context)
        {
            _context = context;
        }

        public IDepartmentRepository Departments => _departmentRepository= _departmentRepository ?? new DepartmentRepository(_context);
        public IFeedbackRepository Feedbacks => _feedbackRepository= _feedbackRepository ?? new FeedbackRepository(_context);
        public IFeedbackCategoryRepository FeedbackCategories => _feedbackCategoryRepository = _feedbackCategoryRepository ?? new FeedbackCategoryRepository(_context);
        public IUserFeedbackRepository UserFeedbacks => _userFeedbackRepository = _userFeedbackRepository ?? new UserFeedbackRepository(_context);
        public IUserRepository Users => _userRepository = _userRepository ?? new UserRepository(_context);

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
