using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Windows.Forms;
using FoodHubCustomerUI.Models;
using System.Globalization;

namespace FoodHubCustomerUI
{
    public partial class RestaurantDetailForm : Form
    {
        // ตัวแปรเก็บข้อมูลร้านที่ถูกเลือก
        private int _restaurantId;
        private string _restaurantName;

        // Constructor: รับค่า ID และชื่อมาจากหน้า Home
        public RestaurantDetailForm(int restaurantId, string restaurantName)
        {
            InitializeComponent();
            _restaurantId = restaurantId;
            _restaurantName = restaurantName;
            ApplyModernLayout();
        }

        private void ApplyModernLayout()
        {
            CustomerUiTheme.ApplyForm(this, new Size(1120, 720), "FoodHub - รายละเอียดร้าน");

            lblRestaurantName.Location = new Point(32, 28);
            lblRestaurantName.Size = new Size(650, 42);
            lblRestaurantName.AutoSize = false;
            lblRestaurantName.Font = CustomerUiTheme.TitleFont;
            lblRestaurantName.ForeColor = CustomerUiTheme.Text;

            lblOpenHours.Location = new Point(34, 78);
            lblOpenHours.Size = new Size(650, 28);
            lblOpenHours.AutoSize = false;
            CustomerUiTheme.StyleLabel(lblOpenHours);

            lblAddress.Location = new Point(34, 108);
            lblAddress.Size = new Size(650, 48);
            CustomerUiTheme.StyleLabel(lblAddress);

            var reviewTitle = CustomerUiTheme.CreateSubtitle("รีวิวจากลูกค้า", new Point(34, 168), 320);
            reviewTitle.Font = CustomerUiTheme.SectionFont;
            reviewTitle.ForeColor = CustomerUiTheme.Text;
            Controls.Add(reviewTitle);

            dgvReviews.Location = new Point(32, 210);
            dgvReviews.Size = new Size(670, 450);
            dgvReviews.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            CustomerUiTheme.StyleGrid(dgvReviews);

            label4.Text = "โปรโมชัน";
            label4.Location = new Point(730, 32);
            label4.Size = new Size(330, 30);
            label4.AutoSize = false;
            CustomerUiTheme.StyleLabel(label4, section: true);

            dgvPromotions.Location = new Point(730, 72);
            dgvPromotions.Size = new Size(350, 210);
            dgvPromotions.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            CustomerUiTheme.StyleGrid(dgvPromotions);

            btnClaimCoupon.Location = new Point(730, 294);
            btnClaimCoupon.Size = new Size(350, 42);
            btnClaimCoupon.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            CustomerUiTheme.StyleSecondaryButton(btnClaimCoupon);

            var bookingTitle = CustomerUiTheme.CreateSubtitle("จองโต๊ะ", new Point(730, 366), 330);
            bookingTitle.Font = CustomerUiTheme.SectionFont;
            bookingTitle.ForeColor = CustomerUiTheme.Text;
            Controls.Add(bookingTitle);

            label1.Location = new Point(730, 414);
            label1.Size = new Size(350, 24);
            label1.AutoSize = false;
            CustomerUiTheme.StyleLabel(label1);

            dtpBookingDate.Location = new Point(730, 442);
            dtpBookingDate.Size = new Size(350, 30);
            CustomerUiTheme.StyleInput(dtpBookingDate);

            label2.Location = new Point(730, 488);
            label2.Size = new Size(110, 28);
            label2.AutoSize = false;
            CustomerUiTheme.StyleLabel(label2);

            numPeople.Location = new Point(850, 486);
            numPeople.Size = new Size(230, 30);
            CustomerUiTheme.StyleInput(numPeople);

            cmbCoupons.Location = new Point(730, 532);
            cmbCoupons.Size = new Size(350, 30);
            CustomerUiTheme.StyleInput(cmbCoupons);

            btnBook.Location = new Point(730, 590);
            btnBook.Size = new Size(350, 46);
            btnBook.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            CustomerUiTheme.StylePrimaryButton(btnBook);
        }

        // --- เหตุการณ์ตอนโหลดหน้าจอครั้งแรก ---
        protected override async void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // ตั้งค่าพื้นฐาน
            lblRestaurantName.Text = _restaurantName;
            dtpBookingDate.MinDate = DateTime.Now; // ห้ามจองย้อนหลัง
            numPeople.Value = 2; // ตั้งค่าเริ่มต้น 2 คน

            // ดึงข้อมูล 3 อย่างพร้อมกันจาก API
            await LoadRestaurantDetail();
            await LoadPromotions();
            await LoadReviews();

            // อัปเดตช่องเลือกคูปอง (ถ้ามีเก็บไว้ในกระเป๋าแล้ว)
            UpdateCouponDropdown();
        }

        // 1. ฟังก์ชันดึงรายละเอียดร้าน (เวลา, ที่อยู่)
        private async System.Threading.Tasks.Task LoadRestaurantDetail()
        {
            try
            {
                var response = await Program.ApiClient.GetAsync($"api/Restaurants/{_restaurantId}");
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    var rest = JsonSerializer.Deserialize<RestaurantDTO>(json);

                    lblOpenHours.Text = $"เวลาเปิด-ปิด: {rest?.OpenHours ?? "-"}";
                    lblAddress.Text = $"ที่อยู่: {rest?.Address ?? "-"}";
                }
            }
            catch { } // ปล่อยผ่านถ้าดึงไม่ได้
        }

        // 2. ฟังก์ชันดึงโปรโมชั่นของร้าน
        private async System.Threading.Tasks.Task LoadPromotions()
        {
            try
            {
                var response = await Program.ApiClient.GetAsync($"api/Promotions/restaurant/{_restaurantId}");
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var promos = JsonSerializer.Deserialize<List<PromotionDTO>>(json);

                    // เช็คว่ามีข้อมูลไหม ถ้าไม่มีจะได้ไม่พยายามเซ็ตตาราง
                    if (promos != null && promos.Count > 0)
                    {
                        dgvPromotions.DataSource = promos;
                        // จัดหน้าตาตาราง
                        dgvPromotions.Columns["PromotionId"].Visible = false;
                        dgvPromotions.Columns["RestaurantId"].Visible = false;
                        dgvPromotions.Columns["Title"].HeaderText = "ชื่อโปรโมชั่น";
                        dgvPromotions.Columns["Description"].HeaderText = "รายละเอียด";
                        dgvPromotions.Columns["Discount"].HeaderText = "ส่วนลด";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("ดึงโปรโมชั่นไม่ได้: " + ex.Message);
            }
        }

        // 3. ฟังก์ชันดึงรีวิวทั้งหมดของร้าน
        private async System.Threading.Tasks.Task LoadReviews()
        {
            try
            {
                var response = await Program.ApiClient.GetAsync($"api/Reviews/restaurant/{_restaurantId}");
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    var reviews = JsonSerializer.Deserialize<List<ReviewDTO>>(json);

                    // เพื่อให้โชว์ข้อมูลสวยๆ เราจะแปลงข้อมูลนิดหน่อยก่อนใส่ตาราง
                    var displayData = reviews.Select(r => new {
                        วันที่ = r.CreatedAt.ToString("dd/MM/yyyy"),
                        ลูกค้า = r.Customer?.Name ?? "ไม่ประสงค์ออกนาม",
                        คะแนน = $"{r.Rating} ⭐",
                        ความคิดเห็น = r.Comment
                    }).ToList();

                    dgvReviews.DataSource = displayData;

                    // 1. ซ่อนแถบลูกศรด้านซ้ายสุด (เพื่อเพิ่มพื้นที่ให้ข้อความ)
                    dgvReviews.RowHeadersVisible = false;

                    // 2. ปิดไม่ให้ลูกค้าพิมพ์แก้ไขข้อมูลในตารางเล่นได้
                    dgvReviews.ReadOnly = true;
                    dgvReviews.AllowUserToAddRows = false;

                    // 3. ปรับให้เลือกข้อมูลทั้งแถวเสมอ
                    dgvReviews.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

                    // 4. สลับสีพื้นหลังแถวเว้นแถว ให้อ่านง่ายสบายตาขึ้น
                    dgvReviews.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(250, 251, 248);
                    dgvReviews.BackgroundColor = CustomerUiTheme.Surface;

                    // 5. ไฮไลต์สำคัญ: จัดการคอลัมน์ "ความคิดเห็น" ไม่ให้ตกขอบ
                    if (dgvReviews.Columns.Contains("ความคิดเห็น"))
                    {
                        dgvReviews.Columns["ความคิดเห็น"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        dgvReviews.Columns["ความคิดเห็น"].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                    }

                    // 6. สั่งให้ความสูงของแต่ละแถว ยืดหดตามความยาวของข้อความรีวิวแบบอัตโนมัติ
                    dgvReviews.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

                    // จัดคอลัมน์อื่นๆ ให้พอดีกับเนื้อหา
                    if (dgvReviews.Columns.Contains("วันที่")) dgvReviews.Columns["วันที่"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    if (dgvReviews.Columns.Contains("คะแนน")) dgvReviews.Columns["คะแนน"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                }
            }
            catch { }
        }
        // ฟังก์ชันอัปเดตช่อง Dropdown เลือกคูปอง
        private void UpdateCouponDropdown()
        {
            // ดึงเฉพาะคูปองที่เป็นของร้านนี้ออกมาจาก "กระเป๋า Static" ใน Program.cs
            var myCouponsForThisRest = Program.ClaimedCoupons.Where(c => c.RestaurantId == _restaurantId).ToList();

            // สร้างตัวเลือกเริ่มต้น
            var displayList = new List<PromotionDTO>();
            displayList.Add(new PromotionDTO { PromotionId = 0, Title = "-- ไม่ใช้คูปอง --" });
            displayList.AddRange(myCouponsForThisRest);

            // ยัดใส่ ComboBox
            cmbCoupons.DataSource = null;
            cmbCoupons.DataSource = displayList;
            cmbCoupons.DisplayMember = "Title";
            cmbCoupons.ValueMember = "PromotionId";
        }

        // เหตุการณ์ตอนกดปุ่ม "กดรับคูปอง"
        private void btnClaimCoupon_Click(object sender, EventArgs e)
        {
            if (dgvPromotions.SelectedRows.Count > 0)
            {
                // ดึงข้อมูลโปรโมชั่นจากแถวที่เลือก
                var selectedRow = dgvPromotions.SelectedRows[0];
                var promo = (PromotionDTO)selectedRow.DataBoundItem;

                // เช็คก่อนว่ามีในกระเป๋าหรือยัง (กันกดซ้ำ)
                bool alreadyClaimed = Program.ClaimedCoupons.Any(c => c.PromotionId == promo.PromotionId);

                if (!alreadyClaimed)
                {
                    Program.ClaimedCoupons.Add(promo); // เพิ่มเข้ากระเป๋า Static
                    MessageBox.Show($"เก็บคูปอง '{promo.Title}' เรียบร้อย!", "สำเร็จ");
                    UpdateCouponDropdown(); // รีเฟรชช่องจองโต๊ะทันที
                }
                else
                {
                    MessageBox.Show("กดรับคูปองนี้ไปแล้ว");
                }
            }
            else
            {
                MessageBox.Show("กรุณาเลือกคูปองที่ต้องการจากตาราง");
            }
        }

        private async void btnBook_Click(object sender, EventArgs e)
        {
            // เตรียมข้อมูลส่งไป API
            var newBooking = new
            {
                customerId = Program.LoggedInCustomerId,
                restaurantId = _restaurantId,
                bookingDate = dtpBookingDate.Value.ToString("yyyy-MM-ddTHH:mm:ss", System.Globalization.CultureInfo.InvariantCulture),
                numPeople = (int)numPeople.Value
            };

            string jsonContent = JsonSerializer.Serialize(newBooking);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            try
            {
                var response = await Program.ApiClient.PostAsync("api/Bookings", content);

                if (response.IsSuccessStatusCode)
                {
                    // เช็คว่าเลือกคูปองไหม เพื่อโชว์ใน MessageBox
                    var selectedPromo = cmbCoupons.SelectedItem as PromotionDTO;
                    string promoMsg = (selectedPromo != null && selectedPromo.PromotionId != 0)
                                      ? $"\n(ใช้คูปอง: {selectedPromo.Title})"
                                      : "";

                    MessageBox.Show($"ส่งคำขอจองโต๊ะสำเร็จแล้ว กรุณารอร้านยืนยัน{promoMsg}", "สำเร็จ");
                    this.Close(); // ปิดหน้านี้ กลับไปหน้า Home
                }
                else
                {
                    MessageBox.Show("ไม่สามารถจองโต๊ะได้ในช่วงเวลานี้\n(สาเหตุ: โต๊ะอาจจะเต็ม หรือ อยู่นอกเวลาเปิด-ปิดของร้าน)", "เกิดข้อผิดพลาดในการจอง");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("เชื่อมต่อ API ไม่ได้: " + ex.Message);
            }
        }

        private void label1_Click(object sender, EventArgs e) { }
        private void label2_Click(object sender, EventArgs e) { }
        private void label6_Click(object sender, EventArgs e) { }
    }
}
