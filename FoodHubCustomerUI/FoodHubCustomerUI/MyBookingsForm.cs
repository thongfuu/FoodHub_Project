using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Windows.Forms;
using FoodHubCustomerUI.Models;

namespace FoodHubCustomerUI
{
    public partial class MyBookingsForm : Form
    {
        public MyBookingsForm() { InitializeComponent(); }

        protected override async void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            await LoadBookings();
        }

        private async Task LoadBookings()
        {
            try
            {
                var response = await Program.ApiClient.GetAsync($"api/Bookings/customer/{Program.LoggedInCustomerId}");
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var bookings = JsonSerializer.Deserialize<List<BookingDTO>>(json, options);

                    // ปรับแต่งข้อมูลโชว์ในตาราง
                    dgvBookings.DataSource = bookings.Select(b => new
                    {
                        ID = b.BookingId,
                        ร้านอาหาร = b.Restaurant?.Name ?? "ไม่ระบุ",
                        วันที่ = b.BookingDate.ToString("dd/MM/yyyy HH:mm"),
                        จำนวน = b.NumPeople + " คน",
                        สถานะ = b.Status,
                        RestaurantId = b.RestaurantId // ซ่อนไว้ใช้ตอนรีวิว
                    }).ToList();
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        // --- ปุ่มยกเลิกจอง ---
        private async void btnCancel_Click(object sender, EventArgs e)
        {
            if (dgvBookings.SelectedRows.Count == 0) return;

            int bookingId = (int)dgvBookings.SelectedRows[0].Cells["ID"].Value;
            string status = dgvBookings.SelectedRows[0].Cells["สถานะ"].Value.ToString();

            if (status != "Pending" && status != "Confirmed")
            {
                MessageBox.Show("ยกเลิกการจองสำเร็จแล้ว ไม่สามารถยกเลิกซ้ำได้");
                return;
            }

            var confirm = MessageBox.Show("ยืนยันการยกเลิกการจองนี้หรือไม่?", "ยืนยัน", MessageBoxButtons.YesNo);
            if (confirm == DialogResult.Yes)
            {
                var resp = await Program.ApiClient.PostAsync($"api/Bookings/cancel/{bookingId}", null);
                if (resp.IsSuccessStatusCode)
                {
                    MessageBox.Show("ยกเลิกการจองสำเร็จ");
                    await LoadBookings();
                }
            }
        }

        // --- ปุ่มเขียนรีวิว ---
        private void btnReview_Click(object sender, EventArgs e)
        {
            if (dgvBookings.SelectedRows.Count == 0) return;

            string status = dgvBookings.SelectedRows[0].Cells["สถานะ"].Value.ToString();
            if (status != "Completed")
            {
                MessageBox.Show("ยังไม่สามารถเขียนรีวิวได้ในตอนนี้");
                return;
            }

            int restId = (int)dgvBookings.SelectedRows[0].Cells["RestaurantId"].Value;
            string restName = dgvBookings.SelectedRows[0].Cells["ร้านอาหาร"].Value.ToString();

            // เปิดหน้าต่างรีวิว
            var reviewForm = new ReviewDialogForm(restId, restName);
            if (reviewForm.ShowDialog() == DialogResult.OK)
            {
                LoadBookings(); // รีเฟรชหน้าจอหลังรีวิวเสร็จ
            }
        }
        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvBookings_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvBookings.Columns[e.ColumnIndex].Name == "สถานะ" && e.Value != null)
            {
                string status = e.Value.ToString();

                if (status == "Cancelled" || status == "LateCancelled")
                {
                    e.CellStyle.ForeColor = Color.White;
                    e.CellStyle.BackColor = Color.Red;

                    // ทำให้ทั้งบรรทัดกลายเป็นสีเทาจางๆ (ตัวเลือกเสริม)
                    dgvBookings.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightGray;
                }
                else if (status == "Confirmed")
                {
                    e.CellStyle.ForeColor = Color.Green;
                    e.CellStyle.Font = new Font(e.CellStyle.Font, FontStyle.Bold); // ทำตัวหนา
                }
                else if (status == "Pending")
                {
                    e.CellStyle.ForeColor = Color.DarkOrange;
                }
                else if (status == "Completed")
                {
                    e.CellStyle.ForeColor = Color.Blue;
                }
            }
        }
    }
}