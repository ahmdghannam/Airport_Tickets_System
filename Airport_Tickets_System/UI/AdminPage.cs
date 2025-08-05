using System.Globalization;

namespace Airport_Tickets_System.UI;

public class AdminPage
{
    private readonly Repository _repository = new Repository();

    public void Start()
    {
        Console.WriteLine("You entered the Admin Page !!");
        Console.WriteLine("Please Choose one of the following functionality");
        Console.WriteLine("1. show all bookings");
        Console.WriteLine("2. show bookings with filter parameters");
        Console.WriteLine("3. import flight data to the system");
        var choose = Console.ReadLine();
        int.TryParse(choose, out var result);

        switch (result)
        {
            case 1:
                break;
            case 2:
                break;
            case 3:
                ImportFlightData();
                break;
            default: throw new ArgumentOutOfRangeException();
        }
    }

    void ImportFlightData()
    {
        Console.WriteLine("Please enter the absolute path of the csv file:\n " +
                          "note that the file should be in this format:\n " +
                          "id, price, departureCountry, destinationCountry, DepartureDate, DepartureAirport, ArrivalAirport, Classes\n" +
                          "IMPORTANT: first line will be skipped as we assume it is a header line"
        );
        var absolutePath = Console.ReadLine();
        if (string.IsNullOrEmpty(absolutePath) || !File.Exists(absolutePath))
        {
            Console.WriteLine("File Path Not Valid !");
        }

        var lines = File.ReadAllLines(absolutePath);
        if (ValidateFlightsInputData(lines))
        {
            var successFlag = _repository.InsertFlightsData(lines);
            var message = successFlag ? "Flights Data Inserted Successfully." : "Insertion Failed!";
            Console.WriteLine(message);
        }
    }

    bool ValidateFlightsInputData(string[] lines)
    {
        return true;
    }
}