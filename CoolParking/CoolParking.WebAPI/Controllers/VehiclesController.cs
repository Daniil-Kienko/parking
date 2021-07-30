using System;
using System.Linq;
using CoolParking.BL.Interfaces;
using CoolParking.BL.Models;
using CoolParking.WebAPI.DTO;
using CoolParking.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CoolParking.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehiclesController : ControllerBase
    {
        private IParkingService parkingService;

        public VehiclesController(IParkingService parkingService)
        {
            this.parkingService = parkingService;
        }

        [HttpGet]
        public ActionResult<Vehicle[]> GetAllVehicles()
        {
            return Ok(parkingService.GetVehicles().ToArray());
        }

        [HttpPost]
        public ActionResult<Vehicle> CreateVehicle([FromBody] CreateVehicleContract vehicleContract)
        {
            if (!ModelState.IsValid)
            {
                string messages = string.Join("; ", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));
                return BadRequest(new ErrorResponseData(400, "Bad Request", messages));
            }
            try
            {
                parkingService.AddVehicle(new Vehicle(vehicleContract.Id, vehicleContract.VehicleType, vehicleContract.Balance));
                return Created(Request.Path, vehicleContract);
            }
            catch (Exception e)
            {
                return BadRequest(new ErrorResponseData(400, "Bad Request", e.Message));
            }
        }

        [HttpGet("{id}")]
        public ActionResult<Vehicle> GetVehicleById(string id)
        {
            if (!Vehicle.CheckIfIdIsValid(id)) return BadRequest(new ErrorResponseData(400, "Bad Request", "The vehicle Id is not valid"));
            var vehicle = parkingService.GetVehicles().FirstOrDefault(v => v.Id == id);
            if (vehicle == null) return NotFound(new ErrorResponseData(404, "Not Found", "There is no vehicle with this Id"));
            return Ok(vehicle);
        }

        [HttpDelete("{id}")]
        public ActionResult RemoveVehicle(string id)
        {
            if (!Vehicle.CheckIfIdIsValid(id)) return BadRequest(new ErrorResponseData(400, "Bad Request", "The vehicle Id is not valid"));
            try
            {
                parkingService.RemoveVehicle(id);
                return NoContent();
            }
            catch (ArgumentException e) { return NotFound(new ErrorResponseData(404, "Not Found", e.Message)); }
            catch (InvalidOperationException e) { return BadRequest(new ErrorResponseData(400, "Bad Request", e.Message)); }
        }
    }
}

