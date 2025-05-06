using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TravelingAPI.DTOs.Travel;
using TravelingAPI.Interfaces;

namespace TravelingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TripController : ControllerBase
    {
        private readonly ITripService _tripService;

        public TripController(ITripService tripService)
        {
            _tripService = tripService;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromForm] TripCreateDto tripDto)
        {
            if (tripDto.Image == null || tripDto.Image.Length == 0)
                return BadRequest("No image uploaded.");

            var tripDtoResult = await _tripService.CreateTripAsync(tripDto);
            tripDtoResult.ImageUrl = $"{Request.Scheme}://{Request.Host}{tripDtoResult.ImageUrl}";
            return CreatedAtAction(nameof(GetById), new { id = tripDtoResult.Id }, tripDtoResult);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var trip = await _tripService.GetTripByIdAsync(id);
            if (trip == null)
                return NotFound();

            trip.ImageUrl = $"{Request.Scheme}://{Request.Host}{trip.ImageUrl}";
            return Ok(trip);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var trips = await _tripService.GetAllTripsAsync();

            foreach (var trip in trips)
            {
                trip.ImageUrl = $"{Request.Scheme}://{Request.Host}{trip.ImageUrl}";
            }

            return Ok(trips);
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update([FromForm] TripUpdateDto dto)
        {
            var result = await _tripService.UpdateTripAsync(dto);
            if (result == null)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _tripService.DeleteTripAsync(id);
            return result ? NoContent() : NotFound();
        }
    }
}
