using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Windows.Forms;
using FoodHubRestaurantUI.Models;

namespace FoodHubRestaurantUI
{
    public partial class RestaurantLoginForm : Form
    {
        public RestaurantLoginForm()
        {
            InitializeComponent();
            ApplyModernLayout();
        }

        private void ApplyModernLayout()
        {
            RestaurantUiTheme.ApplyForm(this, new Size(880, 540), "FoodHub - เข้าสู่ระบบร้านอาหาร");

            var title = RestaurantUiTheme.CreateTitle("FoodHub Partner", new Point(70, 78), 390);
            var subtitle = RestaurantUiTheme.CreateSubtitle("เลือกร้านของคุณเพื่อจัดการคิวจองและโปรโมชัน", new Point(72, 124), 430);
            var card = RestaurantUiTheme.CreateCard("loginCard", new Rectangle(500, 86, 300, 330));
            card.Paint += RestaurantUiTheme.PaintCard;

            var cardTitle = RestaurantUiTheme.CreateTitle("เข้าสู่ระบบร้าน", new Point(24, 28), 240);
            cardTitle.Font = RestaurantUiTheme.SectionFont;
            var restaurantLabel = RestaurantUiTheme.CreateSubtitle("เลือกร้านอาหาร", new Point(24, 92), 220);

            Controls.Remove(cmbRestaurants);
            Controls.Remove(btnLogin);

            cmbRestaurants.Location = new Point(24, 126);
            cmbRestaurants.Size = new Size(252, 32);
            cmbRestaurants.DropDownStyle = ComboBoxStyle.DropDownList;
            RestaurantUiTheme.StyleInput(cmbRestaurants);

            btnLogin.Location = new Point(24, 198);
            btnLogin.Size = new Size(252, 44);
            btnLogin.Text = "เข้าสู่ระบบ";
            RestaurantUiTheme.StylePrimaryButton(btnLogin);

            card.Controls.Add(cardTitle);
            card.Controls.Add(restaurantLabel);
            card.Controls.Add(cmbRestaurants);
            card.Controls.Add(btnLogin);
            Controls.Add(title);
            Controls.Add(subtitle);
            Controls.Add(card);
        }

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
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            Application.Exit(); // สั่งฆ่าทุกหน้าต่างที่ซ่อนอยู่ให้ตายสนิท
        }
    }
}
