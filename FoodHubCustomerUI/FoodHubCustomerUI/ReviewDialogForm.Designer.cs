namespace FoodHubCustomerUI
{
    partial class ReviewDialogForm
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
            numRating = new NumericUpDown();
            txtComment = new TextBox();
            label2 = new Label();
            btnSubmit = new Button();
            ((System.ComponentModel.ISupportInitialize)numRating).BeginInit();
            SuspendLayout();
            // 
            // lblRestaurantName
            // 
            lblRestaurantName.AutoSize = true;
            lblRestaurantName.Location = new Point(12, 9);
            lblRestaurantName.Name = "lblRestaurantName";
            lblRestaurantName.Size = new Size(88, 20);
            lblRestaurantName.TabIndex = 0;
            lblRestaurantName.Text = "ให้คะแนนร้าน:";
            // 
            // numRating
            // 
            numRating.Location = new Point(242, 7);
            numRating.Maximum = new decimal(new int[] { 5, 0, 0, 0 });
            numRating.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numRating.Name = "numRating";
            numRating.Size = new Size(150, 27);
            numRating.TabIndex = 1;
            numRating.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // txtComment
            // 
            txtComment.Location = new Point(12, 65);
            txtComment.Multiline = true;
            txtComment.Name = "txtComment";
            txtComment.Size = new Size(380, 338);
            txtComment.TabIndex = 2;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 42);
            label2.Name = "label2";
            label2.Size = new Size(59, 20);
            label2.TabIndex = 3;
            label2.Text = "เขียนรีวิว";
            // 
            // btnSubmit
            // 
            btnSubmit.Location = new Point(12, 409);
            btnSubmit.Name = "btnSubmit";
            btnSubmit.Size = new Size(380, 29);
            btnSubmit.TabIndex = 4;
            btnSubmit.Text = "ส่งรีวิว";
            btnSubmit.UseVisualStyleBackColor = true;
            btnSubmit.Click += btnSubmit_Click;
            // 
            // ReviewDialogForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(407, 450);
            Controls.Add(btnSubmit);
            Controls.Add(label2);
            Controls.Add(txtComment);
            Controls.Add(numRating);
            Controls.Add(lblRestaurantName);
            Name = "ReviewDialogForm";
            Text = "ReviewDialogForm";
            ((System.ComponentModel.ISupportInitialize)numRating).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblRestaurantName;
        private NumericUpDown numRating;
        private TextBox txtComment;
        private Label label2;
        private Button btnSubmit;
    }
}