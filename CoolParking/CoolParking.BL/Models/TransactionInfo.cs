using System;

namespace CoolParking.BL.Models
{
    public class TransactionInfo
    {
        public DateTime TransactionDateAndTime { get; set; }
        public string VehicleId { get; set; }
        public decimal Sum { get; set; }

        public TransactionInfo(string VehicleId, decimal Sum)
        {
            this.TransactionDateAndTime = DateTime.Now;
            this.VehicleId = VehicleId;
            this.Sum = Sum;
        }
    }
}