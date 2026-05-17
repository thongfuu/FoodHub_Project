namespace FoodHubCustomerUI
{
    internal static class Program
    {
        public static readonly HttpClient ApiClient = new HttpClient { BaseAddress = new Uri("https://localhost:7053/") };

        public static int LoggedInCustomerId = 0;
        public static string LoggedInCustomerName = "";
        public static List<FoodHubCustomerUI.Models.PromotionDTO> ClaimedCoupons = new List<FoodHubCustomerUI.Models.PromotionDTO>();

        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new CustomerLoginForm());
        }
    }
}