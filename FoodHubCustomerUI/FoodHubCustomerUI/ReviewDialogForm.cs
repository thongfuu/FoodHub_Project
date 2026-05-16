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
            ApplyModernLayout();

            // 1. เปลี่ยนชื่อบนหัวหน้าต่าง
            this.Text = "เขียนรีวิวให้ " + restName;

            // 2. 🌟 เพิ่มบรรทัดนี้: เอาชื่อร้านไปโชว์ที่ Label ในหน้าจอ 🌟
            lblRestaurantName.Text = $"ให้คะแนนร้าน: {restName}";
        }

        private void ApplyModernLayout()
        {
            CustomerUiTheme.ApplyForm(this, new Size(520, 560), "FoodHub - เขียนรีวิว");
            MinimumSize = new Size(520, 560);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;

            lblRestaurantName.Location = new Point(32, 28);
            lblRestaurantName.Size = new Size(456, 56);
            lblRestaurantName.AutoSize = false;
            lblRestaurantName.Font = CustomerUiTheme.SectionFont;
            lblRestaurantName.ForeColor = CustomerUiTheme.Text;

            var ratingLabel = CustomerUiTheme.CreateSubtitle("คะแนน", new Point(32, 104), 100);
            Controls.Add(ratingLabel);

            numRating.Location = new Point(132, 100);
            numRating.Size = new Size(356, 30);
            CustomerUiTheme.StyleInput(numRating);

            label2.Text = "เล่าประสบการณ์ของคุณ";
            label2.Location = new Point(32, 154);
            label2.Size = new Size(456, 28);
            label2.AutoSize = false;
            CustomerUiTheme.StyleLabel(label2);

            txtComment.Location = new Point(32, 190);
            txtComment.Size = new Size(456, 260);
            txtComment.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            CustomerUiTheme.StyleInput(txtComment);

            btnSubmit.Location = new Point(32, 476);
            btnSubmit.Size = new Size(456, 44);
            btnSubmit.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            CustomerUiTheme.StylePrimaryButton(btnSubmit);
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
