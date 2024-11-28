using CarRentalSystemAssignment.Models;
using CarRentalSystemAssignment.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalSystemAssignment.Services
{
    public class CarRentalService
    {
        private readonly CarRepository _carRepository;

        public CarRentalService(CarRepository carRepository)
        {
            _carRepository = carRepository;
        }

        public async Task<string> RentCar(int id)
        {
            var car = await _carRepository.GetCarById(id);
            if(car == null)
            {
                return "Car not found.";
            }

            if(!car.IsAvailable)
            {
                return "Car is not available.";
            }

            await _carRepository.UpdateCarAvailability(id, false);
            return "Car rented sucessfully.";
        }

        public async Task<bool> CheckCarAvailability(int id)
        {
            var car = await _carRepository.GetCarById(id);
            return car!=null && car.IsAvailable;
        }
    }
}
