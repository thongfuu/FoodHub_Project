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
            ApplyModernLayout();
            // แสดงชื่อร้านที่แถบด้านบนสุดของหน้าต่าง
            this.Text = $"FoodHub Partner - {Program.LoggedInRestaurantName}";
        }

        private void ApplyModernLayout()
        {
            RestaurantUiTheme.ApplyForm(this, new Size(1080, 700), "FoodHub Partner - Dashboard");
            RestaurantUiTheme.StyleMenu(menuStrip1);
            RestaurantUiTheme.StyleTabs(tabControl1);

            var title = RestaurantUiTheme.CreateTitle("จัดการคิวจอง", new Point(32, 58), 520);
            var subtitle = RestaurantUiTheme.CreateSubtitle($"ร้าน {Program.LoggedInRestaurantName} ตรวจสอบคิวใหม่และอัปเดตสถานะได้จากหน้านี้", new Point(34, 102), 820);

            btnRefresh.Location = new Point(900, 70);
            btnRefresh.Size = new Size(140, 42);
            btnRefresh.Text = "รีเฟรช";
            btnRefresh.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            RestaurantUiTheme.StyleSecondaryButton(btnRefresh);

            tabControl1.Location = new Point(32, 150);
            tabControl1.Size = new Size(1008, 500);
            tabControl1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            tabPage1.Text = "รอการยืนยัน";
            tabPage2.Text = "คิวที่รับแล้ว";
            tabPage1.Resize += (_, _) => LayoutBookingTab(tabPage1, dgvPending, btnConfirm, btnReject);
            tabPage2.Resize += (_, _) => LayoutBookingTab(tabPage2, dgvConfirmed, btnComplete, btnNoShow);

            dgvPending.Location = new Point(18, 22);
            dgvPending.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            RestaurantUiTheme.StyleGrid(dgvPending);

            btnConfirm.Size = new Size(200, 44);
            btnConfirm.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            RestaurantUiTheme.StylePrimaryButton(btnConfirm);

            btnReject.Size = new Size(200, 44);
            btnReject.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            RestaurantUiTheme.StyleDangerButton(btnReject);
            btnConfirm.BringToFront();
            btnReject.BringToFront();

            dgvConfirmed.Location = new Point(18, 22);
            dgvConfirmed.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            RestaurantUiTheme.StyleGrid(dgvConfirmed);

            btnComplete.Size = new Size(200, 44);
            btnComplete.Text = "เช็คอิน / เสร็จแล้ว";
            btnComplete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            RestaurantUiTheme.StylePrimaryButton(btnComplete);

            btnNoShow.Size = new Size(200, 44);
            btnNoShow.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            RestaurantUiTheme.StyleDangerButton(btnNoShow);
            btnComplete.BringToFront();
            btnNoShow.BringToFront();
            LayoutBookingTab(tabPage1, dgvPending, btnConfirm, btnReject);
            LayoutBookingTab(tabPage2, dgvConfirmed, btnComplete, btnNoShow);

            Controls.Add(title);
            Controls.Add(subtitle);
            title.BringToFront();
            subtitle.BringToFront();
            btnRefresh.BringToFront();
        }

        private static void LayoutBookingTab(TabPage page, DataGridView grid, Button primaryButton, Button dangerButton)
        {
            const int margin = 18;
            const int actionWidth = 200;
            const int gap = 24;
            var actionLeft = Math.Max(margin, page.ClientSize.Width - actionWidth - margin);
            var gridWidth = Math.Max(360, actionLeft - gap - margin);

            grid.Location = new Point(margin, 22);
            grid.Size = new Size(gridWidth, Math.Max(260, page.ClientSize.Height - 44));

            primaryButton.Location = new Point(actionLeft, 32);
            dangerButton.Location = new Point(actionLeft, 88);
            primaryButton.BringToFront();
            dangerButton.BringToFront();
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
                    SetBookingGridWeights(dgvPending);
                    SetBookingGridWeights(dgvConfirmed);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("ดึงข้อมูลคิวสำเร็จ: " + ex.Message);
            }
        }

        private static void SetBookingGridWeights(DataGridView grid)
        {
            if (grid.Columns.Contains("ชื่อลูกค้า")) grid.Columns["ชื่อลูกค้า"].FillWeight = 150;
            if (grid.Columns.Contains("เวลา")) grid.Columns["เวลา"].FillWeight = 120;
            if (grid.Columns.Contains("จำนวน")) grid.Columns["จำนวน"].FillWeight = 70;
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
