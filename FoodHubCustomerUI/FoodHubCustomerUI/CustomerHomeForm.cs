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

        public CustomerHomeForm()
        {
            InitializeComponent();
            this.Text = $"Food Hub - ลูกค้า: {Program.LoggedInCustomerName}";
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

        private void menuHome_Click(object sender, EventArgs e)
        {
            LoadRestaurants();
        }

        // ปิดแอป
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            Application.Exit();
        }
    }
}
