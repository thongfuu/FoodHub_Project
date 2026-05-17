namespace FoodHubRestaurantUI
{
    internal static class Program
    {
        public static readonly HttpClient ApiClient = new HttpClient { BaseAddress = new Uri("http://localhost:5000/") };

        public static int LoggedInRestaurantId = 0;
        public static string LoggedInRestaurantName = "";

        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new RestaurantLoginForm());
        }
    }
}