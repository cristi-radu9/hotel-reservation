# hotel-reservation

## Description
This is a console application built with .NET 8 that manages hotel room availability and reservations.  
The application reads hotel and booking data from JSON files and allows users to:
- Check room availability for a specific hotel, date range, and room type.
- Find a combination of rooms needed to accommodate a specified number of people over a date range.

---

## Requirements
- .NET 8 SDK (https://dotnet.microsoft.com/en-us/download/dotnet/8.0)

---

## How to Run the Project

1. Clone the repository:
```bash
git clone https://github.com/cristi-radu9/hotel-reservation.git
cd hotel-reservation
```

2. Build and run the application:
dotnet build
dotnet run -- --hotels hotels.json --bookings bookings.json

3. Example console commands after running the app:
Availability(H1, 20240901, SGL)
Availability(H1, 20240901-20240903, DBL)
RoomTypes(H1, 20240904, 3)
RoomTypes(H1, 20240905-20240907, 5)


4. To run the unit tests:
```bash
dotnet test
```
