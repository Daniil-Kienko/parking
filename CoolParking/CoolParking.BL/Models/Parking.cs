using System.Collections.Generic;
namespace CoolParking.BL.Models
{
    public class Parking
    {
        private static Parking instance;

        public decimal Balance { get; set; }
        public List<Vehicle> ParkedVehicles { get; set; }

        private Parking()
        {
            this.Balance = Settings.InitialParkingBalance;
            ParkedVehicles = new List<Vehicle>();
        }

        /// <summary>
        /// Get a single instance of the Parking class
        /// </summary>
        /// <returns>Instance of Parking class</returns>
        public static Parking GetParking()
        {
            if (instance == null)
                instance = new Parking();
            return instance;
        }
    }
}