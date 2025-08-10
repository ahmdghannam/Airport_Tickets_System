using Airport_Tickets_System.states;

namespace Airport_Tickets_System.UI
{
    internal static class MainPoint
    {
        private static readonly LoginPage LoginPage = new LoginPage();
        private static readonly AdminPage AdminPage = new AdminPage();
        private static readonly PassengerPage PassengerPage = new PassengerPage();
        static void Main(string[] args)
        {
            var loginResult = LoginPage.Login();
            switch (loginResult.State)
            {
                case LoginState.PassengerLoggedIn:
                    Console.WriteLine("Passenger logged in");
                    PassengerPage.Start(loginResult.UserName!);
                    break;
                case LoginState.AdminLoggedIn:
                    Console.WriteLine("Admin logged in");
                    AdminPage.Start();
                    break;
                case LoginState.LoggingInFailed:
                    Console.WriteLine("Logging in failed");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}