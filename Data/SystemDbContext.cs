using Microsoft.EntityFrameworkCore;
using CarRentalSystemAssignment.Models;

namespace CarRentalSystemAssignment.Data
{
    public class SystemDbContext : DbContext
    {
        public SystemDbContext(DbContextOptions<SystemDbContext> options) : base(options) { }

        public DbSet<UserModel> Users { get; set; }
        public DbSet<CarModel> Cars { get; set; }

       
    }

    
    }
