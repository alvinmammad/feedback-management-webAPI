using FeedbackManagement.Core.AbstractRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeedbackManagement.Core.AbstractUnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IDepartmentRepository Departments { get; }
        IFeedbackCategoryRepository FeedbackCategories { get; }
        IFeedbackRepository Feedbacks { get; }
        IUserFeedbackRepository UserFeedbacks { get; }
        IUserRepository Users { get; }
        Task<int> CommitAsync();
    }
}
