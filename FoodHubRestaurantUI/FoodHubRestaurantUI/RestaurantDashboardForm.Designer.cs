namespace FoodHubRestaurantUI
{
    partial class RestaurantDashboardForm
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
            menuStrip1 = new MenuStrip();
            menuDashboard = new ToolStripMenuItem();
            menuPromotions = new ToolStripMenuItem();
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            btnReject = new Button();
            btnConfirm = new Button();
            dgvPending = new DataGridView();
            tabPage2 = new TabPage();
            btnNoShow = new Button();
            btnComplete = new Button();
            dgvConfirmed = new DataGridView();
            btnRefresh = new Button();
            btnLogout = new ToolStripMenuItem();
            menuStrip1.SuspendLayout();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvPending).BeginInit();
            tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvConfirmed).BeginInit();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(20, 20);
            menuStrip1.Items.AddRange(new ToolStripItem[] { menuDashboard, menuPromotions, btnLogout });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(800, 28);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // menuDashboard
            // 
            menuDashboard.Name = "menuDashboard";
            menuDashboard.Size = new Size(100, 24);
            menuDashboard.Text = "จัดการคิวจอง";
            // 
            // menuPromotions
            // 
            menuPromotions.Name = "menuPromotions";
            menuPromotions.Size = new Size(115, 24);
            menuPromotions.Text = "จัดการโปรโมชั่น";
            menuPromotions.Click += menuPromotions_Click;
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Location = new Point(12, 31);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(788, 407);
            tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(btnReject);
            tabPage1.Controls.Add(btnConfirm);
            tabPage1.Controls.Add(dgvPending);
            tabPage1.Location = new Point(4, 29);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(780, 374);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "รอการยืนยัน (Pending)";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // btnReject
            // 
            btnReject.Location = new Point(603, 41);
            btnReject.Name = "btnReject";
            btnReject.Size = new Size(169, 29);
            btnReject.TabIndex = 2;
            btnReject.Text = "ปฏิเสธ";
            btnReject.UseVisualStyleBackColor = true;
            btnReject.Click += btnReject_Click;
            // 
            // btnConfirm
            // 
            btnConfirm.Location = new Point(603, 6);
            btnConfirm.Name = "btnConfirm";
            btnConfirm.Size = new Size(169, 29);
            btnConfirm.TabIndex = 1;
            btnConfirm.Text = "รับคิว";
            btnConfirm.UseVisualStyleBackColor = true;
            btnConfirm.Click += btnConfirm_Click;
            // 
            // dgvPending
            // 
            dgvPending.BackgroundColor = SystemColors.ButtonFace;
            dgvPending.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvPending.Location = new Point(0, 0);
            dgvPending.Name = "dgvPending";
            dgvPending.RowHeadersWidth = 51;
            dgvPending.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPending.Size = new Size(597, 374);
            dgvPending.TabIndex = 0;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(btnNoShow);
            tabPage2.Controls.Add(btnComplete);
            tabPage2.Controls.Add(dgvConfirmed);
            tabPage2.Location = new Point(4, 29);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(780, 374);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "คิวที่รับแล้ว (Confirmed)";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // btnNoShow
            // 
            btnNoShow.Location = new Point(602, 41);
            btnNoShow.Name = "btnNoShow";
            btnNoShow.Size = new Size(170, 29);
            btnNoShow.TabIndex = 2;
            btnNoShow.Text = "ลูกค้าไม่มา";
            btnNoShow.UseVisualStyleBackColor = true;
            btnNoShow.Click += btnNoShow_Click;
            // 
            // btnComplete
            // 
            btnComplete.Location = new Point(602, 6);
            btnComplete.Name = "btnComplete";
            btnComplete.Size = new Size(170, 29);
            btnComplete.TabIndex = 1;
            btnComplete.Text = "เช็คอิน / ลูกค้าทานเสร็จแล้ว";
            btnComplete.UseVisualStyleBackColor = true;
            btnComplete.Click += btnComplete_Click;
            // 
            // dgvConfirmed
            // 
            dgvConfirmed.BackgroundColor = SystemColors.ButtonFace;
            dgvConfirmed.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvConfirmed.Location = new Point(0, 0);
            dgvConfirmed.Name = "dgvConfirmed";
            dgvConfirmed.RowHeadersWidth = 51;
            dgvConfirmed.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvConfirmed.Size = new Size(596, 374);
            dgvConfirmed.TabIndex = 0;
            // 
            // btnRefresh
            // 
            btnRefresh.Location = new Point(611, 31);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(189, 28);
            btnRefresh.TabIndex = 2;
            btnRefresh.Text = "↻";
            btnRefresh.UseVisualStyleBackColor = true;
            btnRefresh.Click += btnRefresh_Click;
            // 
            // btnLogout
            // 
            btnLogout.Name = "btnLogout";
            btnLogout.Size = new Size(101, 24);
            btnLogout.Text = "ออกจากระบบ";
            btnLogout.Click += btnLogout_Click;
            // 
            // RestaurantDashboardForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnRefresh);
            Controls.Add(tabControl1);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "RestaurantDashboardForm";
            Text = "RestaurantDashboardForm";
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvPending).EndInit();
            tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvConfirmed).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem menuDashboard;
        private ToolStripMenuItem menuPromotions;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private DataGridView dgvPending;
        private Button btnReject;
        private Button btnConfirm;
        private Button btnRefresh;
        private DataGridView dgvConfirmed;
        private Button btnComplete;
        private Button btnNoShow;
        private ToolStripMenuItem btnLogout;
    }
}