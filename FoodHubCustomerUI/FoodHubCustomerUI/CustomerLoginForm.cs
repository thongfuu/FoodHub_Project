using FoodHubCustomerUI.Models;
using System.Text.Json;
namespace FoodHubCustomerUI
{
    public partial class CustomerLoginForm : Form
    {
        public CustomerLoginForm() { InitializeComponent(); }
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
