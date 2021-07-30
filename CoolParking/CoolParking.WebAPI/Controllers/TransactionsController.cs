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
    [Route("api/[controller]/[action]")]
    public class TransactionsController : ControllerBase
    {
        private IParkingService parkingService;

        public TransactionsController(IParkingService parkingService)
        {
            this.parkingService = parkingService;
        }

        [HttpGet]
        public ActionResult<TransactionInfo[]> Last()
        {
            return Ok(parkingService.GetLastParkingTransactions());
        }

        [HttpGet]
        public ActionResult<string> All()
        {
            try { return Ok(parkingService.ReadFromLog().Trim()); }
            catch (Exception e) { return NotFound(new ErrorResponseData(404, "Not Found", "There is no logged transactions yet")); }
        }

        [HttpPut]
        public ActionResult<Vehicle> TopUpVehicle([FromBody] TopUpVehicleContract vehicleContract)
        {
            if (!ModelState.IsValid)
            {
                string messages = string.Join("; ", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));
                return BadRequest(new ErrorResponseData(400, "Bad Request", messages));
            }
            Vehicle vehicle = parkingService.GetVehicles().FirstOrDefault(x => x.Id == vehicleContract.Id);
            if (vehicle == null) return NotFound(new ErrorResponseData(404, "Not Found", "There is no vehicle with this Id"));
            parkingService.TopUpVehicle(vehicleContract.Id, vehicleContract.Sum);
            return Ok(vehicle);
        }
    }
}