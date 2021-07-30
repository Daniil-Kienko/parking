using CoolParking.BL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CoolParking.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ParkingController : ControllerBase
    {
        private IParkingService parkingService;

        public ParkingController(IParkingService parkingService)
        {
            this.parkingService = parkingService;
        }

        [HttpGet]
        public ActionResult<decimal> Balance()
        {
            return Ok(parkingService.GetBalance());
        }

        [HttpGet]
        public ActionResult<int> Capacity()
        {
            return Ok(parkingService.GetCapacity());
        }

        [HttpGet]
        public ActionResult<int> FreePlaces()
        {
            return Ok(parkingService.GetFreePlaces());
        }
    }
}
