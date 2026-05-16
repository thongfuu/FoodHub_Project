using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using FoodHubRestaurantUI.Models;

namespace FoodHubRestaurantUI
{
    public partial class RestaurantDashboardForm : Form
    {
        public RestaurantDashboardForm()
        {
            InitializeComponent();
            // แสดงชื่อร้านที่แถบด้านบนสุดของหน้าต่าง
            this.Text = $"Dashboard ร้าน: {Program.LoggedInRestaurantName}";
        }

        protected override async void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            await LoadBookings();
        }

        private async Task LoadBookings()
        {
            try
            {
                var response = await Program.ApiClient.GetAsync($"api/Bookings/restaurant/{Program.LoggedInRestaurantId}");
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var allBookings = JsonSerializer.Deserialize<List<BookingDTO>>(json, options);

                    // 🌟 ล้างข้อมูลตารางเก่าทิ้งก่อน เพื่อให้มันอัปเดตหน้าจอทันทีตอนสลับแท็บ
                    dgvPending.DataSource = null;
                    dgvConfirmed.DataSource = null;

                    // 1. กรองใส่ตาราง Pending (รอการยืนยัน - แท็บ 1)
                    var pendingBookings = allBookings.Where(b => b.Status == "Pending").ToList();
                    dgvPending.DataSource = pendingBookings.Select(b => new {
                        ID = b.BookingId,
                        ชื่อลูกค้า = b.Customer?.Name ?? "ไม่ระบุ",
                        เวลา = b.BookingDate.ToString("dd/MM/yyyy HH:mm"),
                        จำนวน = b.NumPeople + " คน"
                    }).ToList();

                    // 2. กรองใส่ตาราง Confirmed (รับคิวแล้ว - แท็บ 2)
                    var confirmedBookings = allBookings.Where(b => b.Status == "Confirmed").ToList();
                    dgvConfirmed.DataSource = confirmedBookings.Select(b => new {
                        ID = b.BookingId,
                        ชื่อลูกค้า = b.Customer?.Name ?? "ไม่ระบุ",
                        เวลา = b.BookingDate.ToString("dd/MM/yyyy HH:mm"),
                        จำนวน = b.NumPeople + " คน"
                    }).ToList();

                    // ซ่อนคอลัมน์ ID ทั้งสองตาราง (เก็บไว้ใช้ตอนกดปุ่ม แต่ไม่ต้องโชว์ให้รก)
                    if (dgvPending.Columns.Contains("ID")) dgvPending.Columns["ID"].Visible = false;
                    if (dgvConfirmed.Columns.Contains("ID")) dgvConfirmed.Columns["ID"].Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("ดึงข้อมูลคิวสำเร็จ: " + ex.Message);
            }
        }

        // ฟังก์ชันช่วยยิง API อัปเดตสถานะ (ใช้ร่วมกันทุกปุ่ม)
        private async Task UpdateStatus(int bookingId, string newStatus)
        {
            var content = new StringContent(JsonSerializer.Serialize(newStatus), Encoding.UTF8, "application/json");
            var response = await Program.ApiClient.PutAsync($"api/Bookings/{bookingId}/status", content);
            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show($"อัปเดตสถานะเป็น {newStatus} สำเร็จ");
                await LoadBookings(); // รีเฟรชตารางทันทีหลังอัปเดตเสร็จ
            }
            else
            {
                MessageBox.Show($"เกิดข้อผิดพลาดในการอัปเดตสถานะ: {response.StatusCode}");
            }
        }

        // --- Event ของปุ่มต่างๆ ในแท็บที่ 1 (Pending) ---
        private async void btnConfirm_Click(object sender, EventArgs e)
        {
            if (dgvPending.SelectedRows.Count > 0)
            {
                int id = (int)dgvPending.SelectedRows[0].Cells["ID"].Value;
                await UpdateStatus(id, "Confirmed");
            }
        }

        private async void btnReject_Click(object sender, EventArgs e)
        {
            if (dgvPending.SelectedRows.Count > 0)
            {
                int id = (int)dgvPending.SelectedRows[0].Cells["ID"].Value;
                await UpdateStatus(id, "Rejected");
            }
        }

        // --- Event ของปุ่มในแท็บที่ 2 (Confirmed) ---
        private async void btnComplete_Click(object sender, EventArgs e)
        {
            if (dgvConfirmed.SelectedRows.Count > 0)
            {
                int id = (int)dgvConfirmed.SelectedRows[0].Cells["ID"].Value;
                await UpdateStatus(id, "Completed");
            }
        }

        // --- Event ของปุ่ม ลูกค้าไม่มาตามนัด (No Show) ---
        private async void btnNoShow_Click(object sender, EventArgs e)
        {
            if (dgvConfirmed.SelectedRows.Count > 0)
            {
                // ควรถามยืนยันก่อนกด เพราะมีผลต่อประวัติลูกค้า
                var confirmResult = MessageBox.Show("ยืนยันว่าลูกค้าไม่มาตามนัดใช่หรือไม่?\n(ระบบจะบันทึกประวัติเสียของลูกค้า)",
                                                    "ยืนยัน No Show", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (confirmResult == DialogResult.Yes)
                {
                    int id = (int)dgvConfirmed.SelectedRows[0].Cells["ID"].Value;
                    await UpdateStatus(id, "NoShow");
                }
            }
        }

        // --- ปุ่ม Refresh ข้อมูลล่าสุด ---
        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            await LoadBookings();
        }

        // --- เมนูเปิดหน้าจัดการโปรโมชั่น (บน MenuStrip) ---
        private void menuPromotions_Click(object sender, EventArgs e)
        {
            var promoForm = new PromotionManagementForm();
            promoForm.ShowDialog();
        }

        // ปิดโปรแกรมทั้งหมดเมื่อปิดหน้าต่างนี้
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            Application.Exit();
        }
    }
}