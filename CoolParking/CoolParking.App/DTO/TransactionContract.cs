using System;

namespace CoolParking.App.DTO
{
    public class TransactionContract
    {
        public DateTime TransactionDateAndTime { get; set; }
        public string VehicleId { get; set; }
        public decimal Sum { get; set; }
    }

}
