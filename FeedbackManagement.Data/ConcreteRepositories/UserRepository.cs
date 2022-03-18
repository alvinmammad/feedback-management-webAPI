using Entity;
using FeedbackManagement.Core.AbstractRepositories;
using FeedbackManagement.Data.DAL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeedbackManagement.Data.ConcreteRepositories
{
   public class UserRepository : Repository<AppUser> , IUserRepository
    {
        public UserRepository(FeedbackManagementDBContext context) : base(context)
        {

        }

        private FeedbackManagementDBContext FeedbackManagementDBContext
        {
            get
            {
                return _context as FeedbackManagementDBContext;
            }
        }

        public async Task<IEnumerable<AppUser>> GetAllUsersAsync()
        {
            var users = await _context.Users.Include(u => u.Department).Include(u => u.UserRoles).ThenInclude(u => u.Role).ToListAsync();
            return users;
        }

        public async Task<AppUser> GetHRRoleAsync(int id)
        {
            var user = await _context.Users
                 .Include(u => u.UserRoles)
                 .Include(u => u.Department)
                 .Where(u => u.DepartmentID == id && u.UserRoles
                 .Any(ur => ur.RoleId == 3))
                 .FirstOrDefaultAsync();
            return user;
        }
    }
}
