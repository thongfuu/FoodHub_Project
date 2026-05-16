namespace FoodHubCustomerUI
{
    partial class CustomerLoginForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            cmbCustomers = new ComboBox();
            btnLogin = new Button();
            SuspendLayout();
            // 
            // cmbCustomers
            // 
            cmbCustomers.FormattingEnabled = true;
            cmbCustomers.Location = new Point(316, 140);
            cmbCustomers.Name = "cmbCustomers";
            cmbCustomers.Size = new Size(151, 28);
            cmbCustomers.TabIndex = 0;
            // 
            // btnLogin
            // 
            btnLogin.Location = new Point(345, 203);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(94, 29);
            btnLogin.TabIndex = 1;
            btnLogin.Text = "เข้าสู่่ระบบ";
            btnLogin.UseVisualStyleBackColor = true;
            btnLogin.Click += btnLogin_Click;
            // 
            // CustomerLoginForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnLogin);
            Controls.Add(cmbCustomers);
            Name = "CustomerLoginForm";
            Text = "Form1";
            ResumeLayout(false);
        }

        #endregion

        private ComboBox cmbCustomers;
        private Button btnLogin;
    }
}
