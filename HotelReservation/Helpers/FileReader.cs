using HotelReservation.Models;
using System.Text.Json;

namespace HotelReservation.Helpers
{
    public class FileReader
    {
        public static List<Hotel> ReadHotels(string filePath)
        {
            if (File.Exists(filePath))
            {
                return JsonSerializer.Deserialize<List<Hotel>>(File.ReadAllText(filePath)) ?? new List<Hotel>();
            }
            else
            {
                Console.WriteLine($"File not found: {filePath}");
                return new List<Hotel>();
            }
        }

        public static List<Booking> ReadBookings(string filePath)
        {
            if (File.Exists(filePath))
            {
                return JsonSerializer.Deserialize<List<Booking>>(File.ReadAllText(filePath)) ?? new List<Booking>();
            }
            else
            {
                Console.WriteLine($"File not found: {filePath}");
                return new List<Booking>();
            }
        }
    }
}
