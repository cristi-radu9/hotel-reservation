using HotelReservation.BLL.Managers;
using HotelReservation.Models;
using System.Text.Json;

namespace HotelReservationTests
{
    public class BookingManagerTests
    {
        private List<Hotel> _hotels;
        private List<Booking> _bookings;
        private BookingManager _manager;

        [SetUp]
        public void Setup()
        {
            var hotelJson = @"
            [
                {
                    ""id"": ""H1"",
                    ""name"": ""Hotel California"",
                    ""roomTypes"": [
                        { ""code"": ""SGL"", ""size"": 1, ""description"": ""Single Room"", ""amenities"": [""WiFi"", ""TV""], ""features"": [""Non-smoking""] },
                        { ""code"": ""DBL"", ""size"": 2, ""description"": ""Double Room"", ""amenities"": [""WiFi"", ""TV"", ""Minibar""], ""features"": [""Non-smoking"", ""Sea View""] }
                    ],
                    ""rooms"": [
                        { ""roomType"": ""SGL"", ""roomId"": ""101"" },
                        { ""roomType"": ""SGL"", ""roomId"": ""102"" },
                        { ""roomType"": ""DBL"", ""roomId"": ""201"" },
                        { ""roomType"": ""DBL"", ""roomId"": ""202"" }
                    ]
                }
            ]";

            var bookingsJson = @"
            [
                { ""hotelId"": ""H1"", ""arrival"": ""20240901"", ""departure"": ""20240903"", ""roomType"": ""DBL"", ""roomRate"": ""Prepaid"" },
                { ""hotelId"": ""H1"", ""arrival"": ""20240902"", ""departure"": ""20240905"", ""roomType"": ""SGL"", ""roomRate"": ""Standard"" },
                { ""hotelId"": ""H1"", ""arrival"": ""20240901"", ""departure"": ""20240905"", ""roomType"": ""SGL"", ""roomRate"": ""Standard"" }
            ]";

            _hotels = JsonSerializer.Deserialize<List<Hotel>>(hotelJson) ?? new List<Hotel>();
            _bookings = JsonSerializer.Deserialize<List<Booking>>(bookingsJson) ?? new List<Booking>();

            _manager = new BookingManager(_hotels, _bookings);
        }

        // ---------- CheckAvailability ----------

        [Test]
        public void CheckAvailability_HotelNotFound_ReturnsZero()
        {
            var available = _manager.CheckAvailability("Invalid", new(2024, 9, 1), new(2024, 9, 3), "SGL");
            Assert.That(available, Is.EqualTo(0));
        }

        [Test]
        public void CheckAvailability_BookingOverlaps_ReturnsCorrect()
        {
            var available = _manager.CheckAvailability("H1", new(2024, 9, 1), new(2024, 9, 3), "DBL");
            Assert.That(available, Is.EqualTo(1)); // 2 total, 1 booked
        }

        [Test]
        public void CheckAvailability_EndDateNull_BookingStillOverlaps()
        {
            var available = _manager.CheckAvailability("H1", new(2024, 9, 2), null, "SGL");
            Assert.That(available, Is.EqualTo(0)); // 2 total, 2 booked
        }

        [Test]
        public void CheckAvailability_NoOverlap_ReturnsAll()
        {
            var available = _manager.CheckAvailability("H1", new(2024, 9, 10), new(2024, 9, 12), "SGL");
            Assert.That(available, Is.EqualTo(2));
        }

        // ---------- GetOptimalRoomAllocation ----------

        [Test]
        public void GetOptimalRoomAllocation_HotelNotFound_ReturnsEmpty()
        {
            var result = _manager.GetOptimalRoomAllocation("Invalid", new(2024, 9, 1), new(2024, 9, 3), 2);
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void GetOptimalRoomAllocation_ExactFit_ReturnsExpectedRooms()
        {
            var result = _manager.GetOptimalRoomAllocation("H1", new(2024, 9, 10), new(2024, 9, 11), 3);
            Assert.That(result, Is.EqualTo(new List<string> { "DBL", "SGL" }));
        }

        [Test]
        public void GetOptimalRoomAllocation_PartialRoom_AppendsExclamation()
        {
            var result = _manager.GetOptimalRoomAllocation("H1", new(2024, 9, 2), new(2024, 9, 10), 1);
            Assert.That(result, Is.EqualTo(new List<string> { "DBL!" }));
        }

        [Test]
        public void GetOptimalRoomAllocation_EndDateNull_WorksCorrectly()
        {
            var result = _manager.GetOptimalRoomAllocation("H1", new(2024, 9, 10), null, 2);
            Assert.That(result.Count, Is.EqualTo(1));
        }

        [Test]
        public void GetOptimalRoomAllocation_NotEnoughCapacity_ReturnsEmpty()
        {
            var result = _manager.GetOptimalRoomAllocation("H1", new(2024, 9, 10), new(2024, 9, 11), 10);
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void GetOptimalRoomAllocation_AllRoomsBooked_ReturnsEmpty()
        {
            var hotelJson = @"
            [
                {
                    ""id"": ""H1"",
                    ""name"": ""Hotel California"",
                    ""roomTypes"": [
                        { ""code"": ""SGL"", ""size"": 1, ""description"": ""Single Room"", ""amenities"": [""WiFi"", ""TV""], ""features"": [""Non-smoking""] },
                        { ""code"": ""DBL"", ""size"": 2, ""description"": ""Double Room"", ""amenities"": [""WiFi"", ""TV"", ""Minibar""], ""features"": [""Non-smoking"", ""Sea View""] }
                    ],
                    ""rooms"": [
                        { ""roomType"": ""SGL"", ""roomId"": ""101"" },
                        { ""roomType"": ""SGL"", ""roomId"": ""102"" },
                        { ""roomType"": ""DBL"", ""roomId"": ""201"" },
                        { ""roomType"": ""DBL"", ""roomId"": ""202"" }
                    ]
                }
            ]";

            var bookingsJson = @"
            [
                { ""hotelId"": ""H1"", ""arrival"": ""20240901"", ""departure"": ""20240904"", ""roomType"": ""DBL"", ""roomRate"": ""Prepaid"" },
                { ""hotelId"": ""H1"", ""arrival"": ""20240901"", ""departure"": ""20240903"", ""roomType"": ""DBL"", ""roomRate"": ""Prepaid"" },
                { ""hotelId"": ""H1"", ""arrival"": ""20240902"", ""departure"": ""20240905"", ""roomType"": ""SGL"", ""roomRate"": ""Standard"" },
                { ""hotelId"": ""H1"", ""arrival"": ""20240901"", ""departure"": ""20240905"", ""roomType"": ""SGL"", ""roomRate"": ""Standard"" }
            ]";

            _hotels = JsonSerializer.Deserialize<List<Hotel>>(hotelJson) ?? new List<Hotel>();
            _bookings = JsonSerializer.Deserialize<List<Booking>>(bookingsJson) ?? new List<Booking>();

            _manager = new BookingManager(_hotels, _bookings);

            var result = _manager.GetOptimalRoomAllocation("H1", new(2024, 9, 2), new(2024, 9, 3), 2);
            Assert.That(result, Is.Empty); // All DBL and one SGL are booked
        }

        [Test]
        public void GetOptimalRoomAllocation_GuestsZero_ReturnsEmpty()
        {
            var result = _manager.GetOptimalRoomAllocation("H1", new(2024, 9, 10), new(2024, 9, 11), 0);
            Assert.That(result, Is.Empty);
        }

    }
}
