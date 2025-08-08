using System.Globalization;
using System.Text;
using Airport_Tickets_System.Utils;

namespace Airport_Tickets_System;

public class ValidateUserInputOfFlightsUseCase
{
    public bool ValidateFlightsInputData(string[] lines)
    {
        var resultMessage = new StringBuilder();
        
        for (int i = 1; i < lines.Length; i++)
        {
            var row = lines[i].Split(",");
            ValidateRow(row, resultMessage, i);
        }
        if (resultMessage.Length == 0) return true;
        Console.WriteLine("|=================================================================================|");
        Console.WriteLine(resultMessage);
        Console.WriteLine("|=================================================================================|");
        return false;
    }

    private void ValidateRow(string[] row, StringBuilder resultMessage, int i)
    {
        if (row.Length != Consts.NumberOfFlightsColumns)
        {
            resultMessage.AppendLine($"AT ROW - {i + 1} <> Invalid number of entries <>");
            return;
        }

        var validatePrice = ValidatePrice(row[0].Trim());
        if (validatePrice != null)
        {
            resultMessage.AppendLine($"AT ROW - {i + 1} <> {validatePrice} <>");
        }

        var validateDepartureCountry = ValidateCountryName(row[1].Trim());
        if (validateDepartureCountry != null)
        {
            resultMessage.AppendLine($"AT ROW - {i + 1} <> {validateDepartureCountry} <>");
        }

        var validateDestinationCountry = ValidateCountryName(row[2].Trim());
        if (validateDepartureCountry != null)
        {
            resultMessage.AppendLine($"AT ROW - {i + 1} <> {validateDestinationCountry} <>");
        }

        var validateDepartureDate = ValidateDepartureDate(row[3].Trim());
        if (validateDepartureDate != null)
        {
            resultMessage.AppendLine($"AT ROW - {i + 1} <> {validateDepartureDate} <>");
        }

        var validateDepartureAirport = ValidateAirportName(row[4].Trim());
        if (validateDepartureAirport != null)
        {
            resultMessage.AppendLine($"AT ROW - {i + 1} <> {validateDepartureAirport} <>");
        }

        var validateArrivalAirport = ValidateAirportName(row[5].Trim());
        if (validateArrivalAirport != null)
        {
            resultMessage.AppendLine($"AT ROW - {i + 1} <> {validateArrivalAirport} <>");
        }

        var validateClasses = ValidateClasses(row[6].Trim());

        if (validateClasses != null)
        {
            resultMessage.AppendLine($"AT ROW - {i + 1} <> {validateClasses} <>");
        }
    }

    private string? ValidateClasses(string theClasses)
    {
        var classes = theClasses.Split("-");

        foreach (var theClass in classes)
        {
            if (!Enum.TryParse<FlightClasses>(theClass, true, out var parsedClass))
            {
                return "Invalid flight classes";
            }
        }

        return null;
    }

    private string? ValidateAirportName(string airportName)
    {
        if (string.IsNullOrEmpty(airportName))
        {
            return "No airport name detected";
        }

        if (airportName.Length > 100)
        {
            return "Country Name length is not valid";
        }

        return null;
    }

    private string? ValidateDepartureDate(string date)
    {
        if (string.IsNullOrWhiteSpace(date))
        {
            return "No Departure Date detected.";
        }

        if (!DateTime.TryParseExact(
                date,
                "yyyy-M-d H:mm",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out DateTime parsedDate))
        {
            return "Departure Date should be in format YYYY-M-D H:MM (e.g., 2025-9-10 10:01).";
        }

        if (parsedDate < DateTime.Now)
        {
            return "Departure Date must be now or a future date.";
        }

        return null;
    }

    private string? ValidateCountryName(string countryName)
    {
        if (string.IsNullOrEmpty(countryName))
        {
            return "No country name name detected";
        }

        if (countryName.Length > 100)
        {
            return "Country Name length is not valid";
        }

        if (Functionalities.ContainsNonAlphabetical(countryName))
        {
            return "Departure Country is not valid, contains non-alphabetical characters";
        }


        return null;
    }

    private string? ValidatePrice(string price)
    {
        if (string.IsNullOrEmpty(price))
        {
            return "No price detected";
        }

        var success = double.TryParse(price, out var result);
        return success ? null : "The Price is not valid double value";
    }
}