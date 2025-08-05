using static Airport_Tickets_System.Utils.Functionalities;
using static Airport_Tickets_System.Utils.Constants;

namespace Airport_Tickets_System.Database;

public class FlightsDatabase
{
    private readonly string _flightsFilePath = Path.Combine(GetProjectDirectory(), "DataFiles", Files.Flights);

    public bool InsertFlightsData(string[] flights)
    {
        try
        {
            File.AppendAllLines(_flightsFilePath, flights.Skip(1));
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error writing to file: " + ex.Message);
            return false;
        }
    }
}