using Microsoft.EntityFrameworkCore;
using PCBuilder.Repository.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace PCBuilder.Repository.Repository
{
    public interface IUserRepository
    {
        /// <summary>
        /// Return all companies including records marked as deleted and disabled
        /// </summary>
        /// <returns>Models.User</returns>
        Task<ICollection<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(int id);
        Task<User> CreateUserAsync(User user);
        Task<User> UpdateUserAsync(User user);
        Task<bool> DeleteUserAsync(int id);
        


    }
    public class UserRepository : IUserRepository
    {
        private readonly PcBuildingContext _context;
        public UserRepository(PcBuildingContext context)
        {
            this._context = context;
        }

        public async Task<ICollection<User>> GetAllUsersAsync()
        {
            // load list user co isActive true
            return await _context.Users.Where(p => p.IsActive == true).ToListAsync();
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }
        
        public async Task<User> CreateUserAsync(User user)
        {
            user.RoleId = 1;
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return false;
            }
            // khong xoa chi thay doi isActive field
            user.IsActive = false;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
