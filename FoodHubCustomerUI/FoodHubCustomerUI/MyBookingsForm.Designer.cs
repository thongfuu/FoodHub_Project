namespace FoodHubCustomerUI
{
    partial class MyBookingsForm
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
            dgvBookings = new DataGridView();
            label1 = new Label();
            btnCancel = new Button();
            btnReview = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvBookings).BeginInit();
            SuspendLayout();
            // 
            // dgvBookings
            // 
            dgvBookings.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvBookings.Location = new Point(12, 40);
            dgvBookings.Name = "dgvBookings";
            dgvBookings.RowHeadersWidth = 51;
            dgvBookings.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvBookings.Size = new Size(776, 363);
            dgvBookings.TabIndex = 0;
            dgvBookings.CellFormatting += dgvBookings_CellFormatting;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(166, 28);
            label1.TabIndex = 1;
            label1.Text = "Booking History";
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(12, 409);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(381, 29);
            btnCancel.TabIndex = 2;
            btnCancel.Text = "ยกเลิกการจอง";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // btnReview
            // 
            btnReview.Location = new Point(399, 409);
            btnReview.Name = "btnReview";
            btnReview.Size = new Size(389, 29);
            btnReview.TabIndex = 3;
            btnReview.Text = "เขียนรีวิว";
            btnReview.UseVisualStyleBackColor = true;
            btnReview.Click += btnReview_Click;
            // 
            // MyBookingsForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnReview);
            Controls.Add(btnCancel);
            Controls.Add(label1);
            Controls.Add(dgvBookings);
            Name = "MyBookingsForm";
            Text = "MyBookingsForm";
            ((System.ComponentModel.ISupportInitialize)dgvBookings).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dgvBookings;
        private Label label1;
        private Button btnCancel;
        private Button btnReview;
    }
}