using FoodHubRestaurantUI.Models;
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

namespace FoodHubRestaurantUI
{
    public partial class PromotionManagementForm : Form
    {
        public PromotionManagementForm()
        {
            InitializeComponent();
            this.Text = "จัดการโปรโมชั่น";
        }

        protected override async void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            dtpExpireDate.MinDate = DateTime.Now; // ห้ามตั้งวันหมดอายุย้อนหลัง
            await LoadPromotions();
        }

        private async Task LoadPromotions()
        {
            try
            {
                var response = await Program.ApiClient.GetAsync($"api/Promotions/restaurant/{Program.LoggedInRestaurantId}");
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var promos = JsonSerializer.Deserialize<List<PromotionDTO>>(json, options);

                    dgvPromotions.DataSource = promos;

                    // จัดหน้าตาราง
                    if (dgvPromotions.Columns.Contains("PromotionId")) dgvPromotions.Columns["PromotionId"].Visible = false;
                    if (dgvPromotions.Columns.Contains("RestaurantId")) dgvPromotions.Columns["RestaurantId"].Visible = false;
                }
            }
            catch { }
        }

        private async void btnPublish_Click(object sender, EventArgs e)
        {
            // ตรวจสอบความถูกต้องของข้อมูลเบื้องต้น
            if (string.IsNullOrWhiteSpace(txtTitle.Text))
            {
                MessageBox.Show("กรุณากรอกชื่อโปรโมชั่น");
                return;
            }

            var newPromo = new
            {
                restaurantId = Program.LoggedInRestaurantId,
                title = txtTitle.Text,
                description = txtDescription.Text,
                discount = (double)numDiscount.Value,
                expireDate = dtpExpireDate.Value.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                isActive = true
            };

            var content = new StringContent(JsonSerializer.Serialize(newPromo), Encoding.UTF8, "application/json");

            try
            {
                var response = await Program.ApiClient.PostAsync("api/Promotions", content);
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("สร้างโปรโมชั่นใหม่สำเร็จ!");

                    // เคลียร์ฟอร์ม
                    txtTitle.Clear();
                    txtDescription.Clear();
                    numDiscount.Value = 0;

                    // รีเฟรชตาราง
                    await LoadPromotions();
                }
                else
                {
                    string error = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"สร้างไม่สำเร็จ: {error}");
                }
            }
            catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }
        private void label4_Click(object sender, EventArgs e)
        {

        }
        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
