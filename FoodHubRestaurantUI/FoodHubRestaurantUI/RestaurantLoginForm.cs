using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Windows.Forms;
using FoodHubRestaurantUI.Models;

namespace FoodHubRestaurantUI
{
    public partial class RestaurantLoginForm : Form
    {
        public RestaurantLoginForm() { InitializeComponent(); }

        protected override async void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            try
            {
                var response = await Program.ApiClient.GetAsync("api/Restaurants");
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var restaurants = JsonSerializer.Deserialize<List<RestaurantDTO>>(json, options);

                    cmbRestaurants.DataSource = restaurants;
                    cmbRestaurants.DisplayMember = "Name";
                    cmbRestaurants.ValueMember = "RestaurantId";
                }
            }
            catch (Exception ex) { MessageBox.Show("เชื่อมต่อ API ไม่ได้: " + ex.Message); }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (cmbRestaurants.SelectedItem is RestaurantDTO selectedRest)
            {
                Program.LoggedInRestaurantId = selectedRest.RestaurantId;
                Program.LoggedInRestaurantName = selectedRest.Name;

                var dashboard = new RestaurantDashboardForm();
                dashboard.Show();
                this.Hide();
            }
        }
    }
}