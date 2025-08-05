using Airport_Tickets_System.models;
using Airport_Tickets_System.states;

namespace Airport_Tickets_System.UI;

public class LoginPage
{
    private readonly Repository _repository = new Repository();
    public LoginState Login()
    {
        var user = ReadUserInput();
        if (user == null)
        {
            return LoginState.LoggingInFailed;
        }

        return _repository.ValidateUserCredentials(user);

    }

    private User? ReadUserInput()
    {
        Console.WriteLine("Enter your username: ");
        var username = Console.ReadLine();
        Console.WriteLine("Enter your password: ");
        var password = Console.ReadLine();
        if (
            username == null
            || username.Trim().Equals(string.Empty)
            || password == null
            || password.Trim().Equals(string.Empty))
        {
            return null;
        }

        return new User(username.Trim(), password.Trim());
    }
}