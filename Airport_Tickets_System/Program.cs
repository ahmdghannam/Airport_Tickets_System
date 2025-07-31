using Airport_Tickets_System.states;
using AirPortTickets;

namespace Airport_Tickets_System
{
    class MainPoint
    {
        private static LoginPage loginPage = new LoginPage();

        static void Main(string[] args)
        {
            var loginState = loginPage.Login();
            switch (loginState)
            {
                case LoginState.PassengerLoggedIn:
                    Console.WriteLine("Passenger logged in");
                    break;
                case LoginState.AdminLoggedIn:
                    Console.WriteLine("Admin logged in");
                    break;
                case LoginState.LoggingInFailed:
                    Console.WriteLine("Logging in failed");
                    break;
            }
        }
    }
}