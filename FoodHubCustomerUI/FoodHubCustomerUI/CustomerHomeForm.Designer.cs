namespace FoodHubCustomerUI
{
    partial class CustomerHomeForm
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
            dgvRestaurants = new DataGridView();
            menuMyBooking = new MenuStrip();
            หนาแรกToolStripMenuItem = new ToolStripMenuItem();
            การจองของฉนToolStripMenuItem = new ToolStripMenuItem();
            txtSearch = new TextBox();
            btnSearch = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvRestaurants).BeginInit();
            menuMyBooking.SuspendLayout();
            SuspendLayout();
            // 
            // dgvRestaurants
            // 
            dgvRestaurants.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvRestaurants.Location = new Point(12, 75);
            dgvRestaurants.Name = "dgvRestaurants";
            dgvRestaurants.RowHeadersWidth = 51;
            dgvRestaurants.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvRestaurants.Size = new Size(776, 363);
            dgvRestaurants.TabIndex = 0;
            dgvRestaurants.CellDoubleClick += dgvRestaurants_CellDoubleClick;
            // 
            // menuMyBooking
            // 
            menuMyBooking.ImageScalingSize = new Size(20, 20);
            menuMyBooking.Items.AddRange(new ToolStripItem[] { หนาแรกToolStripMenuItem, การจองของฉนToolStripMenuItem });
            menuMyBooking.Location = new Point(0, 0);
            menuMyBooking.Name = "menuMyBooking";
            menuMyBooking.Size = new Size(800, 28);
            menuMyBooking.TabIndex = 1;
            menuMyBooking.Text = "menuStrip1";
            // 
            // หนาแรกToolStripMenuItem
            // 
            หนาแรกToolStripMenuItem.Name = "หนาแรกToolStripMenuItem";
            หนาแรกToolStripMenuItem.Size = new Size(70, 24);
            หนาแรกToolStripMenuItem.Text = "หน้าแรก";
            หนาแรกToolStripMenuItem.Click += menuHome_Click;
            // 
            // การจองของฉนToolStripMenuItem
            // 
            การจองของฉนToolStripMenuItem.Name = "การจองของฉนToolStripMenuItem";
            การจองของฉนToolStripMenuItem.Size = new Size(108, 24);
            การจองของฉนToolStripMenuItem.Text = "การจองของฉัน";
            การจองของฉนToolStripMenuItem.Click += menuMyBookings_Click;
            // 
            // txtSearch
            // 
            txtSearch.Location = new Point(12, 31);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(676, 27);
            txtSearch.TabIndex = 2;
            // 
            // btnSearch
            // 
            btnSearch.Location = new Point(694, 29);
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new Size(94, 29);
            btnSearch.TabIndex = 3;
            btnSearch.Text = "Search";
            btnSearch.UseVisualStyleBackColor = true;
            btnSearch.Click += btnSearch_Click;
            // 
            // CustomerHomeForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnSearch);
            Controls.Add(txtSearch);
            Controls.Add(dgvRestaurants);
            Controls.Add(menuMyBooking);
            MainMenuStrip = menuMyBooking;
            Name = "CustomerHomeForm";
            Text = "CustomerHomeForm";
            ((System.ComponentModel.ISupportInitialize)dgvRestaurants).EndInit();
            menuMyBooking.ResumeLayout(false);
            menuMyBooking.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dgvRestaurants;
        private MenuStrip menuMyBooking;
        private ToolStripMenuItem หนาแรกToolStripMenuItem;
        private ToolStripMenuItem การจองของฉนToolStripMenuItem;
        private TextBox txtSearch;
        private Button btnSearch;
    }
}