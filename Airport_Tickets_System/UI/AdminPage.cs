using System.Globalization;
using System.Text;
using Airport_Tickets_System.Utils;
using Microsoft.VisualBasic;

namespace Airport_Tickets_System.UI;

public class AdminPage
{
    private readonly Repository _repository = new Repository();
    private readonly ValidateUserInputOfFlightsUseCase _validator = new();

    public void Start()
    {
        var result = getUserChoiceOrNull();
        switch (result)
        {
            case 1:
                Console.WriteLine("Show all bookings");
                break;
            case 2:
                Console.WriteLine("Show bookings with filter");
                break;
            case 3:
                ImportFlightData();
                break;
            default:
                Console.WriteLine("Invalid input");
                break;
        }
    }

    private int? getUserChoiceOrNull()
    {
        Console.WriteLine("You entered the Admin Page !!");
        Console.WriteLine("Please Choose one of the following functionality");
        Console.WriteLine("1. show all bookings");
        Console.WriteLine("2. show bookings with filter parameters");
        Console.WriteLine("3. import flight data to the system");
        var choose = Console.ReadLine();
        var success = int.TryParse(choose, out var result);
        return success ? result : null;
    }

    void ImportFlightData()
    {
        Console.WriteLine(
            "-- Please enter the absolute path of the csv file:\n" +
            "-- note that the file should be in this format:\n" +
            "-- price, departureCountry, destinationCountry, DepartureDate, DepartureAirport, ArrivalAirport, Classes\n" +
            "-- IMPORTANT: first line will be skipped as we assume it is a header line"
        );
        var absolutePath = Console.ReadLine();
        if (string.IsNullOrEmpty(absolutePath) || !File.Exists(absolutePath))
        {
            Console.WriteLine("File Path Not Valid !");
            return;
        }

        var lines = File.ReadAllLines(absolutePath);
        if (_validator.ValidateFlightsInputData(lines))
        {
            var successFlag = _repository.InsertFlightsData(lines);
            var message = successFlag ? "Flights Data Inserted Successfully." : "Insertion Failed!";
            Console.WriteLine(message);
        }
    }
}