using HotelReservation.BLL.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelReservation.Helpers
{
    public static class CommandHelper
    {
        public static (string,DateTime, DateTime, string) GetAvailabilityCommandArgs(string line)
        {

            var inside = line.Substring("Availability".Length).Trim('(', ')');
            var parts = inside.Split(',');
            var hotelId = parts[0].Trim();
            var datePart = parts[1].Trim();
            var roomType = parts[2].Trim();

            DateTime startDate, endDate;

            if (datePart.Contains("-"))
            {
                var dates = datePart.Split('-');
                DateTime.TryParseExact(dates[0].Trim(), "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out startDate);
                DateTime.TryParseExact(dates[1].Trim(), "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out endDate);
            }
            else
            {
                DateTime.TryParseExact(datePart, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out startDate);
                endDate = startDate;
            }

            return (hotelId, startDate, endDate, roomType);
        }

        public static (string, DateTime, DateTime, int) GetRoomTypesCommandArgs(string line)
        {
            var inside = line.Substring("RoomTypes".Length).Trim('(', ')');
            var parts = inside.Split(',');
            var hotelId = parts[0].Trim();
            var datePart = parts[1].Trim();
            var people = int.Parse(parts[2].Trim());

            DateTime startDate, endDate;

            if (datePart.Contains("-"))
            {
                var dates = datePart.Split('-');
                DateTime.TryParseExact(dates[0].Trim(), "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out startDate);
                DateTime.TryParseExact(dates[1].Trim(), "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out endDate);
            }
            else
            {
                DateTime.TryParseExact(datePart, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out startDate);
                endDate = startDate;
            }

            return (hotelId, startDate, endDate, people);
        }
    }
}
