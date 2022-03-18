using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeedbackManagement.Core.AbstractServices
{
    public interface IUserService : IService<AppUser>
    {
        Task<AppUser> GetHRRoleAsync(int id);

        Task<IEnumerable<AppUser>> GetAllUsersAsync();

    }
}
