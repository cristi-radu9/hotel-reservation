using HotelReservation.BLL.Helpers;
using HotelReservation.Models;
using System.Linq;
using System.Linq.Expressions;

namespace HotelReservation.BLL.Managers
{
    public class BookingManager
    {
        private readonly List<Hotel> _hotels;
        private readonly List<Booking> _bookings;

        public BookingManager(List<Hotel> hotels, List<Booking> bookings)
        {
            _hotels = hotels;
            _bookings = bookings;
        }

        public int CheckAvailability(string hotelId, DateTime startDate, DateTime? endDate, string roomType)
        {
            var endDateFilter = BookingFilters.GetEndDateFilter(endDate);

            var hotel = _hotels.FirstOrDefault(h => h.Id == hotelId);
            if (hotel == null) return 0;

            var totalRooms = hotel.Rooms.Count(r => r.RoomType == roomType);

            var overlappingBookings = _bookings
                .Where(b => b.HotelId == hotelId && b.RoomType == roomType)
                .Where(b => startDate < b.DepartureDate)
                .Where(endDateFilter)
                .Count();

            return totalRooms - overlappingBookings;
        }


        public List<string> GetOptimalRoomAllocation(string hotelId, DateTime startDate, DateTime? endDate, int guests)
        {
            var endDateFilter = BookingFilters.GetEndDateFilter(endDate);

            var hotel = _hotels.FirstOrDefault(h => h.Id == hotelId);
            if (hotel == null) return new List<string>();

            var bookingThatOverlaps = _bookings
                        .Where(b => b.HotelId == hotelId)
                        .Where(b => startDate < b.DepartureDate)
                        .Where(endDateFilter).ToList();

            foreach(var overlap in bookingThatOverlaps)
            {
                var occupiedRoom=hotel.Rooms.Where(x=>x.RoomType == overlap.RoomType).FirstOrDefault();
                if(occupiedRoom != null)
                {
                    hotel.Rooms.Remove(occupiedRoom);
                }
            }

            var availableRooms = hotel.Rooms
                .Select(r => new
                {
                    Room = r,
                    Type = hotel.RoomTypes.First(rt => rt.Code == r.RoomType)
                })
                .ToList();

            var result = new List<string>();

            // Try to fit the guests as exactly as possible
            availableRooms = availableRooms.OrderBy(x => x.Type.Size).ToList();

            while (guests > 0)
            {
                var bestFit = availableRooms
                    .Where(r => r.Type.Size <= guests)
                    .OrderByDescending(r => r.Type.Size)
                    .FirstOrDefault();

                if (bestFit != null)
                {
                    result.Add(bestFit.Type.Code);
                    guests -= bestFit.Type.Size;
                    availableRooms.Remove(bestFit);
                }
                else
                {
                    // Use smallest available room (may overshoot, mark with "!")
                    var fallback = availableRooms.FirstOrDefault();
                    if (fallback == null)
                        return new List<string>(); // not enough rooms

                    result.Add(fallback.Type.Code + "!");
                    guests = 0;
                }
            }

            return guests > 0 ? new List<string>() : result;
        }


    }
}
