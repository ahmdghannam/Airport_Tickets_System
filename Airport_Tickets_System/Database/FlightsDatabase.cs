using Airport_Tickets_System.models;
using static Airport_Tickets_System.Utils.Functionalities;
using static Airport_Tickets_System.Utils.Consts;

namespace Airport_Tickets_System.Database;

public class FlightsDatabase
{
    private readonly string _flightsFilePath = Path.Combine(GetProjectDirectory(), "DataFiles", Files.Flights);

    public bool InsertFlightsData(string[] flights)
    {
        var idGenerator = GetLastFlightId();
        try
        {
            var rowsWithIds = flights
                .Skip(1)
                .Where(line => !string.IsNullOrWhiteSpace(line))
                .Select(line => ++idGenerator + "," + line);
            
            File.AppendAllLines(_flightsFilePath, rowsWithIds);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error writing to file: " + ex.Message);
            return false;
        }
    }

    private int GetLastFlightId()
    {
        try
        {
            var lines = File.ReadAllLines(_flightsFilePath)
                .Where(line => !string.IsNullOrWhiteSpace(line))
                .ToArray();

            if (lines.Length <= 1)
                return 0; // No flight data (only header exists)

            var lastLine = lines.Last();
            var fields = lastLine.Split(',');

            if (int.TryParse(fields[0], out int lastId))
            {
                return lastId;
            }
            else
            {
                Console.WriteLine("Invalid ID format in last line.");
                return 0;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error reading file: " + ex.Message);
            return 0;
        }
    }
    
    public List<Flight> GetAllFlights()
        {
            try
            {
                var lines = File.ReadAllLines(_flightsFilePath)
                    .Where(line => !string.IsNullOrWhiteSpace(line))
                    .Skip(1) // skip header
                    .ToList();

                var flights = lines.Select(line =>
                {
                    var parts = line.Split(',');

                    return new Flight
                    {
                        Id = int.Parse(parts[0]),
                        Price = decimal.Parse(parts[1]),
                        DepartureCountry = parts[2],
                        DestinationCountry = parts[3],
                        DepartureDate = DateTime.Parse(parts[4]),
                        DepartureAirport = parts[5],
                        ArrivalAirport = parts[6],
                        Classes = parts[7]
                    };
                }).ToList();

                return flights;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error reading flights: " + ex.Message);
                return [];
            }
        }

        public Flight? GetFlightById(int id)
        {
            try
            {
                var lines = File.ReadAllLines(_flightsFilePath)
                    .Where(line => !string.IsNullOrWhiteSpace(line))
                    .Skip(1)
                    .ToList();

                foreach (var parts in lines.Select(line => line.Split(',')))
                {
                    if (int.TryParse(parts[0], out var flightId) && flightId == id)
                    {
                        return new Flight
                        {
                            Id = flightId,
                            Price = decimal.Parse(parts[1]),
                            DepartureCountry = parts[2],
                            DestinationCountry = parts[3],
                            DepartureDate = DateTime.Parse(parts[4]),
                            DepartureAirport = parts[5],
                            ArrivalAirport = parts[6],
                            Classes = parts[7]
                        };
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error reading flight by id: " + ex.Message);
                return null;
            }
        }
}