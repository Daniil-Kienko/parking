using System;
using System.ComponentModel.DataAnnotations;
using CoolParking.BL.Models;
using Newtonsoft.Json;

namespace CoolParking.WebAPI.DTO
{
    public class CreateVehicleContract
    {
        [Required(ErrorMessage ="The vehicle Id is required field")]
        [DataType(DataType.Text)]
        [RegularExpression(@"^[A-Z]{2}-[0-9]{4}-[A-Z]{2}$", ErrorMessage = "The vehicle Id is not valid")]
        public string Id { get; set; }
        [Required(ErrorMessage = "The vehicle type is required field")]
        [EnumDataType(typeof(VehicleType))]
        public VehicleType VehicleType { get; set; }
        [Required(ErrorMessage = "The vehicle balance is required field")]
        [DataType(DataType.Currency)]
        [Range(0, 1000000, ErrorMessage = "Vehicle balance should be greater than or equal to zero")]
        public decimal Balance { get; set; }
    }
}
