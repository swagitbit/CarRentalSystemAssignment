using CarRentalSystemAssignment.Data;
using CarRentalSystemAssignment.Models;
using Microsoft.EntityFrameworkCore;

namespace CarRentalSystemAssignment.Repositories
{
    public class UserRepository
    {
        private readonly SystemDbContext _context;

        public UserRepository(SystemDbContext context)
        {
            _context = context;
        }

        public async Task AddUser(UserModel user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task<UserModel> GetUserByEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<UserModel?> GetUserById(int id)
        {
            return await _context.Users.FindAsync(id);
        }
    }
}
