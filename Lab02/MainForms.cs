using System;
using System.Drawing;
using System.Windows.Forms;

namespace Lab02
{
    internal sealed class FrmMain : Form
    {
        public FrmMain()
        {
            Text = "Lab 02 - File and I/O Stream";
            StartPosition = FormStartPosition.CenterScreen;
            MinimumSize = new Size(640, 500);
            Size = new Size(760, 590);
            BuildUi();
        }

        private void BuildUi()
        {
            var title = new Label { Text = "LAB 02 — FILE AND I/O STREAM", Dock = DockStyle.Top, Height = 72,
                TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Segoe UI", 18, FontStyle.Bold) };
            Controls.Add(title);
            var subtitle = new Label { Text = "Chọn bài thực hành để mở", Dock = DockStyle.Top, Height = 35,
                TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Segoe UI", 10) };
            Controls.Add(subtitle);
            var panel = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 4, Padding = new Padding(24) };
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            for (int i = 0; i < 4; i++) panel.RowStyles.Add(new RowStyle(SizeType.Percent, 25));
            string[] labels = { "Bài 01 - Đọc và chuyển chữ hoa", "Bài 02 - Thông tin file", "Bài 03 - Tính biểu thức",
                "Bài 04 - Quản lý sinh viên", "Bài 05 - Quản lý phòng vé", "Bài 06 - Hôm nay ăn gì?", "Bài 07 - Duyệt file/thư mục" };
            Action[] actions = {
                () => new FrmBai01().ShowDialog(this), () => new FrmBai02().ShowDialog(this),
                () => new FrmBai03().ShowDialog(this), () => new FrmBai04().ShowDialog(this),
                () => new FrmBai05().ShowDialog(this), () => new FrmBai06().ShowDialog(this),
                () => new FrmBai07().ShowDialog(this)
            };
            for (int i = 0; i < labels.Length; i++)
            {
                int idx = i;
                var button = new Button { Text = labels[i], Dock = DockStyle.Fill, Margin = new Padding(8),
                    Font = new Font("Segoe UI", 10), UseVisualStyleBackColor = true };
                button.Click += (s, e) => actions[idx]();
                panel.Controls.Add(button, i % 2, i / 2);
            }
            Controls.Add(panel);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // FrmMain
            // 
            this.ClientSize = new System.Drawing.Size(284, 298);
            this.Name = "FrmMain";
            this.ResumeLayout(false);

        }
    }
}
