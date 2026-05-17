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
            ApplyModernLayout();
            this.Text = "FoodHub Partner - จัดการโปรโมชั่น";
        }

        private void ApplyModernLayout()
        {
            RestaurantUiTheme.ApplyForm(this, new Size(1040, 680), "FoodHub Partner - จัดการโปรโมชั่น");

            label1.Text = "จัดการโปรโมชั่น";
            label1.Location = new Point(32, 28);
            label1.Size = new Size(360, 36);
            label1.AutoSize = false;
            RestaurantUiTheme.StyleLabel(label1, section: true);

            var subtitle = RestaurantUiTheme.CreateSubtitle("ดูโปรโมชันที่กำลังใช้งาน และสร้างข้อเสนอใหม่ให้ลูกค้า", new Point(34, 64), 720);
            Controls.Add(subtitle);

            dgvPromotions.Location = new Point(32, 112);
            dgvPromotions.Size = new Size(620, 500);
            dgvPromotions.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            RestaurantUiTheme.StyleGrid(dgvPromotions);

            groupBox1.Text = "สร้างโปรโมชั่นใหม่";
            groupBox1.Location = new Point(684, 112);
            groupBox1.Size = new Size(324, 500);
            groupBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            groupBox1.BackColor = RestaurantUiTheme.Surface;
            groupBox1.ForeColor = RestaurantUiTheme.Text;
            groupBox1.Font = RestaurantUiTheme.SectionFont;

            label2.Text = "ชื่อโปรโมชั่น";
            label2.Location = new Point(22, 46);
            label2.Size = new Size(260, 24);
            label2.AutoSize = false;
            RestaurantUiTheme.StyleLabel(label2);

            txtTitle.Location = new Point(22, 74);
            txtTitle.Size = new Size(280, 30);
            RestaurantUiTheme.StyleInput(txtTitle);

            label3.Text = "รายละเอียด";
            label3.Location = new Point(22, 124);
            label3.Size = new Size(260, 24);
            label3.AutoSize = false;
            RestaurantUiTheme.StyleLabel(label3);

            txtDescription.Location = new Point(22, 152);
            txtDescription.Size = new Size(280, 120);
            RestaurantUiTheme.StyleInput(txtDescription);

            label4.Text = "ส่วนลด";
            label4.Location = new Point(22, 296);
            label4.Size = new Size(92, 24);
            label4.AutoSize = false;
            RestaurantUiTheme.StyleLabel(label4);

            numDiscount.Location = new Point(120, 294);
            numDiscount.Size = new Size(150, 30);
            RestaurantUiTheme.StyleInput(numDiscount);

            label5.Location = new Point(276, 298);
            label5.Size = new Size(24, 24);
            label5.AutoSize = false;
            RestaurantUiTheme.StyleLabel(label5);

            label6.Text = "วันหมดอายุ";
            label6.Location = new Point(22, 346);
            label6.Size = new Size(260, 24);
            label6.AutoSize = false;
            RestaurantUiTheme.StyleLabel(label6);

            dtpExpireDate.Location = new Point(22, 374);
            dtpExpireDate.Size = new Size(280, 30);
            RestaurantUiTheme.StyleInput(dtpExpireDate);

            btnPublish.Location = new Point(22, 430);
            btnPublish.Size = new Size(280, 44);
            btnPublish.Text = "เผยแพร่โปรโมชั่น";
            RestaurantUiTheme.StylePrimaryButton(btnPublish);
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
                    if (dgvPromotions.Columns.Contains("Title")) dgvPromotions.Columns["Title"].HeaderText = "ชื่อโปรโมชั่น";
                    if (dgvPromotions.Columns.Contains("Description")) dgvPromotions.Columns["Description"].HeaderText = "รายละเอียด";
                    if (dgvPromotions.Columns.Contains("Discount")) dgvPromotions.Columns["Discount"].HeaderText = "ส่วนลด";
                    if (dgvPromotions.Columns.Contains("ExpireDate")) dgvPromotions.Columns["ExpireDate"].HeaderText = "หมดอายุ";
                    if (dgvPromotions.Columns.Contains("IsActive")) dgvPromotions.Columns["IsActive"].HeaderText = "เปิดใช้งาน";
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
