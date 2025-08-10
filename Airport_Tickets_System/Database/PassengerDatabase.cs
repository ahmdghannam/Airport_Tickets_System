using Airport_Tickets_System.models;
using Airport_Tickets_System.Utils;

namespace Airport_Tickets_System.Database;

public class PassengerDatabase
{
    private readonly string _passengersFilePath =
        Path.Combine(Functionalities.GetProjectDirectory(), "DataFiles", Consts.Files.Passengers);


    public Passenger? GetPassengerByUserName(string userName)
    {
        if (string.IsNullOrWhiteSpace(userName))
            return null;

        if (!File.Exists(_passengersFilePath))
            throw new FileNotFoundException("Passengers file not found.", _passengersFilePath);

        var lines = File.ReadAllLines(_passengersFilePath);

        return (from line
                in lines.Skip(1)
            select line.Split(',')
            into columns
            let passengerUsername =
                columns[2].Trim()
            where
                passengerUsername.Equals(userName, StringComparison.OrdinalIgnoreCase)
            select new Passenger
            {
                Id = int.Parse(columns[0]
                ),
                Username = columns[1], FullName = columns[2], PhoneNumber = columns[3]
            }).FirstOrDefault();
    }
}