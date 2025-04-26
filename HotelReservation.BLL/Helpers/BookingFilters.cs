using HotelReservation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelReservation.BLL.Helpers
{
    public static class BookingFilters
    {
        public static Func<Booking, bool> GetEndDateFilter(DateTime? endDate)
        {
            if (endDate.HasValue)
                return b => endDate > b.ArrivalDate;

            return b => true;
        }
    }
}
