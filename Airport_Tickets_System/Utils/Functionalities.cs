using System.Text.RegularExpressions;

namespace Airport_Tickets_System.Utils;

public class Functionalities
{
    public static string GetProjectDirectory()
    {
        var dir = AppContext.BaseDirectory;
        for (var i = 0; i < 4; i++)
        {
            var parent = Directory.GetParent(dir);
            if (parent == null) continue;
            dir = parent.FullName;
        }

        return dir;
    }
    public static bool ContainsNonAlphabetical(string input)
    {
        return !Regex.IsMatch(input, @"^[a-zA-Z\s]*$");
    }
}