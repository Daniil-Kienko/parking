using System;
using System.Text.RegularExpressions;
using CoolParking.BL.Extensions;


namespace CoolParking.BL.Models
{
    public class Vehicle
    {
        public string Id { get; }
        public VehicleType VehicleType { get; }
        public decimal Balance { get; internal set; }

        public Vehicle(string Id, VehicleType VehicleType, decimal Balance)
        {
            if (!CheckIfIdIsValid(Id)) throw new ArgumentException("Your car have incorrect Id");
            else if (Balance < 0) throw new ArgumentException("Your car can't have negative balance");
            this.Id = Id;
            this.VehicleType = VehicleType;
            this.Balance = Balance;
        }

        /// <summary>
        /// Returns a string representation of the vehicle type
        /// </summary>
        /// <returns>String representation of the vehicle type</returns>
        public string GetStringifiedType()
        {
            return Settings.InitialVehicleTypeToString[VehicleType];
        }

        /// <summary>
        /// Generates a random registration plate number
        /// </summary>
        /// <returns>Random registration plate number</returns>
        public static string GenerateRandomRegistrationPlateNumber()
        {
            string temp = "";
            temp += temp.GenerateRandomUppercaseLetters(2);
            temp += "-";
            temp += temp.GenerateRandomInts(4);
            temp += "-";
            temp += temp.GenerateRandomUppercaseLetters(2);
            return temp;
        }

        /// <summary>
        /// Check the validity of a car's license plate
        /// </summary>
        /// <param name="Id">Car's license plate</param>
        /// <returns>True if number is valid, otherwise false</returns>
        public static bool CheckIfIdIsValid(string Id)
        {
            string pattern = @"^[A-Z]{2}-[0-9]{4}-[A-Z]{2}$";
            return Regex.IsMatch(Id, pattern);
        }
    }
}