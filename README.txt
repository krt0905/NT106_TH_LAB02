LAB 02 - FILE AND I/O STREAM (C# WinForms .NET Framework 4.8)
============================================================

CẤU TRÚC
- Lab02.sln: solution Visual Studio
- Lab02/: source code, các file input mẫu
- Mỗi bài là một Form riêng, mở từ FrmMain.

CÁCH MỞ
1. Giải nén ZIP.
2. Mở Lab02.sln bằng Visual Studio 2022.
3. Nếu máy chưa có .NET Framework 4.8 Developer Pack, cài đặt hoặc đổi TargetFrameworkVersion trong Lab02.csproj sang phiên bản đã cài.
4. Nhấp phải project > Restore NuGet Packages. Bài 06 dùng System.Data.SQLite.Core.
5. Build Solution (Ctrl+Shift+B), chạy (F5).

LƯU Ý
- Bài 01/03/04/05 tạo file output trong thư mục chạy, thường là Lab02/bin/Debug.
- Bài 03 hỗ trợ + - * /, ngoặc, số thập phân, dấu âm; không dùng DataTable.Compute.
- Bài 04 dùng định dạng dòng: MSSV|HoTen|DienThoai|Diem1|Diem2|Diem3. Điểm trung bình được tính khi hiển thị/xuất.
- Bài 05 kế thừa dữ liệu phim/giá/phòng từ Lab 1. Vé vớt: A1,A5,C1,C5 (1/4 giá chuẩn); ghế thường: A2,A3,A4,C2,C3,C4 (giá chuẩn); VIP: B2,B3,B4 (2 lần giá chuẩn). B1/B5 không được nêu loại vé trong tài liệu Lab 1 nên mẫu này không bán hai ghế đó. Phòng được phép: Đào, phở và piano (1,2,3); Mai (2,3); Gặp lại chị bầu (1); Tarot (3). Mẫu giới hạn tối đa 2 vé mỗi lần bán.
- Bài 05: input5.txt là dữ liệu bán vé nối dòng theo định dạng Phim|Phòng|Ghế|Giá|Khách. Không xóa input5.txt nếu muốn giữ dữ liệu bán.
- Bài 06: database meals.db tự tạo cạnh chương trình. Nếu SQLite native dependency không nạp được, Restore NuGet Packages và chọn platform target phù hợp (thường x86/x64).
- Bài 07: duyệt file/thư mục theo quyền của tài khoản Windows; file lớn hơn 2 MB không preview và file có dấu hiệu nhị phân không hiển thị dạng text.

ĐỂ NỘP BÀI
Đổi tên ZIP theo quy định lớp, ví dụ: Code-<MaLop>-Lab02_<MSSV>.zip.
Đây là bộ mã khởi đầu hoàn chỉnh để học và kiểm thử; hãy đối chiếu từng yêu cầu cụ thể trong PDF Lab 2 và điều chỉnh giao diện/định dạng báo cáo nếu giảng viên quy định khác.
