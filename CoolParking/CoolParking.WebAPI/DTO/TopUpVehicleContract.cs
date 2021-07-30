using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace CoolParking.WebAPI.DTO
{
    public class TopUpVehicleContract
    {
        [Required(ErrorMessage = "The vehicle Id is required field")]
        [DataType(DataType.Text)]
        [RegularExpression(@"^[A-Z]{2}-[0-9]{4}-[A-Z]{2}$", ErrorMessage = "The vehicle Id is not valid")]
        public string Id { get; set; }
        [Required(ErrorMessage = "The top up sum is required field")]
        [DataType(DataType.Currency)]
        [Range(1, 1000000, ErrorMessage = "The top up sum should be greater than or equal to one")]
        public decimal Sum { get; set; }
    }
}
