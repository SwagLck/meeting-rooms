using MeetingRooms.Api.Models;

namespace MeetingRooms.Api.Data
{
    public static class DbSeeder
    {
        public static void Seed(AppDbContext db)
        {
            if(db.Rooms.Any())
            {
                return;
            }

            var projector = new Equipment { Name = "Projector"};
            var tv = new Equipment { Name = "TV"};
            var whiteboard = new Equipment { Name = "Whiteboard"};
            var largeTable = new Equipment { Name = "Large Table"};

            db.Equipment.AddRange(projector, tv, whiteboard, largeTable);

            var rooms = new List<Room>
            {
                new Room { Name = "Forest Room", Capacity = 10, Floor = 1, IsAvailable = true, Equipment = new List<Equipment> { projector, whiteboard } },
                new Room { Name = "Ocean Room", Capacity = 20, Floor = 2, IsAvailable = true, Equipment = new List<Equipment> { tv, largeTable } },
                new Room { Name = "Mountain Room", Capacity = 15, Floor = 3, IsAvailable = true, Equipment = new List<Equipment> { projector, tv } },
                new Room { Name = "Garden Room", Capacity = 8, Floor = 4, IsAvailable = true, Equipment = new List<Equipment> { whiteboard } },
                new Room { Name = "Desert Room", Capacity = 12, Floor = 5, IsAvailable = false, Equipment = new List<Equipment> { largeTable } }
            };

            var users = new List<User>
            {
                new User { FirstName = "Jonas", LastName = "Jonaitis" },
                new User { FirstName = "Ona", LastName = "Onaite" },
                new User { FirstName = "Petras", LastName = "Petraitis" },
                new User { FirstName = "Audrius", LastName = "Audraitis" }
            };

            db.Users.AddRange(users);
            db.Rooms.AddRange(rooms);
            db.SaveChanges();

            var today = DateTime.Today;

            db.Reservations.AddRange(
                new Reservation { RoomId = rooms[0].Id, OrganizerId = users[0].Id, Title = "Team Meeting", Start = today.AddHours(9), End = today.AddHours(10), NumberOfParticipants = 5, Status = ReservationStatus.Booked },
                new Reservation { RoomId = rooms[1].Id, OrganizerId = users[1].Id, Title = "Client Presentation", Start = today.AddHours(11), End = today.AddHours(12), NumberOfParticipants = 10, Status = ReservationStatus.Completed },
                new Reservation { RoomId = rooms[0].Id, OrganizerId = users[0].Id, Title = "Project Kickoff", Start = today.AddHours(13), End = today.AddHours(14), NumberOfParticipants = 8, Status = ReservationStatus.Booked },
                new Reservation { RoomId = rooms[2].Id, OrganizerId = users[2].Id, Title = "Project Review", Start = today.AddHours(14), End = today.AddHours(15), NumberOfParticipants = 7, Status = ReservationStatus.Cancelled }
            );

            db.SaveChanges();
        }
    }
}