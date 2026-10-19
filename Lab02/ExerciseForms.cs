using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Lab02
{
    internal static class Ui
    {
        public static Button Button(string text, EventHandler click = null)
        {
            var b = new Button { Text = text, AutoSize = true, Margin = new Padding(4) };
            if (click != null) b.Click += click;
            return b;
        }
        public static TextBox TextBox(bool multiline = false)
        {
            return new TextBox { Multiline = multiline, ScrollBars = multiline ? ScrollBars.Both : ScrollBars.None,
                Dock = multiline ? DockStyle.Fill : DockStyle.None, WordWrap = false };
        }
        public static void Error(IWin32Window owner, Exception ex) =>
            MessageBox.Show(owner, ex.Message, "Có lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

    internal sealed class FrmBai01 : Form
    {
        private readonly RichTextBox rtb = new RichTextBox { Dock = DockStyle.Fill, Font = new Font("Consolas", 10) };
        public FrmBai01()
        {
            Text = "Bài 01 - Đọc file và chuyển chữ hoa"; Size = new Size(800, 550); StartPosition = FormStartPosition.CenterParent;
            var btn = Ui.Button("Đọc input1.txt và ghi output1.txt", Run);
            var top = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 45 }; top.Controls.Add(btn);
            Controls.Add(rtb); Controls.Add(top);
        }
        private void Run(object sender, EventArgs e)
        {
            try
            {
                string input = AppPaths.PathFor("input1.txt");
                if (!File.Exists(input)) throw new FileNotFoundException("Không tìm thấy input1.txt trong thư mục chạy.", input);
                string content = File.ReadAllText(input, Encoding.UTF8);
                rtb.Text = content;
                File.WriteAllText(AppPaths.PathFor("output1.txt"), content.ToUpper(), new UTF8Encoding(true));
                MessageBox.Show("Đã ghi output1.txt vào thư mục chạy chương trình.");
            }
            catch (Exception ex) { Ui.Error(this, ex); }
        }
    }

    internal sealed class FrmBai02 : Form
    {
        private readonly TextBox pathBox = new TextBox { ReadOnly = true, Dock = DockStyle.Fill };
        private readonly RichTextBox content = new RichTextBox { Dock = DockStyle.Fill };
        private readonly Label info = new Label { Dock = DockStyle.Top, Height = 70, AutoSize = false };
        public FrmBai02()
        {
            Text = "Bài 02 - Thông tin file văn bản"; Size = new Size(850, 600); StartPosition = FormStartPosition.CenterParent;
            var choose = Ui.Button("Chọn file .txt", ChooseFile);
            var top = new TableLayoutPanel { Dock = DockStyle.Top, Height = 90, ColumnCount = 2, RowCount = 2, Padding = new Padding(8) };
            top.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize)); top.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            top.Controls.Add(choose, 0, 0); top.Controls.Add(pathBox, 1, 0); top.Controls.Add(info, 0, 1); top.SetColumnSpan(info, 2);
            Controls.Add(content); Controls.Add(top);
        }
        private void ChooseFile(object sender, EventArgs e)
        {
            using (var dlg = new OpenFileDialog { Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*", Title = "Chọn file văn bản" })
            {
                if (dlg.ShowDialog() != DialogResult.OK) return;
                try
                {
                    var fi = new FileInfo(dlg.FileName);
                    string text = File.ReadAllText(dlg.FileName, Encoding.UTF8);
                    pathBox.Text = dlg.FileName;
                    content.Text = text;
                    int lines = text.Length == 0 ? 0 : text.Replace("\r\n", "\n").Split('\n').Length;
                    int words = text.Split((char[])null, StringSplitOptions.RemoveEmptyEntries).Length;
                    info.Text = "\nTên: " + fi.Name + "\nKích thước: " + fi.Length + " bytes\n" +
                        "Số dòng: " + lines + "\n Số từ: " + words + "\n Số ký tự: " + text.Length;
                }
                catch (Exception ex) { Ui.Error(this, ex); }
            }
        }
    }

    internal sealed class FrmBai03 : Form
    {
        private readonly RichTextBox result = new RichTextBox { Dock = DockStyle.Fill, Font = new Font("Consolas", 10) };
        public FrmBai03()
        {
            Text = "Bài 03 - Tính biểu thức"; Size = new Size(800, 550); StartPosition = FormStartPosition.CenterParent;
            var top = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 45 };
            top.Controls.Add(Ui.Button("Đọc input3.txt, tính và ghi output3.txt", Run));
            Controls.Add(result); Controls.Add(top);
        }
        private void Run(object sender, EventArgs e)
        {
            try
            {
                string input = AppPaths.PathFor("input3.txt");
                if (!File.Exists(input)) throw new FileNotFoundException("Không tìm thấy input3.txt.", input);
                var lines = File.ReadAllLines(input, Encoding.UTF8);
                var output = new List<string>();
                for (int i = 0; i < lines.Length; i++)
                {
                    if (string.IsNullOrWhiteSpace(lines[i])) { output.Add(""); continue; }
                    try
                    {
                        double value = new ExpressionParser(lines[i]).Parse();
                        output.Add(value.ToString("G15", CultureInfo.InvariantCulture));
                    }
                    catch (Exception ex) { output.Add("Lỗi dòng " + (i + 1) + ": " + ex.Message); }
                }
                File.WriteAllLines(AppPaths.PathFor("output3.txt"), output, new UTF8Encoding(true));
                result.Text = string.Join(Environment.NewLine, output);
                MessageBox.Show("Đã tính xong. Xem output3.txt trong thư mục chạy.");
            }
            catch (Exception ex) { Ui.Error(this, ex); }
        }
    }

    internal sealed class Student
    {
        public string Name, Phone;
        public int Id;
        public double S1, S2, S3;
        public double Average => (S1 + S2 + S3) / 3.0;
        public override string ToString() => Id + "|" + Name.Replace("|", " ") + "|" + Phone + "|" +
            S1.ToString(CultureInfo.InvariantCulture) + "|" + S2.ToString(CultureInfo.InvariantCulture) + "|" + S3.ToString(CultureInfo.InvariantCulture);
    }

    internal sealed class FrmBai04 : Form
    {
        private readonly TextBox name = new TextBox(), id = new TextBox(), phone = new TextBox();
        private readonly TextBox s1 = new TextBox(), s2 = new TextBox(), s3 = new TextBox();
        private readonly DataGridView grid = new DataGridView { Dock = DockStyle.Fill, ReadOnly = true, AutoGenerateColumns = true,
            AllowUserToAddRows = false, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };
        private readonly List<Student> students = new List<Student>();
        private int page = 0;
        private const int PageSize = 5;
        private readonly Label pageLabel = new Label { AutoSize = true, Text = "Trang 1" };
        public FrmBai04()
        {
            Text = "Bài 04 - Quản lý sinh viên"; Size = new Size(950, 650); StartPosition = FormStartPosition.CenterParent;
            var form = new TableLayoutPanel { Dock = DockStyle.Top, Height = 120, ColumnCount = 6, RowCount = 2, Padding = new Padding(8) };
            string[] labels = { "Họ tên", "MSSV (8 số)", "Điện thoại (10 số, đầu 0)", "Điểm 1", "Điểm 2", "Điểm 3" };
            TextBox[] boxes = { name, id, phone, s1, s2, s3 };
            for (int i = 0; i < boxes.Length; i++)
            {
                form.Controls.Add(new Label { Text = labels[i], AutoSize = true }, i, 0);
                boxes[i].Dock = DockStyle.Top; form.Controls.Add(boxes[i], i, 1);
            }
            var buttons = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 45, Padding = new Padding(6) };
            buttons.Controls.Add(Ui.Button("Thêm sinh viên", AddStudent));
            buttons.Controls.Add(Ui.Button("Đọc input4.txt", LoadStudents));
            buttons.Controls.Add(Ui.Button("Ghi input4.txt", SaveStudents));
            buttons.Controls.Add(Ui.Button("Xuất output4.txt", ExportAverages));
            buttons.Controls.Add(Ui.Button("◀ Trang trước", (s,e) => { if (page > 0) page--; RefreshGrid(); }));
            buttons.Controls.Add(pageLabel);
            buttons.Controls.Add(Ui.Button("Trang sau ▶", (s,e) => { if ((page + 1) * PageSize < students.Count) page++; RefreshGrid(); }));
            Controls.Add(grid); Controls.Add(buttons); Controls.Add(form);
            grid.DataSource = new DataTable();
        }
        private void AddStudent(object sender, EventArgs e)
        {
            try
            {
                string nm = name.Text.Trim(), sid = id.Text.Trim(), ph = phone.Text.Trim();
                if (nm.Length == 0) throw new Exception("Họ tên không được để trống.");
                if (sid.Length != 8 || !sid.All(char.IsDigit)) throw new Exception("MSSV phải gồm đúng 8 chữ số.");
                if (ph.Length != 10 || ph[0] != '0' || !ph.All(char.IsDigit)) throw new Exception("Số điện thoại phải gồm 10 chữ số và bắt đầu bằng 0.");
                double a = Score(s1.Text), b = Score(s2.Text), c = Score(s3.Text);
                if (students.Any(x => x.Id == int.Parse(sid))) throw new Exception("MSSV đã tồn tại.");
                students.Add(new Student { Name = nm, Id = int.Parse(sid), Phone = ph, S1 = a, S2 = b, S3 = c });
                page = (students.Count - 1) / PageSize; RefreshGrid();
                name.Clear(); id.Clear(); phone.Clear(); s1.Clear(); s2.Clear(); s3.Clear();
            }
            catch (Exception ex) { Ui.Error(this, ex); }
        }
        private double Score(string t)
        {
            double v;
            if (!double.TryParse(t, NumberStyles.Float, CultureInfo.CurrentCulture, out v) &&
                !double.TryParse(t, NumberStyles.Float, CultureInfo.InvariantCulture, out v)) throw new Exception("Điểm phải là số.");
            if (v < 0 || v > 10) throw new Exception("Điểm phải nằm trong khoảng 0 đến 10.");
            return v;
        }
        private void RefreshGrid()
        {
            var dt = new DataTable();
            dt.Columns.Add("MSSV"); dt.Columns.Add("Họ tên"); dt.Columns.Add("Điện thoại");
            dt.Columns.Add("Điểm 1"); dt.Columns.Add("Điểm 2"); dt.Columns.Add("Điểm 3"); dt.Columns.Add("Trung bình");
            foreach (var st in students.Skip(page * PageSize).Take(PageSize))
                dt.Rows.Add(st.Id, st.Name, st.Phone, st.S1, st.S2, st.S3, st.Average.ToString("0.00"));
            grid.DataSource = dt; pageLabel.Text = "Trang " + (page + 1) + "/" + Math.Max(1, (students.Count + PageSize - 1) / PageSize);
        }
        private void SaveStudents(object sender, EventArgs e)
        {
            try { File.WriteAllLines(AppPaths.PathFor("input4.txt"), students.Select(x => x.ToString()), new UTF8Encoding(true)); MessageBox.Show("Đã ghi input4.txt."); }
            catch (Exception ex) { Ui.Error(this, ex); }
        }
        private void LoadStudents(object sender, EventArgs e)
        {
            try
            {
                var path = AppPaths.PathFor("input4.txt");
                if (!File.Exists(path)) throw new FileNotFoundException("Không tìm thấy input4.txt.", path);
                students.Clear();
                foreach (string line in File.ReadAllLines(path, Encoding.UTF8))
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;
                    string[] p = line.Split('|');
                    if (p.Length != 6) continue;
                    int studentId; double a,b,c;
                    if (!int.TryParse(p[0], out studentId) ||
                        !double.TryParse(p[3], NumberStyles.Float, CultureInfo.InvariantCulture, out a) ||
                        !double.TryParse(p[4], NumberStyles.Float, CultureInfo.InvariantCulture, out b) ||
                        !double.TryParse(p[5], NumberStyles.Float, CultureInfo.InvariantCulture, out c)) continue;
                    students.Add(new Student { Id=studentId, Name=p[1], Phone=p[2], S1=a, S2=b, S3=c });
                }
                page = 0; RefreshGrid();
            }
            catch (Exception ex) { Ui.Error(this, ex); }
        }
        private void ExportAverages(object sender, EventArgs e)
        {
            try
            {
                var lines = students.Select(x => x.Id + " | " + x.Name + " | ĐTB: " + x.Average.ToString("0.00", CultureInfo.InvariantCulture)).ToArray();
                File.WriteAllLines(AppPaths.PathFor("output4.txt"), lines, new UTF8Encoding(true));
                MessageBox.Show("Đã xuất output4.txt.");
            }
            catch (Exception ex) { Ui.Error(this, ex); }
        }
    }

    internal sealed class FrmBai05 : Form
    {
        private readonly ComboBox movie = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = 220 };
        private readonly ComboBox room = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = 100 };
        private readonly CheckedListBox seats = new CheckedListBox { Width = 190, Height = 180 };
        private readonly TextBox customer = new TextBox { Width = 220 };
        private readonly Label total = new Label { AutoSize = true, Text = "Tổng tiền: 0 đ" };
        private readonly ProgressBar progress = new ProgressBar { Width = 260, Minimum = 0, Maximum = 100 };
        private readonly Dictionary<string, int> prices = new Dictionary<string, int>
        {
            { "Đào, phở và piano", 45000 }, { "Mai", 100000 }, { "Gặp lại chị bầu", 70000 }, { "Tarot", 90000 }
        };
        private readonly HashSet<string> sold = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        private readonly List<string> sales = new List<string>();
        public FrmBai05()
        {
            Text = "Bài 05 - Quản lý phòng vé"; Size = new Size(850, 620); StartPosition = FormStartPosition.CenterParent;
            movie.Items.AddRange(new object[] { "Đào, phở và piano", "Mai", "Gặp lại chị bầu", "Tarot" }); movie.SelectedIndex = 0;
            room.Items.AddRange(new object[] { "1", "2", "3" }); room.SelectedIndex = 0;
            movie.SelectedIndexChanged += (s,e) => UpdateRooms();
            room.SelectedIndexChanged += (s,e) => RefreshSeats();
            seats.ItemCheck += (s,e) => BeginInvoke((Action)UpdateTotal);
            var top = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 105, Padding = new Padding(8) };
            top.Controls.Add(new Label { Text = "Khách hàng:", AutoSize = true, Padding = new Padding(0,8,0,0) }); top.Controls.Add(customer);
            top.Controls.Add(new Label { Text = "Phim:", AutoSize = true, Padding = new Padding(0,8,0,0) }); top.Controls.Add(movie);
            top.Controls.Add(new Label { Text = "Phòng:", AutoSize = true, Padding = new Padding(0,8,0,0) }); top.Controls.Add(room);
            var mid = new FlowLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(12) };
            var seatPanel = new FlowLayoutPanel { FlowDirection = FlowDirection.TopDown, Width = 230 };
            seatPanel.Controls.Add(new Label { Text = "Chọn ghế (A/B/C, mỗi hàng 1–5):", AutoSize = true }); seatPanel.Controls.Add(seats); seatPanel.Controls.Add(total);
            mid.Controls.Add(seatPanel);
            var right = new FlowLayoutPanel { FlowDirection = FlowDirection.TopDown, Width = 310 };
            right.Controls.Add(Ui.Button("Bán vé", Sell)); right.Controls.Add(Ui.Button("Đọc lại input5.txt", LoadData));
            right.Controls.Add(Ui.Button("Xuất báo cáo output5.txt", ExportReport)); right.Controls.Add(new Label { Text = "Tiến độ xuất báo cáo:", AutoSize = true }); right.Controls.Add(progress);
            right.Controls.Add(new Label { Text = "Giá: vé vớt 1/4, thường 1x, VIP 2x.", AutoSize = true, MaximumSize = new Size(300, 70) });
            mid.Controls.Add(right);
            Controls.Add(mid); Controls.Add(top);
            UpdateRooms(); RefreshSeats();
        }
        private void UpdateRooms()
        {
            if (movie.SelectedItem == null) return;
            string m = movie.SelectedItem.ToString();
            string allowed = m == "Đào, phở và piano" ? "1,2,3" : m == "Mai" ? "2,3" : m == "Gặp lại chị bầu" ? "1" : "3";
            var allowedRooms = allowed.Split(',');
            string previous = room.SelectedItem == null ? "" : room.SelectedItem.ToString();
            room.Items.Clear(); room.Items.AddRange(allowedRooms.Cast<object>().ToArray());
            room.SelectedItem = allowedRooms.Contains(previous) ? previous : allowedRooms[0];
            RefreshSeats();
        }
        private string SeatKey(string r, string seat) => r + "-" + seat;
        private void RefreshSeats()
        {
            if (room.SelectedItem == null) return;
            seats.Items.Clear();
            foreach (string row in new[] { "A", "B", "C" })
                for (int i = 1; i <= 5; i++)
                {
                    string seat = row + i;
                    if (seat == "B1" || seat == "B5") continue;
                    string key = SeatKey(room.SelectedItem.ToString(), seat);
                    if (!sold.Contains(key)) seats.Items.Add(seat);
                }
            UpdateTotal();
        }
        private int SeatPrice(string seat, string film)
        {
            int standard = prices[film];
            if (seat == "A1" || seat == "A5" || seat == "C1" || seat == "C5") return standard / 4;
            if (seat.StartsWith("B")) return standard * 2;
            return standard;
        }
        private void UpdateTotal()
        {
            if (movie.SelectedItem == null) return;
            int sum = 0;
            foreach (object item in seats.CheckedItems) sum += SeatPrice(item.ToString(), movie.SelectedItem.ToString());
            total.Text = "Tổng tiền: " + sum.ToString("N0") + " đ";
        }
        private void Sell(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(customer.Text)) throw new Exception("Nhập tên khách hàng.");
                if (seats.CheckedItems.Count == 0) throw new Exception("Chọn ít nhất một ghế.");
                if (seats.CheckedItems.Count > 2) throw new Exception("Mỗi lần chỉ được chọn tối đa 2 vé.");
                string r = room.SelectedItem.ToString(), film = movie.SelectedItem.ToString();
                var selected = seats.CheckedItems.Cast<object>().Select(x => x.ToString()).ToList();
                int revenue = selected.Sum(s => SeatPrice(s, film));
                foreach (string s in selected) { sold.Add(SeatKey(r, s)); sales.Add(film + "|" + r + "|" + s + "|" + SeatPrice(s, film) + "|" + customer.Text.Trim()); }
                File.AppendAllLines(AppPaths.PathFor("input5.txt"), sales.Skip(Math.Max(0, sales.Count - selected.Count)), new UTF8Encoding(true));
                MessageBox.Show("Bán vé thành công.\nKhách: " + customer.Text + "\nPhim: " + film + "\nPhòng: " + r +
                    "\nGhế: " + string.Join(", ", selected) + "\nTổng: " + revenue.ToString("N0") + " đ");
                RefreshSeats();
            }
            catch (Exception ex) { Ui.Error(this, ex); }
        }
        private void LoadData(object sender, EventArgs e)
        {
            try
            {
                string path = AppPaths.PathFor("input5.txt");
                if (!File.Exists(path)) throw new FileNotFoundException("Không tìm thấy input5.txt.", path);
                sold.Clear(); sales.Clear();
                foreach (string line in File.ReadAllLines(path, Encoding.UTF8))
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;
                    string[] p = line.Split('|');
                    if (p.Length < 5) continue;
                    sold.Add(SeatKey(p[1], p[2])); sales.Add(line);
                }
                RefreshSeats(); MessageBox.Show("Đã nạp " + sales.Count + " vé từ input5.txt.");
            }
            catch (Exception ex) { Ui.Error(this, ex); }
        }
        private void ExportReport(object sender, EventArgs e)
        {
            try
            {
                progress.Value = 0;
                var lines = new List<string> { "BÁO CÁO DOANH THU PHÒNG VÉ", "Phim | Vé bán | Vé còn lại | Tỷ lệ bán | Doanh thu" };
                int n = prices.Count, i = 0;
                var report = prices.Select(kv =>
                {
                    var matching = sales.Where(s => s.StartsWith(kv.Key + "|", StringComparison.Ordinal)).ToList();
                    int soldCount = matching.Count;
                    int totalSeats = 13; // A1-5, B2-4, C1-5 = 13 seats available by Lab 1 tiers.
                    long revenue = matching.Sum(s => { string[] p=s.Split('|'); int v; return p.Length > 3 && int.TryParse(p[3], out v) ? v : 0; });
                    return new { Film = kv.Key, Sold = soldCount, Left = Math.Max(0, totalSeats - soldCount), Rate = soldCount * 100.0 / totalSeats, Revenue = revenue };
                }).OrderByDescending(x => x.Revenue).ToList();
                foreach (var r in report)
                {
                    lines.Add(r.Film + " | " + r.Sold + " | " + r.Left + " | " + r.Rate.ToString("0.00") + "% | " + r.Revenue.ToString("N0") + " đ");
                    i++; progress.Value = i * 100 / n; Application.DoEvents();
                }
                lines.Add(""); lines.Add("Xếp hạng doanh thu:");
                int rank = 1; foreach (var r in report) lines.Add((rank++) + ". " + r.Film + " — " + r.Revenue.ToString("N0") + " đ");
                File.WriteAllLines(AppPaths.PathFor("output5.txt"), lines, new UTF8Encoding(true));
                MessageBox.Show("Đã xuất output5.txt.");
            }
            catch (Exception ex) { Ui.Error(this, ex); }
        }
    }

    internal sealed class FrmBai06 : Form
    {
        private readonly string dbPath = AppPaths.PathFor("meals.db");
        private readonly DataGridView grid = new DataGridView { Dock = DockStyle.Fill, ReadOnly = true, AllowUserToAddRows = false,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };
        private readonly TextBox mealName = new TextBox { Width = 180 };
        private readonly TextBox contributor = new TextBox { Width = 180 };
        private readonly TextBox imagePath = new TextBox { Width = 240 };
        private readonly Label randomResult = new Label { AutoSize = true, Font = new Font("Segoe UI", 12, FontStyle.Bold) };
        private readonly PictureBox picMonAn = new PictureBox
        {
            Name = "picMonAn",
            Size = new Size(250, 180),
            SizeMode = PictureBoxSizeMode.Zoom,
            BorderStyle = BorderStyle.FixedSingle,
            BackColor = Color.White
        };
        public FrmBai06()
        {
            Text = "Bài 06 - Hôm nay ăn gì? (SQLite)"; Size = new Size(950, 620); StartPosition = FormStartPosition.CenterParent;
            var inputs = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 95, Padding = new Padding(8) };
            inputs.Controls.Add(new Label { Text = "Tên món:", AutoSize = true, Padding = new Padding(0,8,0,0) }); inputs.Controls.Add(mealName);
            inputs.Controls.Add(new Label { Text = "Người đóng góp:", AutoSize = true, Padding = new Padding(0,8,0,0) }); inputs.Controls.Add(contributor);
            inputs.Controls.Add(new Label { Text = "Đường dẫn ảnh:", AutoSize = true, Padding = new Padding(0,8,0,0) }); inputs.Controls.Add(imagePath);
            var buttons = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 45, Padding = new Padding(8) };
            buttons.Controls.Add(Ui.Button("Khởi tạo CSDL / nạp mẫu", InitDb));
            buttons.Controls.Add(Ui.Button("Thêm món", AddMeal));
            buttons.Controls.Add(Ui.Button("Tải danh sách", LoadMeals));
            buttons.Controls.Add(Ui.Button("Chọn món ngẫu nhiên", RandomMeal));
            var bottom = new FlowLayoutPanel { Dock = DockStyle.Bottom, Height = 50, Padding = new Padding(8) }; bottom.Controls.Add(randomResult);

            grid.SelectionChanged += (s, e) =>
            {
                if (grid.CurrentRow == null ||
                    grid.CurrentRow.IsNewRow)
                    return;

                object value = grid.CurrentRow.Cells["HinhAnh"].Value;

                HienThiHinhAnh(
                    value == null || value == DBNull.Value
                        ? ""
                        : value.ToString()
                );
            };
            // Khung hiển thị hình ảnh món ăn
            var imagePanel = new Panel
            {
                Dock = DockStyle.Right,
                Width = 270,
                Padding = new Padding(10)
            };

            picMonAn.Dock = DockStyle.Top;
            imagePanel.Controls.Add(picMonAn);

            // Chừa không gian bên phải cho ảnh
            grid.Dock = DockStyle.Fill;

            Controls.Add(grid);
            Controls.Add(imagePanel);
            Controls.Add(bottom);
            Controls.Add(buttons);
            Controls.Add(inputs);

            try { EnsureDb(); LoadMeals(null, EventArgs.Empty); } catch { }
           
        }
        private SQLiteConnection Connect() => new SQLiteConnection("Data Source=" + dbPath + ";Version=3;");
        private void EnsureDb()
        {
            using (var con = Connect())
            {
                con.Open();
                using (var cmd = new SQLiteCommand(con))
                {
                    cmd.CommandText = @"CREATE TABLE IF NOT EXISTS NguoiDung(IDNCC INTEGER PRIMARY KEY AUTOINCREMENT, HoVaTen TEXT NOT NULL, QuyenHan TEXT NOT NULL DEFAULT 'User');
CREATE TABLE IF NOT EXISTS MonAn(IDMA INTEGER PRIMARY KEY AUTOINCREMENT, TenMonAn TEXT NOT NULL, HinhAnh TEXT, IDNCC INTEGER, FOREIGN KEY(IDNCC) REFERENCES NguoiDung(IDNCC));";
                    cmd.ExecuteNonQuery();
                }
            }
        }
        private void InitDb(object sender, EventArgs e)
        {
            try
            {
                EnsureDb();
                using (var con = Connect())
                {
                    con.Open();
                    using (var cmd = new SQLiteCommand("SELECT COUNT(*) FROM NguoiDung", con))
                    {
                        if (Convert.ToInt32(cmd.ExecuteScalar()) == 0)
                        {
                            using (var insert = new SQLiteCommand(@"
                                    INSERT INTO NguoiDung(HoVaTen, QuyenHan)
                                    VALUES
                                        ('Nguyen Van An', 'Admin'),
                                        ('Tran Thi Binh', 'User'),
                                        ('Le Van Cuong', 'User');

                                    INSERT INTO MonAn(TenMonAn, HinhAnh, IDNCC)
                                    VALUES
                                        ('Phở', 'Images/pho.png', 1),
                                        ('Cơm tấm', 'Images/comtam.png', 2),
                                        ('Bún bò Huế', 'Images/bunbo.png', 3);
                                ", con)) 
                                
                                insert.ExecuteNonQuery();   
                            
                   
                        }
                    }
                }
                LoadMeals(null, EventArgs.Empty); MessageBox.Show("Đã khởi tạo CSDL meals.db.");
            }
            catch (Exception ex) { Ui.Error(this, ex); }
        }
        private void AddMeal(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(mealName.Text) || string.IsNullOrWhiteSpace(contributor.Text))
                    throw new Exception("Nhập tên món và tên người đóng góp.");
                EnsureDb();
                using (var con = Connect())
                {
                    con.Open(); long userId;
                    using (var cmd = new SQLiteCommand("SELECT IDNCC FROM NguoiDung WHERE HoVaTen=@n LIMIT 1", con))
                    {
                        cmd.Parameters.AddWithValue("@n", contributor.Text.Trim());
                        object existing = cmd.ExecuteScalar();
                        if (existing == null)
                        {
                            using (var ins = new SQLiteCommand("INSERT INTO NguoiDung(HoVaTen,QuyenHan) VALUES(@n,'User'); SELECT last_insert_rowid();", con))
                            { ins.Parameters.AddWithValue("@n", contributor.Text.Trim()); userId = Convert.ToInt64(ins.ExecuteScalar()); }
                        }
                        else userId = Convert.ToInt64(existing);
                    }
                    using (var cmd = new SQLiteCommand("INSERT INTO MonAn(TenMonAn,HinhAnh,IDNCC) VALUES(@m,@i,@u)", con))
                    {
                        cmd.Parameters.AddWithValue("@m", mealName.Text.Trim()); cmd.Parameters.AddWithValue("@i", imagePath.Text.Trim()); cmd.Parameters.AddWithValue("@u", userId); cmd.ExecuteNonQuery();
                    }
                }
                LoadMeals(null, EventArgs.Empty);
            }
            catch (Exception ex) { Ui.Error(this, ex); }
        }
        private void LoadMeals(object sender, EventArgs e)
        {
            try
            {
                EnsureDb();
                using (var con = Connect())
                {
                    con.Open();
                    using (var da = new SQLiteDataAdapter("SELECT m.IDMA, m.TenMonAn, m.HinhAnh, n.HoVaTen AS NguoiDongGop FROM MonAn m LEFT JOIN NguoiDung n ON m.IDNCC=n.IDNCC ORDER BY m.TenMonAn", con))
                    {
                        var dt = new DataTable(); da.Fill(dt); grid.DataSource = dt;
                    }
                }
            }
            catch (Exception ex) { Ui.Error(this, ex); }
        }
        private void RandomMeal(object sender, EventArgs e)
        {
            try
            {
                using (var con = Connect())
                {
                    con.Open();
                    using (var cmd = new SQLiteCommand("SELECT m.TenMonAn, n.HoVaTen, m.HinhAnh\r\nFROM MonAn m\r\nLEFT JOIN NguoiDung n ON m.IDNCC=n.IDNCC\r\nORDER BY RANDOM() LIMIT 1", con))
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (!reader.Read())
                        {
                            randomResult.Text = "Chưa có món ăn. Hãy thêm món trước.";
                            HienThiHinhAnh("");
                            return;
                        }

                        string tenMon = reader.IsDBNull(0)
                            ? "Không rõ"
                            : reader.GetString(0);

                        string tenNguoi = reader.IsDBNull(1)
                            ? "Không rõ"
                            : reader.GetString(1);

                        string anh = reader.IsDBNull(2)
                            ? ""
                            : reader.GetString(2);

                        randomResult.Text =
                            "Hôm nay ăn: " + tenMon +
                            " — đóng góp bởi " + tenNguoi;

                        HienThiHinhAnh(anh);
                    }
                }
            }
            catch (Exception ex) { Ui.Error(this, ex); }
        }


        private void HienThiHinhAnh(string duongDan)
        {
            // Giải phóng ảnh cũ trước khi hiển thị ảnh mới
            var oldImage = picMonAn.Image;
            picMonAn.Image = null;
            oldImage?.Dispose();

            if (string.IsNullOrWhiteSpace(duongDan))
                return;

            string path = duongDan.Trim();

            // Hỗ trợ đường dẫn tương đối
            if (!Path.IsPathRooted(path))
            {
                path = Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    path
                );
            }

            if (!File.Exists(path))
            {
                randomResult.Text = "Không tìm thấy ảnh: " + path;
                return;
            }

            try
            {
                // Đọc ảnh mà không giữ khóa file
                using (Image img = Image.FromFile(path))
                {
                    picMonAn.Image = new Bitmap(img);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể tải hình ảnh món ăn.\n" + ex.Message,
                    "Lỗi hình ảnh",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
            }
        }

    }

    internal sealed class FrmBai07 : Form
    {
        private readonly TreeView tree = new TreeView { Dock = DockStyle.Left, Width = 330 };
        private readonly RichTextBox preview = new RichTextBox { Dock = DockStyle.Fill, ReadOnly = true, Font = new Font("Consolas", 10) };
        private readonly Label path = new Label { Dock = DockStyle.Top, Height = 30, AutoEllipsis = true };
        private readonly PictureBox imagePreview = new PictureBox
        {
            Dock = DockStyle.Fill,
            SizeMode = PictureBoxSizeMode.Zoom,
            Visible = false,
            BackColor = Color.White
        };
        public FrmBai07()
        {
            Text = "Bài 07 - Duyệt file và thư mục"; Size = new Size(1000, 650); StartPosition = FormStartPosition.CenterParent;
            tree.BeforeExpand += ExpandNode; tree.AfterSelect += SelectNode; tree.NodeMouseDoubleClick += DoubleClickNode;
            var top = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 42 };
            top.Controls.Add(Ui.Button("Chọn thư mục gốc", BrowseRoot));
            var right = new Panel { Dock = DockStyle.Fill };

            right.Controls.Add(preview);
            right.Controls.Add(imagePreview);
            right.Controls.Add(path);

            Controls.Add(right);
            Controls.Add(tree);
            Controls.Add(top);
            LoadRoots();
        }
        private void LoadRoots()
        {
            tree.Nodes.Clear();
            foreach (var drive in DriveInfo.GetDrives())
            {
                var node = new TreeNode(drive.Name) { Tag = drive.RootDirectory.FullName };
                node.Nodes.Add(new TreeNode("...")); tree.Nodes.Add(node);
            }
        }
        private void BrowseRoot(object sender, EventArgs e)
        {
            using (var dlg = new FolderBrowserDialog())
                if (dlg.ShowDialog() == DialogResult.OK) { tree.Nodes.Clear(); AddDirectory(dlg.SelectedPath, null); }
        }
        private void AddDirectory(string dir, TreeNode parent)
        {
            var node = new TreeNode(Path.GetFileName(dir.TrimEnd(Path.DirectorySeparatorChar)) is string n && n.Length > 0 ? n : dir) { Tag = dir };
            if (parent == null) tree.Nodes.Add(node); else parent.Nodes.Add(node);
            node.Nodes.Add(new TreeNode("..."));
        }
        private void ExpandNode(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node.Nodes.Count == 1 && e.Node.Nodes[0].Text == "...")
            {
                e.Node.Nodes.Clear();
                try
                {
                    string dir = Convert.ToString(e.Node.Tag);
                    foreach (string sub in Directory.GetDirectories(dir))
                    {
                        var child = new TreeNode(Path.GetFileName(sub)) { Tag = sub };
                        child.Nodes.Add(new TreeNode("...")); e.Node.Nodes.Add(child);
                    }
                    foreach (string file in Directory.GetFiles(dir))
                        e.Node.Nodes.Add(new TreeNode(Path.GetFileName(file)) { Tag = file, ImageKey = "file" });
                }
                catch (Exception ex) { e.Node.Nodes.Add(new TreeNode("[Truy cập bị từ chối] " + ex.Message)); }
            }
        }

        private void SelectNode(object sender, TreeViewEventArgs e)
        {
            string p = Convert.ToString(e.Node.Tag);
            path.Text = p;

            // Ẩn và giải phóng ảnh trước đó
            var oldImage = imagePreview.Image;
            imagePreview.Image = null;
            oldImage?.Dispose();

            imagePreview.Visible = false;
            preview.Visible = true;
            preview.Clear();

            if (string.IsNullOrEmpty(p) || Directory.Exists(p))
                return;

            try
            {
                var fi = new FileInfo(p);

                if (fi.Length > 2 * 1024 * 1024)
                {
                    preview.Text =
                        "File lớn hơn 2 MB, không hiển thị trực tiếp.";
                    return;
                }

                string ext = Path.GetExtension(p).ToLowerInvariant();

                // Các định dạng ảnh được hỗ trợ
                string[] imageExtensions =
                {
            ".png", ".jpg", ".jpeg", ".bmp",
            ".gif", ".tif", ".tiff"
        };

                if (imageExtensions.Contains(ext))
                {
                    using (Image img = Image.FromFile(p))
                    {
                        imagePreview.Image = new Bitmap(img);
                    }

                    preview.Visible = false;
                    imagePreview.Visible = true;
                    return;
                }

                // Đọc file văn bản
                byte[] bytes = File.ReadAllBytes(p);

                if (bytes.Any(b => b == 0))
                {
                    preview.Text = "Đây là file nhị phân, không thể xem như văn bản.";
                    return;
                }

                preview.Text = File.ReadAllText(p, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                preview.Text = "Không thể đọc file: " + ex.Message;
            }
        }

        private void DoubleClickNode(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Tag != null && Directory.Exists(Convert.ToString(e.Node.Tag))) e.Node.Expand();
        }
    }
}
