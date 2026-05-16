namespace FoodHubCustomerUI
{
    partial class RestaurantDetailForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            lblRestaurantName = new Label();
            dgvReviews = new DataGridView();
            btnClaimCoupon = new Button();
            dtpBookingDate = new DateTimePicker();
            numPeople = new NumericUpDown();
            cmbCoupons = new ComboBox();
            btnBook = new Button();
            label1 = new Label();
            label2 = new Label();
            lblOpenHours = new Label();
            label4 = new Label();
            lblAddress = new Label();
            dgvPromotions = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)dgvReviews).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numPeople).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvPromotions).BeginInit();
            SuspendLayout();
            // 
            // lblRestaurantName
            // 
            lblRestaurantName.AutoSize = true;
            lblRestaurantName.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblRestaurantName.Location = new Point(12, 23);
            lblRestaurantName.Name = "lblRestaurantName";
            lblRestaurantName.Size = new Size(79, 28);
            lblRestaurantName.TabIndex = 0;
            lblRestaurantName.Text = "ชื่อร้าน:";
            // 
            // dgvReviews
            // 
            dgvReviews.BackgroundColor = SystemColors.ButtonFace;
            dgvReviews.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvReviews.Location = new Point(12, 129);
            dgvReviews.Name = "dgvReviews";
            dgvReviews.ReadOnly = true;
            dgvReviews.RowHeadersWidth = 51;
            dgvReviews.Size = new Size(550, 309);
            dgvReviews.TabIndex = 1;
            // 
            // btnClaimCoupon
            // 
            btnClaimCoupon.Location = new Point(568, 240);
            btnClaimCoupon.Name = "btnClaimCoupon";
            btnClaimCoupon.Size = new Size(220, 29);
            btnClaimCoupon.TabIndex = 3;
            btnClaimCoupon.Text = "กดรับคูปอง";
            btnClaimCoupon.UseVisualStyleBackColor = true;
            btnClaimCoupon.Click += btnClaimCoupon_Click;
            // 
            // dtpBookingDate
            // 
            dtpBookingDate.CustomFormat = "dd/MM/yyyy HH:mm";
            dtpBookingDate.Format = DateTimePickerFormat.Custom;
            dtpBookingDate.Location = new Point(568, 309);
            dtpBookingDate.Name = "dtpBookingDate";
            dtpBookingDate.Size = new Size(220, 27);
            dtpBookingDate.TabIndex = 4;
            // 
            // numPeople
            // 
            numPeople.Location = new Point(644, 342);
            numPeople.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numPeople.Name = "numPeople";
            numPeople.Size = new Size(144, 27);
            numPeople.TabIndex = 5;
            numPeople.Value = new decimal(new int[] { 2, 0, 0, 0 });
            // 
            // cmbCoupons
            // 
            cmbCoupons.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCoupons.FormattingEnabled = true;
            cmbCoupons.Location = new Point(568, 375);
            cmbCoupons.Name = "cmbCoupons";
            cmbCoupons.Size = new Size(220, 28);
            cmbCoupons.TabIndex = 6;
            // 
            // btnBook
            // 
            btnBook.Location = new Point(568, 409);
            btnBook.Name = "btnBook";
            btnBook.Size = new Size(220, 29);
            btnBook.TabIndex = 7;
            btnBook.Text = "ยืนยันการจอง";
            btnBook.UseVisualStyleBackColor = true;
            btnBook.Click += btnBook_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(568, 286);
            label1.Name = "label1";
            label1.Size = new Size(131, 20);
            label1.TabIndex = 8;
            label1.Text = "เลือกวันที่และเวลาจอง";
            label1.Click += label1_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(568, 344);
            label2.Name = "label2";
            label2.Size = new Size(70, 20);
            label2.TabIndex = 9;
            label2.Text = "จำนวนคน:";
            label2.Click += label2_Click;
            // 
            // lblOpenHours
            // 
            lblOpenHours.AutoSize = true;
            lblOpenHours.Location = new Point(12, 59);
            lblOpenHours.Name = "lblOpenHours";
            lblOpenHours.Size = new Size(84, 20);
            lblOpenHours.TabIndex = 12;
            lblOpenHours.Text = "เวลาเปิด-ปิด:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(568, 23);
            label4.Name = "label4";
            label4.Size = new Size(85, 20);
            label4.TabIndex = 13;
            label4.Text = "Promotions";
            // 
            // lblAddress
            // 
            lblAddress.Location = new Point(13, 92);
            lblAddress.Name = "lblAddress";
            lblAddress.Size = new Size(549, 34);
            lblAddress.TabIndex = 16;
            lblAddress.Text = "ที่อยู่:";
            // 
            // dgvPromotions
            // 
            dgvPromotions.BackgroundColor = SystemColors.ButtonFace;
            dgvPromotions.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvPromotions.Location = new Point(568, 46);
            dgvPromotions.MultiSelect = false;
            dgvPromotions.Name = "dgvPromotions";
            dgvPromotions.RowHeadersWidth = 51;
            dgvPromotions.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPromotions.Size = new Size(220, 188);
            dgvPromotions.TabIndex = 18;
            // 
            // RestaurantDetailForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(dgvPromotions);
            Controls.Add(lblAddress);
            Controls.Add(label4);
            Controls.Add(lblOpenHours);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(btnBook);
            Controls.Add(cmbCoupons);
            Controls.Add(numPeople);
            Controls.Add(dtpBookingDate);
            Controls.Add(btnClaimCoupon);
            Controls.Add(dgvReviews);
            Controls.Add(lblRestaurantName);
            Name = "RestaurantDetailForm";
            Text = "RestaurantDetailForm";
            ((System.ComponentModel.ISupportInitialize)dgvReviews).EndInit();
            ((System.ComponentModel.ISupportInitialize)numPeople).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvPromotions).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblRestaurantName;
        private DataGridView dgvReviews;
        private Button btnClaimCoupon;
        private DateTimePicker dtpBookingDate;
        private NumericUpDown numPeople;
        private ComboBox cmbCoupons;
        private Button btnBook;
        private Label label1;
        private Label label2;
        private Label lblOpenHours;
        private Label label4;
        private Label lblAddress;
        private DataGridView dgvPromotions;
    }
}