using HotelReservation.BLL.Managers;
using HotelReservation.Helpers;
using System.CommandLine;
using System.Text.Json;

var hotelsOption = new Option<string>("--hotels", "The path to the hotels JSON file") { IsRequired = true };
var bookingsOption = new Option<string>("--bookings", "The path to the bookings JSON file") { IsRequired = true };

var rootCommand = new RootCommand("Hotel reservation CLI");
rootCommand.AddOption(hotelsOption);
rootCommand.AddOption(bookingsOption);

rootCommand.SetHandler((string hotels, string bookings) =>
{
    Console.WriteLine($"Hotels: {hotels}");
    Console.WriteLine($"Bookings: {bookings}");
    var hotelsObj = FileReader.ReadHotels(hotels);
    var bookingObj = FileReader.ReadBookings(bookings);
    Console.WriteLine($"Hotels: {JsonSerializer.Serialize(hotelsObj)}");
    Console.WriteLine($"Bookings: {JsonSerializer.Serialize(bookingObj)}");
    Console.WriteLine("________________________");
    Console.WriteLine("Enter Commands:");
    var bookinManager=new BookingManager(hotelsObj, bookingObj);
    string line;
    while (!string.IsNullOrWhiteSpace(line = Console.ReadLine() ?? ""))
    {
        if (line.StartsWith("Availability"))
        {
            var commandArgs= CommandHelper.GetAvailabilityCommandArgs(line);
            var availability = bookinManager.CheckAvailability(commandArgs.Item1, commandArgs.Item2, commandArgs.Item3, commandArgs.Item4);
            Console.WriteLine(availability);
        }
        else if (line.StartsWith("RoomTypes"))
        {
            var commandArgs = CommandHelper.GetRoomTypesCommandArgs(line);
            var allocation = bookinManager.GetOptimalRoomAllocation(commandArgs.Item1, commandArgs.Item2, commandArgs.Item3, commandArgs.Item4);
            if (allocation == null)
            {
                Console.WriteLine("Error: Not enough room availability.");
            }
            else
            {
                Console.WriteLine($"{commandArgs.Item1}: {string.Join(", ", allocation)}");
            }
        }
        else
        {
            Console.WriteLine("Unknown command");
        }
    }


}, hotelsOption, bookingsOption);

return await rootCommand.InvokeAsync(args);
