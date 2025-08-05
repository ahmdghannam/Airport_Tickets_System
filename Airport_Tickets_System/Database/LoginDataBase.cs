using Airport_Tickets_System.models;
using Airport_Tickets_System.states;
using Airport_Tickets_System.Utils;

namespace Airport_Tickets_System.Database;

public class LoginDataBase
{
    private readonly string[] _lines = File.ReadAllLines(
        Path.Combine(AppContext.BaseDirectory, "DataFiles", Constants.Files.UsersCredentials)
    );

    public LoginState ValidateUserCredentials(User user)
    {
        var matchedUser =
            (from line in _lines.Skip(1)
                let parts = line.Split(',')
                where parts[0].Trim() == user.Username && parts[1].Trim() == user.Password
                select parts).FirstOrDefault();

        if (matchedUser == null)
            return LoginState.LoggingInFailed;

        return matchedUser[2].Trim() == "Admin" ? LoginState.AdminLoggedIn : LoginState.PassengerLoggedIn;
    }
}