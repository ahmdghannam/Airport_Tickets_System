using Airport_Tickets_System.Database;
using Airport_Tickets_System.models;
using Airport_Tickets_System.states;

namespace Airport_Tickets_System;

public class Repository
{
 
    private LoginDataBase _loginDataBase = new LoginDataBase();

    public LoginState ValidateUserCredentials(User user)
    {
        return _loginDataBase.ValidateUserCredentials(user);
    }

}