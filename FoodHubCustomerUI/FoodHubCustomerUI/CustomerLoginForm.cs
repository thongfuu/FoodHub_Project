using FoodHubCustomerUI.Models;
using System.Text.Json;
namespace FoodHubCustomerUI
{
    public partial class CustomerLoginForm : Form
    {
        public CustomerLoginForm()
        {
            InitializeComponent();
            ApplyModernLayout();
        }

        private void ApplyModernLayout()
        {
            CustomerUiTheme.ApplyForm(this, new Size(880, 540), "FoodHub - เข้าสู่ระบบลูกค้า");

            var title = CustomerUiTheme.CreateTitle("FoodHub", new Point(70, 78), 360);
            var subtitle = CustomerUiTheme.CreateSubtitle("เลือกลูกค้าเพื่อเริ่มค้นหาร้านอาหารและจองโต๊ะ", new Point(72, 124), 430);
            var card = CustomerUiTheme.CreateCard("loginCard", new Rectangle(500, 86, 300, 330));
            card.Paint += CustomerUiTheme.PaintCard;

            var cardTitle = CustomerUiTheme.CreateTitle("เข้าสู่ระบบ", new Point(24, 28), 240);
            cardTitle.Font = CustomerUiTheme.SectionFont;
            var customerLabel = CustomerUiTheme.CreateSubtitle("เลือกชื่อลูกค้า", new Point(24, 92), 220);

            Controls.Remove(cmbCustomers);
            Controls.Remove(btnLogin);

            cmbCustomers.Location = new Point(24, 126);
            cmbCustomers.Size = new Size(252, 32);
            cmbCustomers.DropDownStyle = ComboBoxStyle.DropDownList;
            CustomerUiTheme.StyleInput(cmbCustomers);

            btnLogin.Location = new Point(24, 198);
            btnLogin.Size = new Size(252, 44);
            btnLogin.Text = "เข้าสู่ระบบ";
            CustomerUiTheme.StylePrimaryButton(btnLogin);

            card.Controls.Add(cardTitle);
            card.Controls.Add(customerLabel);
            card.Controls.Add(cmbCustomers);
            card.Controls.Add(btnLogin);
            Controls.Add(title);
            Controls.Add(subtitle);
            Controls.Add(card);
        }

        protected override async void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            try
            {
                var response = await Program.ApiClient.GetAsync("api/Customers");
                response.EnsureSuccessStatusCode();
                string json = await response.Content.ReadAsStringAsync();
                var customers = JsonSerializer.Deserialize<List<CustomerDTO>>(json);
                cmbCustomers.DataSource = customers;
                cmbCustomers.DisplayMember = "Name";
                cmbCustomers.ValueMember = "CustomerId";
            }
            catch (Exception ex) { MessageBox.Show("ต่อ API ไม่ได้: " + ex.Message); }
        }
        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (cmbCustomers.SelectedItem is CustomerDTO selectedCustomer)
            {
                Program.LoggedInCustomerId = selectedCustomer.CustomerId;
                Program.LoggedInCustomerName = selectedCustomer.Name;
                // MessageBox.Show($"ยินดีต้อนรับ {Program.LoggedInCustomerName}");
                var homeForm = new CustomerHomeForm();
                homeForm.Show();
                this.Hide();
            }
        }
    }
}
