using Airport_Tickets_System.Database;
using Airport_Tickets_System.models;
using Airport_Tickets_System.states;

namespace Airport_Tickets_System;

public class Repository
{
    private readonly LoginDataBase _loginDataBase = new LoginDataBase();
    private readonly FlightsDatabase _flightsDatabase = new FlightsDatabase();


    public LoginState ValidateUserCredentials(User user)
    {
        return _loginDataBase.ValidateUserCredentials(user);
    }

    public bool InsertFlightsData(string[] flights)
    {
        return _flightsDatabase.InsertFlightsData(flights);
    }
}