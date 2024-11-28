using CarRentalSystemAssignment.Models;
using CarRentalSystemAssignment.Repositories;
using CarRentalSystemAssignment.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalSystemAssignment.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CarController : ControllerBase
    {
        private readonly CarRentalService _carRentalService;
        private readonly CarRepository _carRepository;

        public CarController(CarRentalService carRentalService, CarRepository carRepository)
        {
            _carRentalService = carRentalService;
            _carRepository = carRepository;
        }

        // Get available cars
        [HttpGet]
        public async Task<IActionResult> GetAvailableCars()
        {
            var cars = await _carRepository.GetAvailableCars();
            return Ok(cars);
        }

        //Add a car (Admin only)
        [Authorize(Policy = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddCar([FromBody] CarModel car)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _carRepository.AddCar(car);
            return CreatedAtAction(nameof(GetAvailableCars), new { id = car.Id }, car);
        }

        // Update car details (Admin only)
        [Authorize(Policy = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCar(int id, [FromBody] CarModel updatedCar)
        {
            var existingCar = await _carRepository.GetCarById(id);
            if (existingCar == null)
                return NotFound();

            existingCar.Make = updatedCar.Make;
            existingCar.Model = updatedCar.Model;
            existingCar.Year = updatedCar.Year;
            existingCar.PricePerDay = updatedCar.PricePerDay;
            existingCar.IsAvailable = updatedCar.IsAvailable;

            await _carRepository.UpdateCarAvailability(id, existingCar.IsAvailable);
            return NoContent();
        }

        // Delete a car (Admin only)
        [Authorize(Policy = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCar(int id)
        {
            var car = await _carRepository.GetCarById(id);
            if (car == null)
                return NotFound();

            await _carRepository.UpdateCarAvailability(id, false); // Mark car as unavailable (soft delete)
            return NoContent();
        }

        // Rent a car (User only)
        [Authorize(Policy = "User")]
        [HttpPost("rent")]
        public async Task<IActionResult> RentCar([FromBody] RentCarModel rentCarModel)
        {
            var isAvailable = await _carRentalService.CheckCarAvailability(rentCarModel.CarId);
            if (!isAvailable)
                return BadRequest("Car is not available for rent.");

            var result = await _carRentalService.RentCar(rentCarModel.CarId);
            return Ok(new { Message = result });
        }
    }

 
    public class RentCarModel
    {
        public int CarId { get; set; }
    }
}
