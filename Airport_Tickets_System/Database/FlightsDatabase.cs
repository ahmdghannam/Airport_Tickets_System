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
}