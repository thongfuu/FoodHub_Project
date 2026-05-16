using System.Text;
using System.Text.Json;

namespace FoodHubCustomerUI
{
    public partial class ReviewDialogForm : Form
    {
        private int _restId;

        public ReviewDialogForm(int restId, string restName)
        {
            InitializeComponent();
            _restId = restId;

            // 1. เปลี่ยนชื่อบนหัวหน้าต่าง
            this.Text = "เขียนรีวิวให้ " + restName;

            // 2. 🌟 เพิ่มบรรทัดนี้: เอาชื่อร้านไปโชว์ที่ Label ในหน้าจอ 🌟
            lblRestaurantName.Text = $"ให้คะแนนร้าน: {restName}";
        }

        private async void btnSubmit_Click(object sender, EventArgs e)
        {
            var reviewData = new
            {
                customerId = Program.LoggedInCustomerId,
                restaurantId = _restId,
                rating = (int)numRating.Value,
                comment = txtComment.Text
            };

            var content = new StringContent(JsonSerializer.Serialize(reviewData), Encoding.UTF8, "application/json");
            var resp = await Program.ApiClient.PostAsync("api/Reviews", content);

            if (resp.IsSuccessStatusCode)
            {
                MessageBox.Show("ขอบคุณสำหรับการรีวิว");
                this.DialogResult = DialogResult.OK; // บอกหน้าแม่ว่าทำเสร็จแล้ว
                this.Close();
            }
        }
    }
}