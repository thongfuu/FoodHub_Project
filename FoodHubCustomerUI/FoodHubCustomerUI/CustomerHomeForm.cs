using FoodHubCustomerUI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FoodHubCustomerUI
{
    public partial class CustomerHomeForm : Form
    {
        private List<RestaurantDTO> allRestaurants = new List<RestaurantDTO>();
        private bool isLoggingOut = false;

        public CustomerHomeForm()
        {
            InitializeComponent();
            ApplyModernLayout();
            this.Text = $"FoodHub - ลูกค้า: {Program.LoggedInCustomerName}";
        }

        private void ApplyModernLayout()
        {
            CustomerUiTheme.ApplyForm(this, new Size(1040, 680), "FoodHub - หน้าแรก");
            CustomerUiTheme.StyleMenu(menuMyBooking);

            var title = CustomerUiTheme.CreateTitle("ค้นหาร้านอาหาร", new Point(32, 58), 520);
            var subtitle = CustomerUiTheme.CreateSubtitle($"สวัสดี {Program.LoggedInCustomerName} เลือกร้านที่ชอบ แล้วดับเบิลคลิกเพื่อดูรายละเอียด", new Point(34, 102), 760);

            txtSearch.Location = new Point(32, 150);
            txtSearch.Size = new Size(790, 30);
            txtSearch.PlaceholderText = "ค้นหาชื่อร้านหรือประเภทอาหาร";
            txtSearch.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            CustomerUiTheme.StyleInput(txtSearch);

            btnSearch.Location = new Point(840, 146);
            btnSearch.Size = new Size(160, 40);
            btnSearch.Text = "ค้นหา";
            btnSearch.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            CustomerUiTheme.StylePrimaryButton(btnSearch);

            dgvRestaurants.Location = new Point(32, 210);
            dgvRestaurants.Size = new Size(968, 420);
            dgvRestaurants.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            CustomerUiTheme.StyleGrid(dgvRestaurants);

            Controls.Add(title);
            Controls.Add(subtitle);
            title.BringToFront();
            subtitle.BringToFront();
        }

        protected override async void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            await LoadRestaurants();
        }

        private async System.Threading.Tasks.Task LoadRestaurants()
        {
            try
            {
                var response = await Program.ApiClient.GetAsync("api/Restaurants");
                response.EnsureSuccessStatusCode();

                string json = await response.Content.ReadAsStringAsync();
                allRestaurants = JsonSerializer.Deserialize<List<RestaurantDTO>>(json);

                UpdateGrid(allRestaurants);
            }
            catch (Exception ex) { MessageBox.Show("ดึงข้อมูลไม่ได้: " + ex.Message); }
        }

        private void UpdateGrid(List<RestaurantDTO> data)
        {
            dgvRestaurants.DataSource = data;
            dgvRestaurants.Columns["RestaurantId"].Visible = false;
            dgvRestaurants.Columns["Name"].HeaderText = "ชื่อร้านอาหาร";
            dgvRestaurants.Columns["Category"].HeaderText = "ประเภท";
            dgvRestaurants.Columns["AvgRating"].HeaderText = "คะแนนเฉลี่ย";
            dgvRestaurants.Columns["Name"].FillWeight = 180;
            dgvRestaurants.Columns["Category"].FillWeight = 100;
            dgvRestaurants.Columns["AvgRating"].FillWeight = 70;
        }

        // ปุ่มค้นหา
        private void btnSearch_Click(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.ToLower();
            var filtered = allRestaurants.Where(r => r.Name.ToLower().Contains(keyword) || r.Category.ToLower().Contains(keyword)).ToList();
            UpdateGrid(filtered);
        }

        // ดับเบิลคลิกที่ตาราง เพื่อเปิดหน้าร้านอาหาร
        private void dgvRestaurants_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var selectedRow = dgvRestaurants.Rows[e.RowIndex];
                int restId = (int)selectedRow.Cells["RestaurantId"].Value;
                string restName = selectedRow.Cells["Name"].Value.ToString();

                var detailForm = new RestaurantDetailForm(restId, restName);
                detailForm.ShowDialog(); // ShowDialog จะเปิดหน้าต่างใหม่ทับหน้าเดิม
            }
        }
        private void menuMyBookings_Click(object sender, EventArgs e)
        {
            var myBookingsForm = new MyBookingsForm();
            myBookingsForm.ShowDialog(); // เปิดหน้าประวัติการจองขึ้นมาทับ
        }
        private void btnLogout_Click(object sender, EventArgs e)
        {
            var confirm = MessageBox.Show("คุณต้องการออกจากระบบใช่หรือไม่?", "ยืนยัน", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                isLoggingOut = true;

                // 1. เคลียร์ข้อมูลลูกค้าที่เคยล็อกอินค้างไว้ใน Program.cs
                Program.LoggedInCustomerId = 0;
                Program.LoggedInCustomerName = "";
                Program.ClaimedCoupons.Clear();

                // 2. เปิดหน้า Login ใหม่ขึ้นมา
                var loginForm = new CustomerLoginForm();
                loginForm.Show();

                // 3. ปิดหน้า Home ทิ้งไป
                this.Close();
            }
        }
        private void menuHome_Click(object sender, EventArgs e)
        {
            LoadRestaurants();
        }

        // ปิดแอป
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);

            if (!isLoggingOut)
            {
                Application.Exit();
            }
        }
    }
}
