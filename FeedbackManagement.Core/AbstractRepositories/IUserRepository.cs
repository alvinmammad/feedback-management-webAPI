using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeedbackManagement.Core.AbstractRepositories
{
    public interface IUserRepository : IRepository<AppUser>
    {
        Task<AppUser> GetHRRoleAsync(int id);
        Task<IEnumerable<AppUser>> GetAllUsersAsync();

    }
}
