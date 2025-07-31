using Airport_Tickets_System.states;

namespace Airport_Tickets_System.models;

public record Bookings(
    string Id,
    string PassengerId,
    BookingState State,
    DateTime RegistrationDate
);