using CarRentalSystemAssignment.Data;
using CarRentalSystemAssignment.Models;
using Microsoft.EntityFrameworkCore;

namespace CarRentalSystemAssignment.Repositories
{
    public class CarRepository
    {
        private readonly SystemDbContext _context;

        public CarRepository(SystemDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CarModel>> GetAvailableCars()
        {
            return await _context.Cars.Where(c => c.IsAvailable).ToListAsync();
        }

        public async Task<CarModel?> GetCarById(int id)
        {
            return await _context.Cars.FindAsync(id);
        }

        public async Task UpdateCarAvailability(int id, bool isAvailable)
        {
            var car = await GetCarById(id);
            if(car !=null)
            {
                car.IsAvailable = isAvailable;
                await _context.SaveChangesAsync();
            }
        }

        public async Task AddCar(CarModel car)
        {
            await _context.Cars.AddAsync(car);
            await _context.SaveChangesAsync();
        }
    }
}
