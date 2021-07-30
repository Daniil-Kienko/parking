using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Timers;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CoolParking.BL.Models;
using CoolParking.BL.Interfaces;

namespace CoolParking.BL.Services
{
    public class ParkingService : IParkingService
    {
        private Parking parkingObj = Parking.GetParking();
        private List<TransactionInfo> transactions = new List<TransactionInfo>();
        private readonly ITimerService withdrawalTimer;
        private readonly ITimerService loggingTimer;
        private readonly ILogService logService;
        private bool disposedValue;

        public ParkingService() : this(new TimerService(), new TimerService(), new LogService()) { }

        public ParkingService(ITimerService withdrawTimer, ITimerService loggingTimer, ILogService logService)
        {
            this.withdrawalTimer = withdrawTimer;
            this.loggingTimer = loggingTimer;
            this.logService = logService;
            this.SetTimerIntervals();
            this.RegisterEventHandlers();
            this.StartTimers();
        }

        /// <summary>
        /// Sets initial intervals for timers
        /// </summary>
        private void SetTimerIntervals()
        {
            this.withdrawalTimer.Interval = Settings.InitialPaymentPeriod;
            this.loggingTimer.Interval = Settings.InitialLogWritingPeriod;
        }

        /// <summary>
        /// Register initial event handlers
        /// </summary>
        private void RegisterEventHandlers()
        {
            this.withdrawalTimer.Elapsed += ProcessTransactions;
            this.loggingTimer.Elapsed += WriteToLog;
        }

        /// <summary>
        /// Start timers
        /// </summary>
        private void StartTimers()
        {
            this.withdrawalTimer.Start();
            this.loggingTimer.Start();
        }

        /// <summary>
        /// Stop timers
        /// </summary>
        private void StopTimers()
        {
            this.withdrawalTimer.Stop();
            this.loggingTimer.Stop();
        }

        /// <summary>
        /// Add a new car to the parking
        /// </summary>
        /// <param name="vehicle">The vehicle to be parked</param>
        public void AddVehicle(Vehicle vehicle)
        {
            if (GetFreePlaces() == 0) throw new InvalidOperationException("Sorry, we don't have a parking place for your car");
            Vehicle parkedVehicle = parkingObj.ParkedVehicles.Find(t => t.Id == vehicle.Id);
            if (parkedVehicle != null) throw new ArgumentException("Sorry, a vehicle with this Id is already parked");
            parkingObj.ParkedVehicles.Add(vehicle);
        }

        /// <summary>
        /// Add a new car to the parking
        /// </summary>
        /// <param name="id">Vehicle registration plate number</param>
        /// <param name="type">Vehicle type</param>
        /// <param name="balance">Vehicle balance</param>
        public void AddVehicle(string id, string type, decimal balance)
        {
            bool typeExists = Settings.InitialVehicleTypeToString.ContainsValue(type);
            if (!typeExists) throw new ArgumentException("You entered wrong vehicle type");
            VehicleType vehicleType = Settings.InitialVehicleTypeToString.FirstOrDefault(x => x.Value == type).Key;
            AddVehicle(new Vehicle(id, vehicleType, balance));
        }

        /// <summary>
        /// Get parking balance
        /// </summary>
        /// <returns>Parking balance</returns>
        public decimal GetBalance()
        {
            return parkingObj.Balance;
        }

        /// <summary>
        /// Get parking capacity
        /// </summary>
        /// <returns>Parking capacity</returns>
        public int GetCapacity()
        {
            return Settings.InitialParkingCapacity;
        }

        /// <summary>
        /// Get a dictionary of vehicle types with their string representation
        /// </summary>
        /// <returns>Dictionary of vehicle types with their string representation</returns>
        public Dictionary<VehicleType, string> GetVehicleTypes()
        {
            return Settings.InitialVehicleTypeToString;
        }

        /// <summary>
        /// Get free parking places
        /// </summary>
        /// <returns>Amount of free parking places</returns>
        public int GetFreePlaces()
        {
            return GetCapacity() - parkingObj.ParkedVehicles.Count;
        }

        /// <summary>
        /// Get last parking transactions
        /// </summary>
        /// <returns>Last parking transactions</returns>
        public TransactionInfo[] GetLastParkingTransactions()
        {
            return transactions.ToArray();
        }

        /// <summary>
        /// Get all parked vehicles
        /// </summary>
        /// <returns>All parked vehicles</returns>
        public ReadOnlyCollection<Vehicle> GetVehicles()
        {
            return parkingObj.ParkedVehicles.AsReadOnly();
        }

        /// <summary>
        /// Get transactions stored in the log
        /// </summary>
        /// <returns>Transactions stored in the log</returns>
        public string ReadFromLog()
        {
            return logService.Read();
        }

        /// <summary>
        /// Writes transactions stored in program to log file and delete these transactions from program memory
        /// </summary>
        private void WriteToLog(object sender, ElapsedEventArgs e)
        {
            StopTimers();
            StringBuilder sb = new StringBuilder();
            for(int i = 0; i < transactions.Count; i++)
            {
                var transaction = transactions[i];
                sb.Append($"{transaction.TransactionDateAndTime.ToLocalTime(),19} {transaction.VehicleId,10} {transaction.Sum,9}");
                if (i != transactions.Count - 1) sb.Append("\n");
            }
            logService.Write(sb.ToString());
            transactions.Clear();
            StartTimers();
        }

        /// <summary>
        /// Create new transaction for every parked vehicle
        /// </summary>
        private void ProcessTransactions(object sender, ElapsedEventArgs e)
        {
            for (int i = 0; i < parkingObj.ParkedVehicles.Count; i++)
            {
                Vehicle vehicle = parkingObj.ParkedVehicles[i];
                decimal parkingPrice = CalculateCarParkingPrice(vehicle);
                vehicle.Balance -= parkingPrice;
                parkingObj.Balance += parkingPrice;
                transactions.Add(new TransactionInfo(vehicle.Id, parkingPrice));
            }
        }

        /// <summary>
        /// Get the amount of money to be withdrawn from the client's vehicle
        /// </summary>
        /// <param name="vehicle">Vehicle for calculation</param>
        /// <returns>Amount of money to be withdrawn from the client's vehicle</returns>
        private decimal CalculateCarParkingPrice(Vehicle vehicle)
        {
            decimal initialCarParkingPrice = Settings.InitialPrices[vehicle.VehicleType];
            decimal finalCarParkingPrice;
            if (vehicle.Balance <= 0) finalCarParkingPrice = initialCarParkingPrice * Settings.InitialPenaltyFactor;
            else if (vehicle.Balance > 0 && vehicle.Balance < initialCarParkingPrice)
            {
                decimal diff = initialCarParkingPrice - vehicle.Balance;
                finalCarParkingPrice = vehicle.Balance + diff * Settings.InitialPenaltyFactor;
            }
            else finalCarParkingPrice = initialCarParkingPrice;
            return finalCarParkingPrice;
        }

        /// <summary>
        /// Remove vehicle from parking
        /// </summary>
        /// <param name="vehicleId">Licence plate number of vehicle we should remove</param>
        public void RemoveVehicle(string vehicleId)
        {
            Vehicle vehicle = parkingObj.ParkedVehicles.Find(t => t.Id == vehicleId);
            if (vehicle == null) throw new ArgumentException("Sorry, we don't have a parked vehicle with this ID");
            else if (vehicle.Balance < 0) throw new InvalidOperationException("Please, pay off the debt in order to pick up the vehicle");
            parkingObj.ParkedVehicles.Remove(vehicle);
        }

        /// <summary>
        /// Top up vehicle balance
        /// </summary>
        /// <param name="vehicleId">Licence plate number of vehicle which balance we should top up</param>
        /// <param name="sum">Sum to top up</param>
        public void TopUpVehicle(string vehicleId, decimal sum)
        {
            if (sum <= 0) throw new ArgumentException("Sorry, you need to invest at least 0.1$");
            Vehicle vehicle = parkingObj.ParkedVehicles.Find(t => t.Id == vehicleId);
            if (vehicle == null) throw new ArgumentException("Sorry, we don't have a parked vehicle with this ID");
            vehicle.Balance += sum;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // Dispose managed state (managed objects)
                    withdrawalTimer.Dispose();
                    loggingTimer.Dispose();
                }

                // Free unmanaged resources (unmanaged objects) and override finalizer
                if (File.Exists(logService.LogPath)) File.Delete(logService.LogPath);
                // Set large fields to null
                parkingObj.ParkedVehicles.Clear();
                parkingObj.Balance = 0;
                transactions.Clear();

                disposedValue = true;
            }
        }

        ~ParkingService()
        {
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            System.GC.SuppressFinalize(this);
        }
    }
}