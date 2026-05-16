namespace FoodHubRestaurantUI
{
    partial class PromotionManagementForm
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
            dgvPromotions = new DataGridView();
            label1 = new Label();
            groupBox1 = new GroupBox();
            btnPublish = new Button();
            dtpExpireDate = new DateTimePicker();
            label6 = new Label();
            label5 = new Label();
            label4 = new Label();
            numDiscount = new NumericUpDown();
            label3 = new Label();
            label2 = new Label();
            txtDescription = new TextBox();
            txtTitle = new TextBox();
            ((System.ComponentModel.ISupportInitialize)dgvPromotions).BeginInit();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numDiscount).BeginInit();
            SuspendLayout();
            // 
            // dgvPromotions
            // 
            dgvPromotions.BackgroundColor = SystemColors.ButtonFace;
            dgvPromotions.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvPromotions.Location = new Point(12, 54);
            dgvPromotions.Name = "dgvPromotions";
            dgvPromotions.RowHeadersWidth = 51;
            dgvPromotions.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPromotions.Size = new Size(776, 188);
            dgvPromotions.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(12, 12);
            label1.Name = "label1";
            label1.Size = new Size(145, 28);
            label1.TabIndex = 1;
            label1.Text = "โปรโมชั่นที่มีอยู่";
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(btnPublish);
            groupBox1.Controls.Add(dtpExpireDate);
            groupBox1.Controls.Add(label6);
            groupBox1.Controls.Add(label5);
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(numDiscount);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(txtDescription);
            groupBox1.Controls.Add(txtTitle);
            groupBox1.Location = new Point(12, 248);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(776, 190);
            groupBox1.TabIndex = 2;
            groupBox1.TabStop = false;
            groupBox1.Text = "ฟอร์มสร้างโปรโมชั่นใหม่";
            groupBox1.Enter += groupBox1_Enter;
            // 
            // btnPublish
            // 
            btnPublish.Location = new Point(426, 109);
            btnPublish.Name = "btnPublish";
            btnPublish.Size = new Size(264, 29);
            btnPublish.TabIndex = 9;
            btnPublish.Text = "เผยแพร่โปรโมชั่น";
            btnPublish.UseVisualStyleBackColor = true;
            btnPublish.Click += btnPublish_Click;
            // 
            // dtpExpireDate
            // 
            dtpExpireDate.Format = DateTimePickerFormat.Short;
            dtpExpireDate.Location = new Point(506, 76);
            dtpExpireDate.Name = "dtpExpireDate";
            dtpExpireDate.Size = new Size(184, 27);
            dtpExpireDate.TabIndex = 8;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(426, 81);
            label6.Name = "label6";
            label6.Size = new Size(77, 20);
            label6.TabIndex = 7;
            label6.Text = "วันหมดอายุ:";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(669, 45);
            label5.Name = "label5";
            label5.Size = new Size(21, 20);
            label5.TabIndex = 6;
            label5.Text = "%";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(426, 45);
            label4.Name = "label4";
            label4.Size = new Size(53, 20);
            label4.TabIndex = 5;
            label4.Text = "ส่วนลด:";
            label4.Click += label4_Click;
            // 
            // numDiscount
            // 
            numDiscount.DecimalPlaces = 2;
            numDiscount.Increment = new decimal(new int[] { 5, 0, 0, 131072 });
            numDiscount.Location = new Point(485, 43);
            numDiscount.Name = "numDiscount";
            numDiscount.Size = new Size(175, 27);
            numDiscount.TabIndex = 4;
            numDiscount.ValueChanged += numericUpDown1_ValueChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(7, 74);
            label3.Name = "label3";
            label3.Size = new Size(88, 20);
            label3.TabIndex = 3;
            label3.Text = "Description:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(6, 45);
            label2.Name = "label2";
            label2.Size = new Size(41, 20);
            label2.TabIndex = 2;
            label2.Text = "Title:";
            // 
            // txtDescription
            // 
            txtDescription.Location = new Point(9, 97);
            txtDescription.Multiline = true;
            txtDescription.Name = "txtDescription";
            txtDescription.Size = new Size(411, 87);
            txtDescription.TabIndex = 1;
            // 
            // txtTitle
            // 
            txtTitle.Location = new Point(53, 42);
            txtTitle.Name = "txtTitle";
            txtTitle.Size = new Size(367, 27);
            txtTitle.TabIndex = 0;
            // 
            // PromotionManagementForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(groupBox1);
            Controls.Add(label1);
            Controls.Add(dgvPromotions);
            Name = "PromotionManagementForm";
            Text = "PromotionManagementForm";
            ((System.ComponentModel.ISupportInitialize)dgvPromotions).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numDiscount).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dgvPromotions;
        private Label label1;
        private GroupBox groupBox1;
        private TextBox txtTitle;
        private Label label2;
        private TextBox txtDescription;
        private Label label3;
        private Label label4;
        private NumericUpDown numDiscount;
        private Label label6;
        private Label label5;
        private DateTimePicker dtpExpireDate;
        private Button btnPublish;
    }
}