using System;
using System.Text.Json.Serialization;

namespace HotelReservation.Models
{
    public class Booking
    {
        [JsonPropertyName("hotelId")]
        public string HotelId { get; set; }

        private string _arrival;
        [JsonPropertyName("arrival")]
        public string Arrival
        {
            get => _arrival;
            set
            {
                _arrival = value;
                if (DateTime.TryParseExact(_arrival, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out var parsedArrival))
                {
                    ArrivalDate = parsedArrival;
                }
                else
                {
                    Console.WriteLine($"Invalid arrival date: {_arrival}");
                    ArrivalDate = DateTime.MinValue;  // Set a default invalid date
                }
            }
        }

        private string _departure;
        [JsonPropertyName("departure")]
        public string Departure
        {
            get => _departure;
            set
            {
                _departure = value;
                if (DateTime.TryParseExact(_departure, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out var parsedDeparture))
                {
                    DepartureDate = parsedDeparture;
                }
                else
                {
                    Console.WriteLine($"Invalid departure date: {_departure}");
                    DepartureDate = DateTime.MinValue;  // Set a default invalid date
                }
            }
        }

        [JsonPropertyName("roomType")]
        public string RoomType { get; set; }

        [JsonPropertyName("roomRate")]
        public string RoomRate { get; set; }

        public DateTime ArrivalDate { get; private set; }
        public DateTime DepartureDate { get; private set; }
    }
}
