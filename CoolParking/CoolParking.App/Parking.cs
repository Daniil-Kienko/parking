using System;
using System.Net.Http;
using System.Text;
using CoolParking.App.DTO;
using CoolParking.App.Extensions;
using CoolParking.App.Services;
using static CoolParking.App.Helpers.ConsoleInteractionService;

namespace CoolParking.App
{
    class Parking
    {
        static void Main(string[] args)
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            using (HttpService client = new HttpService(clientHandler)) {
                GreetUser();
                ParkWorking(client);
                GoodByeUser();
            }
        }

        private static void GreetUser()
        {
            Console.WriteLine("*** We are glad to welcome you in the \"Cool Parking\" ***");
        }

        private static void GoodByeUser()
        {
            Console.WriteLine("Goodbye!");
        }

        private static void PrintMenu()
        {
            Console.WriteLine("Menu:");
            Console.WriteLine("1. Display current Parking balance");
            Console.WriteLine("2. Display Parking capacity");
            Console.WriteLine("3. Display the number of free and occupied parking places");
            Console.WriteLine("4. Display Parking Transactions for the current period");
            Console.WriteLine("5. Display Parking Transactions for all periods");
            Console.WriteLine("6. Display parked vehicles");
            Console.WriteLine("7. Park your vehicle");
            Console.WriteLine("8. Pick up vehicle");
            Console.WriteLine("9. Get and Display vehicle by Id");
            Console.WriteLine("10.Top up vehicle balance");
            Console.WriteLine("0. Exit");
        }

        private static void ParkWorking(HttpService clientHandler)
        {
            while (true)
            {
                PrintMenu();
                int choice = GetIntInputInRange("Which operation would you like to perform: ", 0, 10);
                if (choice == 0) break;
                Action<HttpService> func = GetAction(choice);
                func(clientHandler);
            }
        }
        
        private static Action<HttpService> GetAction(int choice)
        {
            return choice switch
            {
                1 => PrintCurrentParkingBalance,
                2 => PrintParkingCapacity,
                3 => PrintFreeAndOccupiedParkingPlaces,
                4 => PrintCurrentPeriodTransactions,
                5 => PrintFullTransactionsHistory,
                6 => PrintParkedVehicles,
                7 => ParkVehicle,
                8 => PickUpVehicle,
                9 => GetVehicleById,
                10 => TopUpVehicleBalance,
                _ => throw new ArgumentException("Your choice is not valid, please try again")
            };
        }

        private static void PrintCurrentParkingBalance(HttpService client)
        {
            try
            {
                string balance = client.GetData("parking/balance");
                Console.WriteLine($"Current Parking balance: {balance}");
            }
            catch (Exception e)
            {
                WriteError($"Error: {e.Message}");
            }
        }

        private static void PrintParkingCapacity(HttpService client)
        {
            try
            {
                string capacity = client.GetData("parking/capacity");
                Console.WriteLine($"Parking has {capacity} parking places");
            }
            catch (Exception e)
            {
                WriteError($"Error: {e.Message}");
            }
        }

        private static void PrintFreeAndOccupiedParkingPlaces(HttpService client)
        {
            try
            {
                string availablePlaces = client.GetData("parking/freePlaces");
                string parkingCapacity = client.GetData("parking/capacity");
                Console.WriteLine($"{availablePlaces} out of {parkingCapacity} parking places are empty");
            }
            catch (Exception e)
            {
                WriteError($"Error: {e.Message}");
            }
        }

        private static void PrintCurrentPeriodTransactions(HttpService client)
        {
            try
            {
                var transactionsArr = client.GetDataDeserialized<TransactionContract[]>("transactions/last");
                Console.WriteLine("All parking transactions for current period: ");
                if (transactionsArr.Length == 0)
                    Console.WriteLine("There were no transactions during this period");
                else
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append($"{"Date and Time",16} {"Vehicle Id",13} {"Transaction Sum",15}\n");
                    foreach (var transaction in transactionsArr)
                        sb.Append($"{transaction.TransactionDateAndTime.ToLocalTime(),19} {transaction.VehicleId,10} {transaction.Sum,9}\n");
                    Console.WriteLine(sb);
                }
            }
            catch (Exception e)
            {
                WriteError($"Error: {e.Message}");
            }
        }

        private static void PrintFullTransactionsHistory(HttpService client)
        {
            try
            {
                PrintCurrentPeriodTransactions(client);
                var transactionsString = client.GetData("transactions/all");
                Console.WriteLine("All parking transactions for the previous periods: ");
                if (transactionsString.Length == 0) Console.WriteLine("There were no transactions during the previous periods");
                else
                {
                    Console.WriteLine($"{"Date and Time",16} {"Vehicle Id",13} {"Transaction Sum",15}");
                    Console.WriteLine(transactionsString.Trim());
                }
            }
            catch (Exception e)
            {
                WriteError($"Error: {e.Message}");
            }
        }

        private static void PrintParkedVehicles(HttpService client)
        {
            try
            {
                var vehiclesArr = client.GetDataDeserialized<VehicleContract[]>("vehicles");
                if (vehiclesArr.Length == 0) Console.WriteLine("The Parking is empty");
                else
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append($"{"Vehicle type",13} {"Vehicle Id",10} {"Vehicle balance",10}\n");
                    foreach (var vehicle in vehiclesArr)
                    {
                        sb.Append($"{Settings.VehicleTypeToStringNames[vehicle.VehicleType],10} {vehicle.Id,12} {vehicle.Balance,8}\n");
                    }
                    Console.WriteLine(sb);
                }
            }
            catch (Exception e)
            {
                WriteError($"Error: {e.Message}");
            }
        }

        private static void ParkVehicle(HttpService client)
        {
            try
            {
                string id = GetStringInput("Enter vehicle Id: ");
                string typeQuestion = "Enter vehicle type".InsertArrayWithNumeration(Settings.VehicleTypeToStringNames);
                int vehicleType = GetIntInputInRange(typeQuestion, 1, 4) - 1;
                decimal balance = GetDecimalInput("Enter vehicle balance: ");
                client.PostData("vehicles", new { id, vehicleType, balance });
                WriteSuccess("Vehicle have been successfully parked");
            }
            catch (Exception e)
            {
                WriteError($"Error: {e.Message}");
            }
        }

        private static void PickUpVehicle(HttpService client)
        {
            try
            {
                string id = GetStringInput("Enter vehicle Id: ");
                client.DeleteData("vehicles/" + id);
                WriteSuccess("Vehicle have been successfully picked up");
            }
            catch (Exception e)
            {
                WriteError($"Error: {e.Message}");
            }
        }

        private static void GetVehicleById(HttpService client)
        {
            try
            {
                string id = GetStringInput("Enter vehicle Id: ");
                var vehicle = client.GetDataDeserialized<VehicleContract>($"vehicles/{id}");
                PrintVehicle(vehicle);
            }
            catch (Exception e)
            {
                WriteError($"Error: {e.Message}");
            }
        }

        private static void TopUpVehicleBalance(HttpService client)
        {
            try
            {
                Console.Write("Enter vehicle Id: ");
                string id = GetStringInput("Enter vehicle Id: ");
                decimal balance = GetDecimalInput("Enter the amount to top up: ");
                var vehicle = client.PutDataAndDeserializeResponse<VehicleContract>("transactions/topUpVehicle", new { id, sum = balance });
                PrintVehicle(vehicle);
            }
            catch (Exception e)
            {
                WriteError($"Error: {e.Message}");
            }
        }

        private static void PrintVehicle(VehicleContract vehicle)
        {
            Console.WriteLine($"{"Vehicle type",10} {"Vehicle Id",10} {"Vehicle balance",8}");
            Console.WriteLine($"{Settings.VehicleTypeToStringNames[vehicle.VehicleType],10} {vehicle.Id,12} {vehicle.Balance,8}");
        }
    }
}
