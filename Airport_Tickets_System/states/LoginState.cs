namespace Airport_Tickets_System.states;

public enum LoginState
{
    PassengerLoggedIn,
    AdminLoggedIn,
    LoggingInFailed,
}

public class LoginResult(LoginState state, string? username = null)
{
    public LoginState State { get; } = state;
    public string? UserName { get; } = username;
}