using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace CoolParking.BL.Models
{
    static class Settings
    {
        public static decimal InitialParkingBalance = 0;
        public static int InitialParkingCapacity = 10;
        public static double InitialPaymentPeriod = 5;
        public static double InitialLogWritingPeriod = 60;
        public static string InitialLogWritingPath = $@"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\Transactions.log";
        public static Dictionary<VehicleType, decimal> InitialPrices = new Dictionary<VehicleType, decimal>() {
            {VehicleType.PassengerCar, 2 },
            {VehicleType.Truck, 5 },
            {VehicleType.Bus, 3.5m },
            {VehicleType.Motorcycle, 1 },
        };
        public static Dictionary<VehicleType, string> InitialVehicleTypeToString = new Dictionary<VehicleType, string>() {
            {VehicleType.PassengerCar, "Passenger car" },
            {VehicleType.Truck, "Truck"  },
            {VehicleType.Bus, "Bus"  },
            {VehicleType.Motorcycle, "Motorcycle"  },
        };
        public static decimal InitialPenaltyFactor = 2.5m;
    }
}