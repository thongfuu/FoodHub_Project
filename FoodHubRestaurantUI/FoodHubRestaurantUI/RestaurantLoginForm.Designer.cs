namespace FoodHubRestaurantUI
{
    partial class RestaurantLoginForm
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
            cmbRestaurants = new ComboBox();
            btnLogin = new Button();
            SuspendLayout();
            // 
            // cmbRestaurants
            // 
            cmbRestaurants.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbRestaurants.FormattingEnabled = true;
            cmbRestaurants.Location = new Point(319, 148);
            cmbRestaurants.Name = "cmbRestaurants";
            cmbRestaurants.Size = new Size(151, 28);
            cmbRestaurants.TabIndex = 0;
            // 
            // btnLogin
            // 
            btnLogin.Location = new Point(345, 195);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(94, 29);
            btnLogin.TabIndex = 1;
            btnLogin.Text = "เข้าสู่ระบบ";
            btnLogin.UseVisualStyleBackColor = true;
            btnLogin.Click += btnLogin_Click;
            // 
            // RestaurantLoginForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnLogin);
            Controls.Add(cmbRestaurants);
            Name = "RestaurantLoginForm";
            Text = "Form1";
            ResumeLayout(false);
        }

        #endregion

        private ComboBox cmbRestaurants;
        private Button btnLogin;
    }
}
